using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

	public static Manager instance = null;

	public Toggle rotationToggle;
	public Toggle distanceToggle;

	public Transform trackPiecePrefab;
	public Transform ballPrefab;

	public float scaleFactor = .5f;
	public float gameRadius = 1.5f;
	public float speedForDistanceChange = 1.2f;
	public float speedForRotation = .9f;

	Vector3 initialCameraPosition;

	GameObject hitObject = null;

	Vector2 touch2Position = Vector2.zero;
	Vector3 positionOffset;
	Vector3 currentEulerAngles;

	void Awake()
	{
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy (gameObject);    

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad (gameObject);
	}

	void Update () {
		if (Input.touchCount > 0 && trackPiecePrefab != null) {

			if (Input.touchCount > 1 && hitObject) {

				var touch = Input.GetTouch (1);

				if (touch.phase == TouchPhase.Began) {

					touch2Position = touch.position;
				} if (touch.phase == TouchPhase.Moved && touch2Position != Vector2.zero) {
					Vector2 newTouch2Position = touch.position;
					float difference = newTouch2Position.y - touch2Position.y;

					if (rotationToggle.isOn) {
						// Rotate around y axis
						hitObject.transform.Rotate (0, difference, 0);
					} else if (distanceToggle.isOn) {
						// Move closer or further
						Vector3 touchPositionInWorld = Camera.main.ScreenToWorldPoint (touch.position);
						Debug.Log ("Moving towards");
						hitObject.transform.position = Vector3.MoveTowards (hitObject.transform.position, touchPositionInWorld, speedForDistanceChange * (difference > 0 ? -Time.deltaTime : Time.deltaTime));
					}
						
					touch2Position = newTouch2Position;
				} else if (touch.phase == TouchPhase.Ended && touch2Position != Vector2.zero) {
					touch2Position = Vector2.zero;
					hitObject = null; // this is so that we don't bounce right back to touch position in the 1 touch logic below
				}

			} else { // one touch

				var touch = Input.GetTouch(0);

				if (touch.phase == TouchPhase.Began) {

					Ray ray = Camera.main.ScreenPointToRay (touch.position);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit)) {
						Debug.Log (" You just hit " + hit.collider.gameObject.name);

						if (hit.transform.CompareTag ("TrackParent")) {
							Debug.Log ("- a TrackParent!");
							hitObject = hit.transform.gameObject;
							positionOffset = hit.transform.position - hit.point;
							currentEulerAngles = hit.transform.eulerAngles;
						}
					} else {

						Vector3 touchPos = touch.position;
						touchPos.z = gameRadius;

						Vector3 position = Camera.main.ScreenToWorldPoint (touchPos);
						Vector3 cameraEulerAngles = Camera.main.transform.eulerAngles;

						Transform trackPiece = Transform.Instantiate (trackPiecePrefab);
						trackPiece.position = position;
						trackPiece.Rotate (0, cameraEulerAngles.y, 0);

						Vector3 trackScale = trackPiece.localScale;
						trackPiece.localScale = new Vector3 (trackScale.x * scaleFactor, trackScale.y * scaleFactor, trackScale.z * scaleFactor);
					}
				} else if (touch.phase == TouchPhase.Moved && hitObject) {

					Vector3 touchPos = touch.position;
					touchPos.z = gameRadius;

					Vector3 position = Camera.main.ScreenToWorldPoint (touchPos);
					Vector3 cameraEulerAngles = Camera.main.transform.eulerAngles;

					Transform trackPiece = hitObject.transform;
					trackPiece.position = new Vector3 (position.x + positionOffset.x, position.y + positionOffset.y, position.z + positionOffset.y);
					Vector3 eulerAngles = trackPiece.eulerAngles;
					trackPiece.eulerAngles = new Vector3 (0, currentEulerAngles.y + cameraEulerAngles.y, 0);

				} else if (touch.phase == TouchPhase.Ended && hitObject) {
					hitObject = null;
					touch2Position = Vector2.zero;
				}
			}
		}
	}

	private Transform GetHighestPiece() {
		GameObject[] trackParents = GameObject.FindGameObjectsWithTag ("TrackParent");

		Transform highestPiece = null;
		foreach (GameObject trackParent in trackParents) {
			if (highestPiece == null || highestPiece.position.y < trackParent.transform.position.y) {
				highestPiece = trackParent.transform;
			}
		}
		return highestPiece;
	}

	public void DropBall() {

		Debug.Log ("Dropball*******************");
		Vector3 startPosition = new Vector3 (0, 2, 1.5f);

		Transform highestPiece = GetHighestPiece ();
		if (highestPiece != null) {
			startPosition = Vector3.zero;

			Transform startPoint = highestPiece.Find ("StartPoint");
			if (startPoint != null) {
				startPosition = startPoint.transform.position;
			}
		}
		startPosition.y += .2f;
		Debug.Log ("Instantiating Ball at " + startPosition.ToString ());
		Transform ball = Transform.Instantiate (ballPrefab, startPosition, Quaternion.identity);
		Vector3 ballScale = ball.localScale;
		ball.localScale = new Vector3 (ballScale.x * scaleFactor, ballScale.y * scaleFactor, ballScale.z * scaleFactor);

	}

	public void Reset() {
		GameObject[] trackParents = GameObject.FindGameObjectsWithTag ("TrackParent");

		foreach (GameObject trackParent in trackParents) {
			GameObject.Destroy (trackParent);
		}
	}
		
	public void RotationToggleValueChanged(Toggle change) {
		distanceToggle.isOn = !rotationToggle.isOn;
	}

	public void DistanceToggleValueChanged(Toggle change) {
		rotationToggle.isOn = !distanceToggle.isOn;
	}

}
