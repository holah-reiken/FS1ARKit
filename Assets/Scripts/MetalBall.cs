using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBall : MonoBehaviour {

	AudioSource audioSource;

	public AudioClip hitSound;
	public AudioClip rollSound;

	// Use this for initialization
	void Awake () {
		audioSource = GetComponent<AudioSource> ();
	}

	void Start() {
		audioSource.Play();
	}

	void OnCollisionEnter (Collision col) {
		Debug.Log ("col.gameObject.tag=" + col.gameObject.tag);
		Debug.Log ("col.gameObject.name=" + col.gameObject.name);
		if(col.gameObject.CompareTag("TrackParent") || col.gameObject.CompareTag("TrackPiece")) {
			StartCoroutine("Play");
		}
	}

	void OnCollisionExit(Collision col) {
		audioSource.Stop ();
	}

	private IEnumerator Play() {

		audioSource.clip = hitSound;
		audioSource.loop = false;
		Debug.Log ("play hit sound");
		audioSource.Play ();
		yield return new WaitForSeconds (.05f);
		audioSource.clip = rollSound;
		audioSource.loop = true;
		Debug.Log ("play roll sound");
		audioSource.Play ();
	}
}
