using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;
using System;

namespace TechnoWolf.Project1
{
	/**<summary>Move towards a target.</summary>*/
	public class FollowTarget : ControlledMovement<TimelineRecord_FollowTarget>
	{
		public float maxSpeed = 4.0f;
		public float velocityBlendRate = 100.0f;
		public bool targetLocationReached { get; private set; }

		public Transform TargetTransform
		{
			get
			{
				if (GetComponent<HostileTargetSelector>() == null
					|| GetComponent<HostileTargetSelector>().target == null
					)
				{
					return null;
				}
				return GetComponent<HostileTargetSelector>().target.transform;
			}
		}

		public Vector3 TargetPositon
		{
			get
			{
				if (TargetTransform == null)
				{
					if (GetComponent<HostileTargetSelector>() == null)
					{
						return new Vector3(float.MaxValue, 0.0f, 0.0f);
					}
					return GetComponent<HostileTargetSelector>().targetLastSpotted;
				}
				return TargetTransform.position;
			}
		}

		protected override void RecordCurrentState(TimelineRecord_FollowTarget record)
		{
			base.RecordCurrentState(record);
			record.maxSpeed = maxSpeed;
			record.velocityBlendRate = velocityBlendRate;
			record.targetLocationReached = targetLocationReached;
		}

		protected override void ApplyRecordedState(TimelineRecord_FollowTarget record)
		{
			base.ApplyRecordedState(record);
			maxSpeed = record.maxSpeed;
			velocityBlendRate = record.velocityBlendRate;
			targetLocationReached = record.targetLocationReached;
		}

		private void Start()
		{
			targetLocationReached = true;
		}

		protected override void FirstPausedUpdate()
		{
			Debug.Log("hello from here");
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		}

		protected override void FirstResumedUpdate()
		{
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		}

		protected override void FlowingUpdate()
		{
			if ((TargetTransform == null && targetLocationReached)
				|| !GetComponent<Health>().IsAlive
				|| Vector3.Distance(TargetPositon, transform.position) < 0.3f
				)
			{
				IsApplyingMotion = false;
				targetLocationReached = true;
				if (GetComponent<FollowDefinedPath>() != null)
				{
					GetComponent<FollowDefinedPath>().enabled = true;
				}
				return;
			}
			if (GetComponent<FollowDefinedPath>() != null)
			{
				GetComponent<FollowDefinedPath>().enabled = false;
			}
			targetLocationReached = false;
			Vector2 newVelocity =
					Vector3.ClampMagnitude(
						(TargetPositon - transform.position).normalized * maxSpeed,
						maxSpeed
						);
			float blendFactor = velocityBlendRate * ManipulableTime.deltaTime;
			for (int c = 0; c < 2; c++)
			{
				newVelocity[c] = newVelocity[c] * blendFactor + GetComponent<Rigidbody2D>().velocity[c] * (1.0f - blendFactor);
			}
			newVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
			GetComponent<Rigidbody2D>().velocity = newVelocity;
			IsApplyingMotion = true;
			GetComponent<DirectionLooking>().Direction = newVelocity;
		}
	}

	public class TimelineRecord_FollowTarget : TimelineRecord_ControlledMovement
	{
		public float maxSpeed;
		public float velocityBlendRate;
		public bool targetLocationReached;
	}
}
