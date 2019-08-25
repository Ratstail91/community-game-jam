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
	float jumpForce = 100f;
	float maxSpeed = 80f;
	float maxFallSpeed = 400f;
	Vector2 gravity = new Vector2(0, -200f);
	Vector2 defaultGravity = new Vector2(0, -200f);
	bool floating = false;

	//grounded
	bool grounded = false;
	Vector3 center;
	Vector3 baseC;
	Vector3 baseL;
	Vector3 baseR;
	float baseAngle;
	Vector3 baseRotation;

	//methods
	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		rigidBody = GetComponent<Rigidbody2D>();
		boxCollider = GetComponent<BoxCollider2D>();
	}

	void Update() {
		HandleInput();

		gravity = defaultGravity;
		floating = false;
	}

	void FixedUpdate() {
		HandleMovement();
	}

	void HandleInput() {
		//determine if on the ground (using coyote time)
		center = new Vector3(boxCollider.offset.x, boxCollider.offset.y, 0);
		baseC = new Vector3(transform.localScale.x * (boxCollider.offset.x), -boxCollider.size.y / 1.85f, 0);
		baseL = new Vector3(transform.localScale.x * (boxCollider.offset.x - boxCollider.size.x / 2.1f), -boxCollider.size.y / 1.85f, 0);
		baseR = new Vector3(transform.localScale.x * (boxCollider.offset.x + boxCollider.size.x / 2.1f), -boxCollider.size.y / 1.85f, 0);

		baseAngle = Vector2.SignedAngle(Vector2.down, gravity);
		baseRotation = new Vector3(0, 0, baseAngle);
		rigidBody.rotation = baseAngle;

		baseC = Quaternion.Euler(baseRotation) * baseC;
		baseL = Quaternion.Euler(baseRotation) * baseL;
		baseR = Quaternion.Euler(baseRotation) * baseR;

		bool trueGrounded;
		trueGrounded  = Physics2D.Linecast(transform.position + center, transform.position + baseC, 1 << LayerMask.NameToLayer("Ground"));
		trueGrounded |= Physics2D.Linecast(transform.position + center, transform.position + baseL, 1 << LayerMask.NameToLayer("Ground"));
		trueGrounded |= Physics2D.Linecast(transform.position + center, transform.position + baseR, 1 << LayerMask.NameToLayer("Ground"));

		if (trueGrounded) {
			StartCoroutine(SetGroundedWithDelay(true, 0.1f));
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

		if (verticalInput * rigidBody.velocity.y <= 0 && grounded) {
			rigidBody.velocity = new Vector2 (rigidBody.velocity.x, rigidBody.velocity.y * 0.85f);
		}

		//move in the inputted direction, if not at max speed
		if (horizontalInput * rigidBody.velocity.x < maxSpeed) {
			rigidBody.AddForce(Vector2.right * horizontalInput * moveForce);
		}

		if (-verticalInput * rigidBody.velocity.y < maxSpeed && floating) {
			rigidBody.AddForce(Vector2.up * -verticalInput * moveForce);
		}

		//slow the player down when it's falling too fast
		if (Mathf.Abs(rigidBody.velocity.x) > maxFallSpeed) {
			rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * maxFallSpeed, rigidBody.velocity.y);
		}

		if (Mathf.Abs(rigidBody.velocity.y) > maxFallSpeed) {
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Sign(rigidBody.velocity.y) * maxFallSpeed);
		}

		//jump up
		if (jumping) {
			rigidBody.AddForce(-gravity * jumpForce);
			jumping = false;
		}

		//apply gravity
		if (!grounded && rigidBody.velocity.magnitude < maxFallSpeed) {
			rigidBody.AddForce(gravity);
		}
	}

	public void SetGravityTowards(Vector3 position) {
		Vector2 pos = new Vector2(position.x - transform.position.x, position.y - transform.position.y);

		pos.Normalize();
		gravity = pos * gravity.magnitude;
	}

	public void SetDefaultGravity(Vector2 dir) {
		gravity = dir;
		defaultGravity = dir;
	}

	public Vector2 GetDefaultGravity() {
		return defaultGravity;
	}

	public void SetFloating(bool b) {
		floating = b;
	}

	//utilities
	IEnumerator SetGroundedWithDelay(bool value, float delay) {
		yield return new WaitForSeconds(delay);
		grounded = value;
	}

	void OnDrawGizmos() {
		if (boxCollider != null) {
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position + center, transform.position + baseC);
			Gizmos.DrawLine(transform.position + center, transform.position + baseL);
			Gizmos.DrawLine(transform.position + center, transform.position + baseR);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(transform.position + center, transform.position + new Vector3(gravity.x, gravity.y, 0));
		}
	}
}