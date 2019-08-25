using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : MonoBehaviour {
	static GameObject player = null;

	void Awake() {
		if (player == null) {
			player = GameObject.Find("Player");
		}
	}

	void LateUpdate() {
		if (Vector3.Distance(transform.position, player.transform.position) < 100) {
			player.GetComponent<PlayerController>().SetGravityTowards(transform.position);
			player.GetComponent<PlayerController>().SetFloating(true);
		}
	}
}