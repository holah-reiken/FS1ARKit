using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	public static Manager instance = null;

	public Transform trackPiecePrefab;
	public Transform ballPrefab;

	public float scaleFactor = .5f;
	public float gameRadius = 1.5f;

	Vector3 initialCameraPosition;

	GameObject hitObject = null;
	Transform highestPiece = null;

	bool trackPiecePlaced = false;

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

			var touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began) {

				Ray ray = Camera.main.ScreenPointToRay (touch.position);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					Debug.Log (" You just hit " + hit.collider.gameObject.name);

					if (hit.transform.CompareTag ("TrackParent")) {
						Debug.Log ("- a TrackParent!");
						hitObject = hit.transform.gameObject;
					}
				} else {

					Vector3 touchPos = touch.position;
					Debug.Log ("touchPos=" + touchPos.ToString ());
					touchPos.z = gameRadius;

					Vector3 position = Camera.main.ScreenToWorldPoint (touchPos);
					Vector3 cameraEulerAngles = Camera.main.transform.eulerAngles;

					Transform trackPiece = Transform.Instantiate (trackPiecePrefab);
					trackPiece.position = position;
					trackPiece.Rotate (0, cameraEulerAngles.y, 0);

					Vector3 trackScale = trackPiece.localScale;
					trackPiece.localScale = new Vector3 (trackScale.x * scaleFactor, trackScale.y * scaleFactor, trackScale.z * scaleFactor);

					if (highestPiece == null || highestPiece.position.y < position.y) {
						highestPiece = trackPiece;
					}
				}

			} else if (touch.phase == TouchPhase.Moved && hitObject) {

				Vector3 touchPos = touch.position;
				Debug.Log ("touchPos=" + touchPos.ToString ());
				touchPos.z = gameRadius;

				Vector3 position = Camera.main.ScreenToWorldPoint (touchPos);
				Vector3 cameraEulerAngles = Camera.main.transform.eulerAngles;

				Transform trackPiece = hitObject.transform;// Transform.Instantiate (trackPiecePrefab);
				trackPiece.position = position;
				trackPiece.eulerAngles = new Vector3 (0, 90 + cameraEulerAngles.y, 0);
				//trackPiece.Rotate (0, cameraEulerAngles.y, 0);

			} else if (touch.phase == TouchPhase.Ended && hitObject) {
				hitObject = null;
			}
		}
	}

	public void DropBall() {

		Debug.Log ("Dropball*******************");
		Vector3 startPosition = new Vector3 (0, 2, 1.5f);

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
		

}
