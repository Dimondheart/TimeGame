using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : ControlledMovement
{
	public float movementSpeed = 6.0f;
	public bool freezeMovement = false;
	public float dashSpeed = 15.0f;
	public float dashDuration = 0.2f;

	private float lastDashStart = 0.0f;
	private Vector3 dashVelocity;
	private bool isDashingInternal = false;

	public bool IsDashing
	{
		get
		{
			return isDashingInternal;
		}
		private set
		{
			isDashingInternal = value;
			GetComponent<SurfaceFriction>().enabled = !value;
		}
	}

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
					SurfaceFriction sf = GetComponent<SurfaceFriction>();
					if (sf.IsSwimming)
					{
						newVelocity = newVelocity.normalized * ((movementSpeed + dashSpeed) / 2.0f);
					}
					else
					{
						IsDashing = true;
						lastDashStart = Time.time;
						dashVelocity = newVelocity.normalized * dashSpeed;
					}
				}
			}
		}
		GetComponent<Rigidbody2D>().velocity = newVelocity;
		IsApplyingMotion = IsDashing || !Mathf.Approximately(0.0f, newVelocity.x) || !Mathf.Approximately(0.0f, newVelocity.y);
		Vector2 lookDirection = new Vector2(DynamicInput.GetAxis("Look Horizontal"), DynamicInput.GetAxis("Look Vertical"));
		if (lookDirection.magnitude < 0.01f)
		{
			lookDirection = newVelocity;
		}
		GetComponent<DirectionLooking>().Direction = lookDirection;
	}
}
