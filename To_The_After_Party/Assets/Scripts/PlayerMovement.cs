using UnityEngine;
using System.Collections;
using InControl;

public class PlayerMovement : MonoBehaviour {

	public float MaxSpeed = 10.0f;
	public float AccelRate = 0.75f;
	public float FrictionRate = 0.01f;
	public float StationarySpeed = 0.02f;
	public float DrunkWalkAmount = 0.01f;
	
	public KeyCode KeyUp, KeyLeft, KeyDown, KeyRight;

	private float screenx = Screen.width;
	private float screeny = Screen.height;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		float xL = 0.0f, yL = 0.0f;
		if (Input.GetKey(KeyUp))
			yL += 1.0f;
		if (Input.GetKey(KeyDown))
			yL -= 1.0f;
		if (Input.GetKey(KeyRight))
			xL += 1.0f;
		if (Input.GetKey(KeyLeft))
			xL -= 1.0f;

		float VX = rigidbody2D.velocity.x + AccelRate * xL;
		if (VX > 0) {
			VX = Mathf.Min(VX, MaxSpeed);
		}
		else {
			VX = Mathf.Max(VX, -MaxSpeed);
		}

		float VY = rigidbody2D.velocity.y + AccelRate * yL;
		if (VY > 0) {
			VY = Mathf.Min(VY, MaxSpeed);
		}
		else {
			VY = Mathf.Max(VY, -MaxSpeed);
		}

		if (Mathf.Abs(VY) > StationarySpeed) {
			if (yL == 0.0f)
				VY = Mathf.Max (0, (Mathf.Abs(VY) - FrictionRate)) * Mathf.Sign(VY);
			VX += DrunkWalkAmount * Mathf.Cos(Time.time);
		}

		if (Mathf.Abs(VX) > StationarySpeed) {
			if (xL == 0.0f)
				VX = Mathf.Max (0, (Mathf.Abs(VX) - FrictionRate)) * Mathf.Sign(VX);
			VY += DrunkWalkAmount * Mathf.Sin(Time.time);
		}

		rigidbody2D.velocity = new Vector2(VX, VY);

		//RestrictMovement();
	}

	void RestrictMovement() {
		Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);

		if(screenPos.x <= 0) {
			screenPos.x = 0;
		}

		if(screenPos.x >= 1) {
			screenPos.x = 1;
		}

		if(screenPos.y <= 0) {
			screenPos.y = 0;
		}
		
		if(screenPos.y >= 1) {
			screenPos.y = 1;
		}

		transform.position = Camera.main.ViewportToWorldPoint(screenPos);
	}
}
