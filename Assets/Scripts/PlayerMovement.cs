using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : ControlledMovement
{
	public float movementSpeed = 5.0f;
	public bool freezeMovement = false;
	public float dashSpeedFactor = 4.0f;
	public float dashDuration = 0.25f;
	public bool IsDashing { get; private set; }

	private float lastDashStart = 0.0f;
	public Vector3 dashVelocity;

	private void Update()
	{
		Vector3 newVelocity = new Vector3(DynamicInput.GetAxis("Move Horizontal"), DynamicInput.GetAxis("Move Vertical"), 0.0f).normalized
						 * movementSpeed;
		if (freezeMovement)
		{
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		}
		else
		{
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			if (IsDashing)
			{
				if (Time.time - lastDashStart >= dashDuration)
				{
					IsDashing = false;
				}
				else
				{
					newVelocity = dashVelocity;
				}
			}
			else
			{
				if (DynamicInput.GetButton("Dash"))
				{
					IsDashing = true;
					lastDashStart = Time.time;
					dashVelocity = newVelocity * dashSpeedFactor;
				}
			}
		}
		GetComponent<Rigidbody2D>().velocity = newVelocity;
		IsApplyingMotion = IsDashing || !Mathf.Approximately(0.0f, newVelocity.x) || !Mathf.Approximately(0.0f, newVelocity.y);
	}
}
