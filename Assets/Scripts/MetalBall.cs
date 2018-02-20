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

	void OnCollisionEnter (Collision col) {
		if(col.gameObject.CompareTag("TrackParent")) {
			StartCoroutine(Play());
		}
	}

	void OnCollisionExit(Collision col) {
		audioSource.Stop ();
	}

	IEnumerator Play() {

		audioSource.clip = hitSound;
		audioSource.Play ();
		yield return new WaitForSeconds (1);
		audioSource.clip = rollSound;
		audioSource.Play ();
	}
}
