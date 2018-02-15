using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	public static Manager instance = null;

	public Transform trackPiecePrefab;

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

			Vector3 position = new Vector3 (0, 0, 1);
			Transform trackPiece = Transform.Instantiate (trackPiecePrefab);
			trackPiece.position = position;

			Debug.Log ("trackPiece.position=" + trackPiece.position.ToString());
		}
	}
}
