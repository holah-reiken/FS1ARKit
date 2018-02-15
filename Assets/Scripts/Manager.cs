using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	public static Manager instance = null;

	public Transform trackPiecePrefab;
	public Transform ballPrefab;

	public float scaleFactor = .5f;

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

				//Vector3 position = new Vector3 (0, 0, 1);

				Vector3 touchPos = touch.position;
				touchPos.z = 2;
				Vector3 position = Camera.main.ScreenToWorldPoint(touchPos);

				Transform trackPiece = Transform.Instantiate (trackPiecePrefab);
				trackPiece.position = position;

				Vector3 trackScale = trackPiece.localScale;
				trackPiece.localScale = new Vector3 (trackScale.x * scaleFactor, trackScale.y * scaleFactor, trackScale.z * scaleFactor);
				Debug.Log ("trackScale="+trackScale.ToString()+", trackPiece.localScale=" + trackPiece.localScale.ToString ());

				Transform ball = Transform.Instantiate (ballPrefab);
				ball.position = new Vector3 (position.x, position.y + 1f, position.z);

				Vector3 ballScale = ball.localScale;
				ball.localScale = new Vector3 (ballScale.x * scaleFactor, ballScale.y * scaleFactor, ballScale.z * scaleFactor);
				Debug.Log ("ballScale="+ballScale.ToString()+", ball.localScale=" + ball.localScale.ToString ());

				Debug.Log ("trackPiece.position=" + trackPiece.position.ToString () + ", touch.position=" + touch.position.ToString ());

			}
		}
	}
}
