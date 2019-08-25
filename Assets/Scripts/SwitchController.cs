using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour {
	static GameObject player = null;

	public Vector2 newGravity;

	void Awake() {
		if (player == null) {
			player = GameObject.Find("Player");
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (player.GetComponent<PlayerController>().GetDefaultGravity() != newGravity) {
			player.GetComponent<PlayerController>().SetDefaultGravity(newGravity);
		}
	}
}