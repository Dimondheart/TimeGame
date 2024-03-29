﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Selects a target for hostile actions.</summary>*/
	public class HostileTargetSelector : RecordableMonoBehaviour
	{
		public GameObject target { get; private set; }
		public List<GameObject> hostilesInLineOfSight = new List<GameObject>();
		public List<GameObject> otherHostilesDetected = new List<GameObject>();
		public Vector3 targetLastSpotted { get; private set; }

		public void OnLineOfSightEnter(GameObject entered)
		{
			if (ManipulableTime.IsApplyingRecords)
			{
				return;
			}
			if (!hostilesInLineOfSight.Contains(entered) && IsHostile(entered))
			{
				hostilesInLineOfSight.Add(entered);
			}
		}

		public void OnLineOfSightExit(GameObject exited)
		{
			if (ManipulableTime.IsApplyingRecords)
			{
				return;
			}
			hostilesInLineOfSight.Remove(exited);
		}

		public void OnLineOfSightPersist(GameObject seen)
		{
			if (ManipulableTime.IsApplyingRecords)
			{
				return;
			}
			if (IsHostile(seen))
			{
				if (!hostilesInLineOfSight.Contains(seen))
				{
					hostilesInLineOfSight.Add(seen);
				}
			}
			else
			{
				hostilesInLineOfSight.Remove(seen);
			}
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (ManipulableTime.IsApplyingRecords)
			{
				return;
			}
			if (!otherHostilesDetected.Contains(collision.gameObject) && IsHostile(collision.gameObject))
			{
				otherHostilesDetected.Add(collision.gameObject);
			}
		}

		private void OnCollisionExit2D(Collision2D collision)
		{
			if (ManipulableTime.IsApplyingRecords)
			{
				return;
			}
			otherHostilesDetected.Remove(collision.gameObject);
		}

		private void Start()
		{
			targetLastSpotted = transform.position;
		}

		protected override void FlowingUpdate()
		{
			if (!hostilesInLineOfSight.Contains(target))
			{
				target = null;
			}
			if (target != null)
			{
				targetLastSpotted = target.transform.position;
			}
			target = ClosestDetectedHostile();
		}

		/**<summary>Checks if this thing considers the specified other thing
		 * hostile.</summary>
		 */
		public bool IsHostile(GameObject otherThing)
		{
			return otherThing.GetComponent<Health>() != null
				&& otherThing.GetComponent<Health>().isAlignedWithPlayer != GetComponent<Health>().isAlignedWithPlayer;
		}

		public GameObject ClosestDetectedHostile()
		{
			if (hostilesInLineOfSight.Count <= 0 && otherHostilesDetected.Count <= 0)
			{
				return null;
			}
			GameObject closest = null;
			float closestDist = float.PositiveInfinity;
			float dist;
			foreach (GameObject go in hostilesInLineOfSight)
			{
				dist = Vector3.Distance(go.transform.position, transform.position);
				if (go.GetComponent<Health>().IsAlive && dist < closestDist)
				{
					closestDist = dist;
					closest = go;
				}
			}
			foreach (GameObject go in otherHostilesDetected)
			{
				dist = Vector3.Distance(go.transform.position, transform.position);
				if (go.GetComponent<Health>().IsAlive && dist < closestDist)
				{
					closestDist = dist;
					closest = go;
				}
			}
			return closest;
		}

		public sealed class TimelineRecord_HostileTargetSelector : TimelineRecordForBehaviour<HostileTargetSelector>
		{
			public GameObject target;
			public GameObject[] hostilesInLineOfSight;
			public GameObject[] otherHostilesDetected;
			public Vector3 targetLastSpotted;

			protected override void RecordState(HostileTargetSelector tsel)
			{
				base.RecordState(tsel);
				target = tsel.target;
				hostilesInLineOfSight = tsel.hostilesInLineOfSight.ToArray();
				otherHostilesDetected = tsel.otherHostilesDetected.ToArray();
				targetLastSpotted = tsel.targetLastSpotted;
			}

			protected override void ApplyRecord(HostileTargetSelector tsel)
			{
				base.ApplyRecord(tsel);
				tsel.target = target;
				tsel.hostilesInLineOfSight.Clear();
				tsel.hostilesInLineOfSight.AddRange(hostilesInLineOfSight);
				tsel.otherHostilesDetected.Clear();
				tsel.otherHostilesDetected.AddRange(otherHostilesDetected);
				tsel.targetLastSpotted = targetLastSpotted;
			}
		}
	}
}
