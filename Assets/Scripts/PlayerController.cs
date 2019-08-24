using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	//components
	SpriteRenderer spriteRenderer;
	Rigidbody2D rigidBody;
	BoxCollider2D boxCollider;

	//input
	float horizontalInput = 0f;
	float verticalInput = 0f;
	bool jumping = false;
	const float deadZone = 0.25f;

	//movement
	float moveForce = 400f;
	float jumpForce = 8000f;
	float gravity = 200f;
	float maxSpeed = 80f;
	float maxFallSpeed = 200f;

	bool grounded = false;
	const float groundedProjection = 16.5f;

	//methods
	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		rigidBody = GetComponent<Rigidbody2D>();
		boxCollider = GetComponent<BoxCollider2D>();
	}

	void Update() {
		HandleInput();
	}

	void FixedUpdate() {
		HandleMovement();
	}

	void HandleInput() {
		//determine if on the ground (using coyote time)
		bool trueGrounded;
		trueGrounded  = Physics2D.Linecast(transform.position + new Vector3(boxCollider.offset.x, boxCollider.offset.y, 0), transform.position + new Vector3(transform.localScale.x * (boxCollider.offset.x), -groundedProjection, 0), 1 << LayerMask.NameToLayer("Ground"));
		trueGrounded |= Physics2D.Linecast(transform.position + new Vector3(boxCollider.offset.x, boxCollider.offset.y, 0), transform.position + new Vector3(transform.localScale.x * (boxCollider.offset.x + boxCollider.size.x / 2.1f), -groundedProjection, 0), 1 << LayerMask.NameToLayer("Ground"));
		trueGrounded |= Physics2D.Linecast(transform.position + new Vector3(boxCollider.offset.x, boxCollider.offset.y, 0), transform.position + new Vector3(transform.localScale.x * (boxCollider.offset.x - boxCollider.size.x / 2.1f), -groundedProjection, 0), 1 << LayerMask.NameToLayer("Ground"));

		if (trueGrounded) {
			grounded = true;
		} else {
			StartCoroutine(SetGroundedWithDelay(false, 0.1f)); //coyote physics: 100ms
		}

		//get input
		horizontalInput = GamePad.GetAxis(CAxis.LX);
		verticalInput = GamePad.GetAxis(CAxis.LY);

		if (Mathf.Abs(horizontalInput) < deadZone) {
			horizontalInput = 0f;
		}

		if (Mathf.Abs(verticalInput) < deadZone) {
			verticalInput = 0f;
		}

		//determine if jumping
		if (GamePad.GetState().Pressed(CButton.A) && grounded) {
			jumping = true;
		}
	}

	void HandleMovement() {
		//stop the player if input in that direction has been removed
		if (horizontalInput * rigidBody.velocity.x <= 0 && grounded) {
			rigidBody.velocity = new Vector2 (rigidBody.velocity.x * 0.85f, rigidBody.velocity.y);
		}

		//move in the inputted direction, if not at max speed
		if (horizontalInput * rigidBody.velocity.x < maxSpeed) {
			rigidBody.AddForce (Vector2.right * horizontalInput * moveForce);
		}

		//slow the player down when it's travelling too fast
		if (Mathf.Abs(rigidBody.velocity.x) > maxSpeed) {
			rigidBody.velocity = new Vector2 (Mathf.Sign(rigidBody.velocity.x) * maxSpeed, rigidBody.velocity.y);
		}

		if (!grounded && rigidBody.velocity.y > -maxFallSpeed) {
			rigidBody.AddForce(Vector2.up * -gravity);
		}

		//jump up
		if (jumping) {
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f); //max v-jump speed
			rigidBody.AddForce (new Vector2(0f, jumpForce));
			jumping = false;
		}
	}

	IEnumerator SetGroundedWithDelay(bool value, float delay) {
		yield return new WaitForSeconds(delay);
		grounded = value;
	}

	//utilities
	void OnDrawGizmos() {
		if (boxCollider != null) {
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position + new Vector3(boxCollider.offset.x, boxCollider.offset.y, 0), transform.position + new Vector3(transform.localScale.x * (boxCollider.offset.x), -groundedProjection, 0));
			Gizmos.DrawLine(transform.position + new Vector3(boxCollider.offset.x, boxCollider.offset.y, 0), transform.position + new Vector3(transform.localScale.x * (boxCollider.offset.x + boxCollider.size.x / 2.1f), -groundedProjection, 0));
			Gizmos.DrawLine(transform.position + new Vector3(boxCollider.offset.x, boxCollider.offset.y, 0), transform.position + new Vector3(transform.localScale.x * (boxCollider.offset.x - boxCollider.size.x / 2.1f), -groundedProjection, 0));
		}
	}
}