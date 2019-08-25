using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwitchController : MonoBehaviour {
	Rigidbody2D rigidBody;

	void Awake() {
		rigidBody = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		rigidBody.rotation += 1;
	}

	void OnTriggerEnter2D(Collider2D collider) {
		Debug.Log("Trigger Collision");
	}
}