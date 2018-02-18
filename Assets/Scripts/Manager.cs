using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	public static Manager instance = null;

	public Transform trackPiecePrefab;
	public Transform ballPrefab;
	public Transform centerPrefab;
	public Transform testPrefab;

	public float scaleFactor = .5f;
	public float gameRadius = 1.45f;

	Vector3 gameCenter = Vector3.zero;
	Vector3 initialCameraPosition;
	bool gameCenterSet = false;


	Transform centerObject = null;

	float y = 0f; //HO-R

	GameObject hitObject = null;

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
					hitObject = hit.transform.gameObject;
				} else {

					Vector3 touchPos = /*gameCenter; // HO-R */touch.position;
					Debug.Log ("touchPos=" + touchPos.ToString ());
					touchPos.z = gameRadius;

					Vector3 position = Camera.main.ScreenToWorldPoint (touchPos);

					Transform trackPiece = Transform.Instantiate (trackPiecePrefab);
					trackPiece.position = position;

					Debug.Log ("Placed at " + position.ToString ());

					Vector3 trackScale = trackPiece.localScale;
					trackPiece.localScale = new Vector3 (trackScale.x * scaleFactor, trackScale.y * scaleFactor, trackScale.z * scaleFactor);
				}
			} else if (touch.phase == TouchPhase.Moved && hitObject) {

				Debug.Log ("Moving hit object from " + hitObject.transform.position.ToString ());
				Vector3 touchPos = touch.position;
				Debug.Log ("touchPos=" + touchPos.ToString ());
				touchPos.z = hitObject.transform.position.z;// gameRadius;

				Vector3 position = Camera.main.ScreenToWorldPoint (touchPos);
				position.z = hitObject.transform.position.z;
				Debug.Log ("Moved to " + position.ToString ());

				Vector3 newPosition = Camera.main.ScreenToWorldPoint (touch.position);
				hitObject.transform.position = position; 
			} else if (touch.phase == TouchPhase.Ended && hitObject) {
				hitObject = null;
			
				/*
				if (centerObject == null) {
					centerObject = Transform.Instantiate (centerPrefab, Camera.main.ScreenToWorldPoint (touch.position), Quaternion.identity);
				}

				Transform.Instantiate (testPrefab, new Vector3 (0, y, gameRadius), Quaternion.identity, centerObject);
				Transform parentedTrackPiece = Transform.Instantiate (trackPiecePrefab, new Vector3 (0, y, gameRadius), Quaternion.identity, centerObject);
				trackScale = parentedTrackPiece.localScale;
				parentedTrackPiece.localScale = new Vector3 (trackScale.x * scaleFactor * 10, trackScale.y * scaleFactor * 10, trackScale.z * scaleFactor * 10);
				y += .02f;
				*/

				/*
				if (!gameCenterSet) {
					gameCenter = touch.position;
					gameCenterSet = true;
					initialCameraPosition = Camera.main.ScreenToWorldPoint (touch.position);
					centerObject = GameObject.CreatePrimitive (PrimitiveType.Sphere);
					centerObject.transform.position = initialCameraPosition;
					centerObject.GetComponent<Renderer> ().material.color = Color.red;
					centerObject.transform.localScale = new Vector3(.5f, .5f, 5f);
					centerObject.transform.position = gameCenter;
				}

				Transform trackPiece = Transform.Instantiate (trackPiecePrefab, centerObject.transform);
				trackPiece.localPosition = new Vector3 (0, y, 0);
				y += .1f;
				*/
				/*
				Vector3 currentCameraPosition = Camera.main.ScreenToWorldPoint (touch.position);

				float xChange = currentCameraPosition.x = initialCameraPosition.x;
				float yChange = currentCameraPosition.y = initialCameraPosition.y;
				float zChange = currentCameraPosition.z = initialCameraPosition.z;

				Vector3 touchPos = gameCenter; // HO-R touch.position;
				Debug.Log("touchPos="+touchPos.ToString());
				touchPos.z = gameRadius;
				Vector3 position = Camera.main.ScreenToWorldPoint (touchPos);

				position.x -= xChange;
				position.y -= yChange;
				position.z -= zChange;

				Transform trackPiece = Transform.Instantiate (trackPiecePrefab);
				trackPiece.position = position;

				Vector3 trackScale = trackPiece.localScale;
				trackPiece.localScale = new Vector3 (trackScale.x * scaleFactor, trackScale.y * scaleFactor, trackScale.z * scaleFactor);

				Transform ball = Transform.Instantiate (ballPrefab);
				ball.position = new Vector3 (position.x, position.y + .5f, position.z + .2f);

				Vector3 ballScale = ball.localScale;
				ball.localScale = new Vector3 (ballScale.x * scaleFactor, ballScale.y * scaleFactor, ballScale.z * scaleFactor);

				Debug.Log ("trackPiece.position=" + trackPiece.position.ToString () + ", touch.position=" + touch.position.ToString ());
				*/

			} else if (false && touch.phase == TouchPhase.Moved) {
				Ray ray = Camera.main.ScreenPointToRay (touch.position);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					Debug.Log (" You just hit " + hit.collider.gameObject.name);


					Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, hit.collider.transform.position.z));
					// lerp and set the position of the current object to that of the touch, but smoothly over time.
					hit.collider.transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime);
					/*


					Vector3 touchPos = touch.position;
					touchPos.z = gameRadius;// hit.transform.position.z;

					Vector3 position = Camera.main.ScreenToWorldPoint (touchPos);
					hit.transform.position = position;
					*/
				}
			}
		}
	}

	void XUpdate () {
		if (Input.touchCount > 0 && trackPiecePrefab != null) {

			var touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began) {

				Vector3 touchPos = /*gameCenter; // HO-R */touch.position;
				Debug.Log("touchPos="+touchPos.ToString());
				touchPos.z = gameRadius;

				Vector3 position = Camera.main.ScreenToWorldPoint (touchPos);

				Transform trackPiece = Transform.Instantiate (trackPiecePrefab);
				trackPiece.position = position;

				Vector3 trackScale = trackPiece.localScale;
				trackPiece.localScale = new Vector3 (trackScale.x * scaleFactor, trackScale.y * scaleFactor, trackScale.z * scaleFactor);

				/*
				if (centerObject == null) {
					centerObject = Transform.Instantiate (centerPrefab, Camera.main.ScreenToWorldPoint (touch.position), Quaternion.identity);
				}

				Transform.Instantiate (testPrefab, new Vector3 (0, y, gameRadius), Quaternion.identity, centerObject);
				Transform parentedTrackPiece = Transform.Instantiate (trackPiecePrefab, new Vector3 (0, y, gameRadius), Quaternion.identity, centerObject);
				trackScale = parentedTrackPiece.localScale;
				parentedTrackPiece.localScale = new Vector3 (trackScale.x * scaleFactor * 10, trackScale.y * scaleFactor * 10, trackScale.z * scaleFactor * 10);
				y += .02f;
				*/

				/*
				if (!gameCenterSet) {
					gameCenter = touch.position;
					gameCenterSet = true;
					initialCameraPosition = Camera.main.ScreenToWorldPoint (touch.position);
					centerObject = GameObject.CreatePrimitive (PrimitiveType.Sphere);
					centerObject.transform.position = initialCameraPosition;
					centerObject.GetComponent<Renderer> ().material.color = Color.red;
					centerObject.transform.localScale = new Vector3(.5f, .5f, 5f);
					centerObject.transform.position = gameCenter;
				}

				Transform trackPiece = Transform.Instantiate (trackPiecePrefab, centerObject.transform);
				trackPiece.localPosition = new Vector3 (0, y, 0);
				y += .1f;
				*/
				/*
				Vector3 currentCameraPosition = Camera.main.ScreenToWorldPoint (touch.position);

				float xChange = currentCameraPosition.x = initialCameraPosition.x;
				float yChange = currentCameraPosition.y = initialCameraPosition.y;
				float zChange = currentCameraPosition.z = initialCameraPosition.z;

				Vector3 touchPos = gameCenter; // HO-R touch.position;
				Debug.Log("touchPos="+touchPos.ToString());
				touchPos.z = gameRadius;
				Vector3 position = Camera.main.ScreenToWorldPoint (touchPos);

				position.x -= xChange;
				position.y -= yChange;
				position.z -= zChange;

				Transform trackPiece = Transform.Instantiate (trackPiecePrefab);
				trackPiece.position = position;

				Vector3 trackScale = trackPiece.localScale;
				trackPiece.localScale = new Vector3 (trackScale.x * scaleFactor, trackScale.y * scaleFactor, trackScale.z * scaleFactor);

				Transform ball = Transform.Instantiate (ballPrefab);
				ball.position = new Vector3 (position.x, position.y + .5f, position.z + .2f);

				Vector3 ballScale = ball.localScale;
				ball.localScale = new Vector3 (ballScale.x * scaleFactor, ballScale.y * scaleFactor, ballScale.z * scaleFactor);

				Debug.Log ("trackPiece.position=" + trackPiece.position.ToString () + ", touch.position=" + touch.position.ToString ());
				*/

			} else if (false && touch.phase == TouchPhase.Moved) {
				Ray ray = Camera.main.ScreenPointToRay (touch.position);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					Debug.Log (" You just hit " + hit.collider.gameObject.name);


					Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, hit.collider.transform.position.z));
					// lerp and set the position of the current object to that of the touch, but smoothly over time.
					hit.collider.transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime);
					/*


					Vector3 touchPos = touch.position;
					touchPos.z = gameRadius;// hit.transform.position.z;

					Vector3 position = Camera.main.ScreenToWorldPoint (touchPos);
					hit.transform.position = position;
					*/
				}
			}
		}
	}
}
