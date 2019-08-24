using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	[SerializeField]
	GameObject target;
	[SerializeField]
	float lerpSpeed = 2f;

	Vector3 virtualLocation;

	void Start() {
		virtualLocation = transform.position;
	}

	void Update() {
		//only continue if the target has been set
		if (target == null) {
			return;
		}

		//cache the position we want to move to
		Vector3 targetPosition = target.transform.position;

		//If the distance is small, short circuit the lerp, so we don't have sudden pops in camera motion
		if ((targetPosition - virtualLocation).sqrMagnitude > 0.01f) {
			//Interpolate to the target location
			virtualLocation = Vector3.Lerp(virtualLocation, targetPosition, lerpSpeed * Time.deltaTime);

			//Snap to pixel coordinates
			Vector3 snapped = virtualLocation;

			snapped.x = Mathf.Round(snapped.x * 100) / 100;
			snapped.y = Mathf.Round(snapped.y * 100) / 100;

			snapped.z = transform.position.z; //BUGFIX

			transform.position = snapped;
		}
	}
}