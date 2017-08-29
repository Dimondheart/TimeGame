using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : ControlledMovement
{
	public float movementSpeed = 6.0f;
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

	public override TimelineRecord MakeTimelineRecord()
	{
		TimelineRecord_PlayerMovement record = new TimelineRecord_PlayerMovement();
		AddTimelineRecordValues(record);
		record.movementSpeed = movementSpeed;
		record.dashSpeed = dashSpeed;
		record.dashDuration = dashDuration;

		record.lastDashStart = lastDashStart;
		record.dashVelocity = dashVelocity;
		record.isDashingInternal = isDashingInternal;
		record.dashReleasedAfterExitingWater = dashReleasedAfterExitingWater;
		return record;
}

	public override void ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_PlayerMovement rec = (TimelineRecord_PlayerMovement)record;
		ApplyTimelineRecordValues(rec);
		movementSpeed = rec.movementSpeed;
		dashSpeed = rec.dashSpeed;
		dashDuration = rec.dashDuration;

		lastDashStart = rec.lastDashStart;
		dashVelocity = rec.dashVelocity;
		isDashingInternal = rec.isDashingInternal;
		dashReleasedAfterExitingWater = rec.dashReleasedAfterExitingWater;
	}

	private void Awake()
	{
		lastDashStart = ConvertableTimeRecord.GetTime();
	}

	private void Update()
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		if (GetComponent<Health>().health <= 0)
		{
			IsApplyingMotion = false;
			return;
		}
		if (ManipulableTime.IsTimeFrozen)
		{
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
			return;
		}
		Vector3 newVelocity =
			new Vector3(DynamicInput.GetAxis("Move Horizontal"), DynamicInput.GetAxis("Move Vertical"), 0.0f).normalized
			* movementSpeed;
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
		GetComponent<Rigidbody2D>().velocity = newVelocity;
		IsApplyingMotion = IsDashing || !Mathf.Approximately(0.0f, newVelocity.x) || !Mathf.Approximately(0.0f, newVelocity.y);
		Vector2 lookDirection = new Vector2(DynamicInput.GetAxis("Look Horizontal"), DynamicInput.GetAxis("Look Vertical"));
		if (Mathf.Approximately(0.0f, lookDirection.magnitude))
		{
			lookDirection = newVelocity;
		}
		GetComponent<DirectionLooking>().Direction = lookDirection;
	}

	public class TimelineRecord_PlayerMovement : ControlledMovement.TimelineRecord_ControlledMovement
	{
		public float movementSpeed;
		public float dashSpeed;
		public float dashDuration;

		public ConvertableTimeRecord lastDashStart;
		public Vector3 dashVelocity;
		public bool isDashingInternal;
		public bool dashReleasedAfterExitingWater;
	}
}
