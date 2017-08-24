using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : ControlledMovement
{
	public float movementSpeed = 6.0f;
	public bool freezeMovement = false;
	public float dashSpeed = 15.0f;
	public float dashDuration = 0.2f;

	private ConvertableTimeRecord lastDashStart;
	private Vector3 dashVelocity;
	private bool isDashingInternal = false;
	private bool dashReleasedAfterExitingWater = true;

	public bool IsDashing
	{
		get
		{
			return isDashingInternal;
		}
		private set
		{
			isDashingInternal = value;
			GetComponent<SurfaceInteraction>().enabled = !value;
		}
	}

	private void Awake()
	{
		lastDashStart = ConvertableTimeRecord.GetTime();
	}

	private void Update()
	{
		if (ManipulableTime.IsTimeFrozen && !DynamicInput.GetButton("Freeze Move"))
		{
			if (DynamicInput.GetButton("Freeze Move"))
			{
				IsDashing = false;
			}
			else
			{
				GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
				return;
			}
		}
		Vector3 newVelocity =
			new Vector3(DynamicInput.GetAxis("Move Horizontal"), DynamicInput.GetAxis("Move Vertical"), 0.0f).normalized
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
				float currentTime = ManipulableTime.time;
				float lastDashTime = lastDashStart.manipulableTime;
				if (ManipulableTime.IsTimeFrozen)
				{
					currentTime = Time.time;
					lastDashTime = lastDashStart.unityTime;
				}
				if (currentTime - lastDashTime >= dashDuration)
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
					SurfaceInteraction sf = GetComponent<SurfaceInteraction>();
					if (sf.IsSwimming)
					{
						dashReleasedAfterExitingWater = false;
						newVelocity = newVelocity.normalized * ((movementSpeed + dashSpeed) / 2.0f);
					}
					else if (dashReleasedAfterExitingWater)
					{
						IsDashing = true;
						lastDashStart.SetToCurrent();
						dashVelocity = newVelocity.normalized * dashSpeed;
					}
				}
				else
				{
					dashReleasedAfterExitingWater = true;
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
