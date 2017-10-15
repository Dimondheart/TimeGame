using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.DynamicInputSystem;
using TechnoWolf.TimeManipulation;
using System;

namespace TechnoWolf.Project1
{
	/**<summary>Movement controlled by the player.</summary>*/
	public class PlayerMovement : RecordableMonoBehaviour
	{
		public PhysicsMaterial2D stationaryMaterial;
		public PhysicsMaterial2D applyingMotionMaterial;
		public float movementSpeed = 6.0f;
		public float dashSpeed = 15.0f;
		public float dashDuration = 0.2f;
		private bool stopApplying = false;

		private ConvertableTime lastDashStart;
		private Vector3 dashVelocity;
		private bool isDashingInternal = false;
		private bool dashReleasedAfterExitingWater = true;
		private bool isApplyingMotion = false;

		public bool IsApplyingMotion
		{
			get
			{
				return isApplyingMotion;
			}
			protected set
			{
				isApplyingMotion = value;
				if (value)
				{
					GetComponent<Rigidbody2D>().sharedMaterial = applyingMotionMaterial;
				}
				else
				{
					GetComponent<Rigidbody2D>().sharedMaterial = stationaryMaterial;
				}
			}
		}

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

		private void Awake()
		{
			lastDashStart = ConvertableTime.GetTime();
		}

		public override void Update()
		{
			if (ManipulableTime.IsTimeOrGamePaused)
			{
				GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
				return;
			}
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
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
			IsApplyingMotion =
				IsDashing
				|| !Mathf.Approximately(0.0f, newVelocity.x)
				|| !Mathf.Approximately(0.0f, newVelocity.y);
		}

		public sealed class TimelineRecord_PlayerMovement : TimelineRecordForBehaviour<PlayerMovement>
		{
			public bool isApplyingMotion;
			public float movementSpeed;
			public float dashSpeed;
			public float dashDuration;
			public bool stopApplying;

			public ConvertableTime lastDashStart;
			public Vector3 dashVelocity;
			public bool isDashingInternal;
			public bool dashReleasedAfterExitingWater;

			protected override void WriteCurrentState(PlayerMovement pm)
			{
				base.WriteCurrentState(pm);
				isApplyingMotion = pm.isApplyingMotion;
				movementSpeed = pm.movementSpeed;
				dashSpeed = pm.dashSpeed;
				dashDuration = pm.dashDuration;
				stopApplying = pm.stopApplying;

				lastDashStart = pm.lastDashStart;
				dashVelocity = pm.dashVelocity;
				isDashingInternal = pm.isDashingInternal;
				dashReleasedAfterExitingWater = pm.dashReleasedAfterExitingWater;
			}

			protected override void ApplyRecordedState(PlayerMovement pm)
			{
				base.ApplyRecordedState(pm);
				pm.isApplyingMotion = isApplyingMotion;
				pm.movementSpeed = movementSpeed;
				pm.dashSpeed = dashSpeed;
				pm.dashDuration = dashDuration;
				pm.stopApplying = stopApplying;

				pm.lastDashStart = lastDashStart;
				pm.dashVelocity = dashVelocity;
				pm.isDashingInternal = isDashingInternal;
				pm.dashReleasedAfterExitingWater = dashReleasedAfterExitingWater;
			}
		}
	}
}
