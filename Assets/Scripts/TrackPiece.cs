using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPiece: MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void Update()
	{
		if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Moved))
		{
			var touch = Input.GetTouch(0);

			Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			RaycastHit raycastHit;
			if (Physics.Raycast(raycast, out raycastHit))
			{
				if (raycastHit.transform == transform)
				{
					Debug.Log("Hit Me");

					/*
					Vector3 touchPos = gameCenter; 
					Debug.Log("touchPos="+touchPos.ToString());
					touchPos.z = Manager.instance.gameRadius;

					Vector3 position = Camera.main.ScreenToWorldPoint (touchPos);

					//Transform trackPiece = Transform.Instantiate (trackPiecePrefab);
					//trackPiece.position = position;
					transform.position = position;
					*/
					//Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, transform.position.z));
					Vector3 touchedPos = Camera.main.ScreenToWorldPoint(touch.position);
					Debug.Log ("B4 transform.position=" + transform.position.ToString ());
					Debug.Log ("touchedPos=" + touchedPos.ToString ());
					transform.position = new Vector3 (touchedPos.x, touchedPos.y, transform.position.z);
//					transform.position = touchedPos;// Vector3.Lerp(transform.position, touchedPos, Time.deltaTime);
					Debug.Log ("F2 transform.position=" + transform.position.ToString ());
				}
			}
		}
	}
}
