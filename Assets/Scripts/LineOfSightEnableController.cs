using System;
using System.Collections;
using System.Collections.Generic;
using TechnoWolf.TimeManipulation;
using UnityEngine;

namespace TechnoWolf.Project1
{
	/**<summary>Simple script to disable the 2DDL light used for line of sight
	 * when no enemies are nearby.</summary>
	 */
	public class LineOfSightEnableController : RecordableMonoBehaviour
	{
		public DynamicLight2D.DynamicLight dynamicLight { get; private set; }

		private List<Collider2D> enemiesInRange = new List<Collider2D>();

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (ManipulableTime.IsApplyingRecords)
			{
				return;
			}
			Health otherHealth = collision.GetComponentInParent<Health>();
			if (otherHealth != null && GetComponent<Health>().isAlignedWithPlayer != otherHealth.isAlignedWithPlayer)
			{
				enemiesInRange.Add(collision);
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (ManipulableTime.IsApplyingRecords)
			{
				return;
			}
			enemiesInRange.Remove(collision);
		}

		private void Awake()
		{
			dynamicLight = GetComponentInChildren<DynamicLight2D.DynamicLight>(true);
		}

		protected override void FlowingUpdate()
		{
			dynamicLight.enabled = enemiesInRange.Count > 0;
		}

		public sealed class TimelineRecord_LineOfSightEnableController : TimelineRecordForBehaviour<LineOfSightEnableController>
		{
			public Collider2D[] enemiesInRange;

			protected override void RecordState(LineOfSightEnableController lose)
			{
				base.RecordState(lose);
				enemiesInRange = lose.enemiesInRange.ToArray();
			}

			protected override void ApplyRecord(LineOfSightEnableController lose)
			{
				base.ApplyRecord(lose);
				lose.enemiesInRange.Clear();
				lose.enemiesInRange.AddRange(enemiesInRange);
			}
		}
	}
}
