using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBlock : MonoBehaviour {
	Rigidbody2D rigidBody;
	float angle = 0;

	void Awake() {
		rigidBody = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		angle += 1;
		rigidBody.rotation = angle;
	}

	void OnCollisionStay2D(Collision2D collision) {
		Transform playerTransform = collision.gameObject.transform;

		playerTransform.RotateAround(transform.position, Vector3.forward, 1);
	}
}