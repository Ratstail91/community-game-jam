using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour {
	const float deadZone = 0.25f;
	[SerializeField]
	float distance = 0f;

	void Update() {
		float horizontalInput = GamePad.GetAxis(CAxis.LX);
		float verticalInput = GamePad.GetAxis(CAxis.LY);

		if (Mathf.Abs(horizontalInput) < deadZone) {
			horizontalInput = 0f;
		}

		if (Mathf.Abs(verticalInput) < deadZone) {
			verticalInput = 0f;
		}

		if (horizontalInput == 0f && verticalInput == 0f) {
			transform.localPosition = new Vector3(0, 0, 0);
			return;
		}

		Vector3 direction = new Vector3(horizontalInput, -verticalInput, 0);
		direction.Normalize();
		direction *= distance;

		transform.localPosition = direction;
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color(1, 0, 0);
		Gizmos.DrawLine(transform.parent.position, transform.position);
	}
}