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
	public class LineOfSightEnableController : RecordableMonoBehaviour<TimelineRecord_LineOfSightEnableController>
	{
		public DynamicLight2D.DynamicLight dynamicLight { get; private set; }

		private List<Collider2D> enemiesInRange = new List<Collider2D>();

		protected override void RecordCurrentState(TimelineRecord_LineOfSightEnableController record)
		{
			record.enemiesInRange = enemiesInRange.ToArray();
		}

		protected override void ApplyRecordedState(TimelineRecord_LineOfSightEnableController record)
		{
			enemiesInRange.Clear();
			enemiesInRange.AddRange(record.enemiesInRange);
		}

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
	}

	public class TimelineRecord_LineOfSightEnableController : TimelineRecordForBehaviour
	{
		public Collider2D[] enemiesInRange;
	}
}
