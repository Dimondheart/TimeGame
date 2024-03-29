﻿using System.Collections;
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

		private void OnEnable()
		{
			if (ManipulableTime.IsApplyingRecords)
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

		public sealed class TimelineRecord_FollowDefinedPath : TimelineRecord_ControlledMovement<FollowDefinedPath>
		{
			public Transform[] targets;
			public float maxSpeed;
			public int targetIndex;

			protected override void RecordState(FollowDefinedPath fdp)
			{
				base.RecordState(fdp);
				targets = (Transform[])fdp.targets.Clone();
				maxSpeed = fdp.maxSpeed;
				targetIndex = fdp.targetIndex;
			}

			protected override void ApplyRecord(FollowDefinedPath fdp)
			{
				base.ApplyRecord(fdp);
				fdp.targets = (Transform[])targets.Clone();
				fdp.maxSpeed = maxSpeed;
				fdp.targetIndex = targetIndex;
			}
		}
	}
}
