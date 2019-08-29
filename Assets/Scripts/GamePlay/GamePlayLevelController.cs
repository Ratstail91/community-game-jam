using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayLevelController : MonoBehaviour {
	void Update() {
		if (Input.GetKey("escape")) {
			Application.Quit(0);
		}
	}
}