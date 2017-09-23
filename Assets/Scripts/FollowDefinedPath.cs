using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;
using System;

namespace TechnoWolf.Project1
{
	/**<summary>Follow a path using a predefined set of points, which
	 * loop back to the first point.</summary>
	 */
	public class FollowDefinedPath : ControlledMovement
	{
		public Transform[] targets;
		public float maxSpeed = 4.0f;
		public int targetIndex = 0;

		public override TimelineRecordForComponent MakeTimelineRecord()
		{
			return new TimelineRecord_FollowDefinedPath();
		}

		public override void RecordCurrentState(TimelineRecordForComponent record)
		{
			TimelineRecord_FollowDefinedPath rec = (TimelineRecord_FollowDefinedPath)record;
			AddTimelineRecordValues(rec);
			rec.targets = (Transform[])targets.Clone();
			rec.maxSpeed = maxSpeed;
			rec.targetIndex = targetIndex;
		}

		public override void ApplyTimelineRecord(TimelineRecordForComponent record)
		{
			TimelineRecord_FollowDefinedPath rec = (TimelineRecord_FollowDefinedPath)record;
			ApplyTimelineRecordValues(rec);
			targets = (Transform[])rec.targets.Clone();
			maxSpeed = rec.maxSpeed;
			targetIndex = rec.targetIndex;
		}

		private void OnEnable()
		{
			if (ManipulableTime.ApplyingTimelineRecords)
			{
				return;
			}
			float shortestDist = float.PositiveInfinity;
			float dist;
			// Find the nearest target
			for (int i = 0; i < targets.Length; i++)
			{
				dist = Vector3.Distance(transform.position, targets[i].position);
				if (dist < shortestDist)
				{
					shortestDist = dist;
					targetIndex = i;
				}
			}
		}

		private void Update()
		{
			if (ManipulableTime.ApplyingTimelineRecords)
			{
				return;
			}
			if (ManipulableTime.IsTimeFrozen)
			{
				GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
				return;
			}
			else
			{
				GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			}
			if (!GetComponent<Health>().IsAlive)
			{
				IsApplyingMotion = false;
				return;
			}
			if (targetIndex >= targets.Length)
			{
				targetIndex = 0;
			}
			if (Vector3.Distance(transform.position, targets[targetIndex].position) <= 0.25f)
			{
				targetIndex++;
				if (targetIndex >= targets.Length)
				{
					targetIndex = 0;
				}
			}
			Vector3 newVelocity =
					Vector3.ClampMagnitude(
						(targets[targetIndex].position - transform.position).normalized * maxSpeed,
						maxSpeed
						);
			GetComponent<Rigidbody2D>().velocity = newVelocity;
			IsApplyingMotion = true;
			GetComponent<DirectionLooking>().Direction = newVelocity;
		}

		public class TimelineRecord_FollowDefinedPath : ControlledMovement.TimelineRecord_ControlledMovement
		{
			public Transform[] targets;
			public float maxSpeed;
			public int targetIndex;
		}
	}
}
