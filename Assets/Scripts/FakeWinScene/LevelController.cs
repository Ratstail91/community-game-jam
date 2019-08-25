using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {
	void Update() {
		GamePadState state = GamePad.GetState();

		bool input = false;
		const float deadZone = 0.25f;

		//any input
		input |= state.Pressed(CButton.A) || state.Pressed(CButton.B) || state.Pressed(CButton.X) || state.Pressed(CButton.Y);
		input |= state.Pressed(CButton.Back) || state.Pressed(CButton.Start);
		input |= Mathf.Abs(state.GetAxis(CAxis.LX)) > deadZone || Mathf.Abs(state.GetAxis(CAxis.LY)) > deadZone || Mathf.Abs(state.GetAxis(CAxis.RX)) > deadZone || Mathf.Abs(state.GetAxis(CAxis.RY)) > deadZone;

		if (input) {
			SceneManager.LoadScene("GamePlay");
		}
	}
}