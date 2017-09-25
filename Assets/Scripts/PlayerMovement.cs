using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.DynamicInputSystem;
using TechnoWolf.TimeManipulation;
using System;

namespace TechnoWolf.Project1
{
	/**<summary>Movement controlled by the player.</summary>*/
	public class PlayerMovement : ControlledMovement
	{
		public float movementSpeed = 6.0f;
		public float dashSpeed = 15.0f;
		public float dashDuration = 0.2f;
		private bool stopApplying = false;

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

		public bool IsTryingToMove
		{
			get
			{
				return !Mathf.Approximately(0.0f, DynamicInput.GetAxis("Move Horizontal")) || !Mathf.Approximately(0.0f, DynamicInput.GetAxis("Move Vertical"));
			}
		}

		public override TimelineRecordForComponent MakeTimelineRecord()
		{
			return new TimelineRecord_PlayerMovement();
		}

		public override void RecordCurrentState(TimelineRecordForComponent record)
		{
			TimelineRecord_PlayerMovement rec = (TimelineRecord_PlayerMovement)record;
			AddTimelineRecordValues(rec);
			rec.movementSpeed = movementSpeed;
			rec.dashSpeed = dashSpeed;
			rec.dashDuration = dashDuration;
			rec.stopApplying = stopApplying;

			rec.lastDashStart = lastDashStart;
			rec.dashVelocity = dashVelocity;
			rec.isDashingInternal = isDashingInternal;
			rec.dashReleasedAfterExitingWater = dashReleasedAfterExitingWater;
		}

		public override void ApplyTimelineRecord(TimelineRecordForComponent record)
		{
			TimelineRecord_PlayerMovement rec = (TimelineRecord_PlayerMovement)record;
			ApplyTimelineRecordValues(rec);
			movementSpeed = rec.movementSpeed;
			dashSpeed = rec.dashSpeed;
			dashDuration = rec.dashDuration;
			stopApplying = rec.stopApplying;

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
			if (ManipulableTime.IsApplyingRecords)
			{
				return;
			}
			if (ManipulableTime.IsTimeOrGamePaused)
			{
				GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
				return;
			}
			if (!GetComponent<Health>().IsAlive)
			{
				IsApplyingMotion = false;
				return;
			}
			Vector3 newVelocity =
				Vector3.ClampMagnitude(
					new Vector3(DynamicInput.GetAxisRaw("Move Horizontal"), DynamicInput.GetAxisRaw("Move Vertical"), 0.0f),
					1.0f
					)
				* movementSpeed;
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			if (IsDashing)
			{
				float currentTime = ManipulableTime.time;
				float lastDashTime = lastDashStart.manipulableTime;
				if (ManipulableTime.IsTimeOrGamePaused)
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
				SurfaceInteraction sf = GetComponent<SurfaceInteraction>();
				if (DynamicInput.GetButtonDown("Dash") || (DynamicInput.GetButtonHeld("Dash") && sf.IsSwimming))
				{
					GetComponent<PlayerMelee>().StopSwinging();
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
					if (GetComponent<PlayerMelee>().IsSwinging || GetComponent<PlayerGuard>().IsGuarding)
					{
						newVelocity = newVelocity * 0.1f;
					}
				}
			}
			GetComponent<Rigidbody2D>().velocity = newVelocity;
			IsApplyingMotion = IsDashing || !Mathf.Approximately(0.0f, newVelocity.x) || !Mathf.Approximately(0.0f, newVelocity.y);
			bool isSwimming = GetComponent<SurfaceInteraction>().IsSwimming;
			if ((GetComponent<PlayerMelee>().IsInCooldown && !IsDashing) || isSwimming)
			{
				Vector2 lookDirection = new Vector2(DynamicInput.GetAxisRaw("Look Horizontal"), DynamicInput.GetAxisRaw("Look Vertical"));
				if (Mathf.Approximately(0.0f, lookDirection.magnitude)
					|| (isSwimming && !(DynamicInput.GamepadModeEnabled || DynamicInput.GetButtonHeld("Melee") || DynamicInput.GetButtonHeld("Guard")))
					)
				{
					lookDirection = newVelocity;
				}
				GetComponent<DirectionLooking>().Direction = lookDirection;
			}
		}

		public class TimelineRecord_PlayerMovement : ControlledMovement.TimelineRecord_ControlledMovement
		{
			public float movementSpeed;
			public float dashSpeed;
			public float dashDuration;
			public bool stopApplying;

			public ConvertableTimeRecord lastDashStart;
			public Vector3 dashVelocity;
			public bool isDashingInternal;
			public bool dashReleasedAfterExitingWater;
		}
	}
}
