using System;
using System.Collections;
using System.Collections.Generic;
using TechnoWolf.TimeManipulation;
using UnityEngine;

namespace TechnoWolf.Project1
{
	/**<summary></summary>*/
	public class LineOfSightEnableController : MonoBehaviour, TimeManipulation.ITimelineRecordable
	{
		public DynamicLight2D.DynamicLight dynamicLight { get; private set; }

		private List<Collider2D> enemiesInRange = new List<Collider2D>();

		TimelineRecordForComponent ITimelineRecordable.MakeTimelineRecord()
		{
			return new TimelineRecord_LineOfSightEnableController();
		}

		void ITimelineRecordable.RecordCurrentState(TimelineRecordForComponent record)
		{
			TimelineRecord_LineOfSightEnableController rec = (TimelineRecord_LineOfSightEnableController)record;
			rec.dynamicLight = dynamicLight;
			rec.enemiesInRange = enemiesInRange.ToArray();
		}

		void ITimelineRecordable.ApplyTimelineRecord(TimelineRecordForComponent record)
		{
			TimelineRecord_LineOfSightEnableController rec = (TimelineRecord_LineOfSightEnableController)record;
			dynamicLight = rec.dynamicLight;
			enemiesInRange.Clear();
			enemiesInRange.AddRange(rec.enemiesInRange);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (TimeManipulation.ManipulableTime.IsApplyingRecords || TimeManipulation.ManipulableTime.IsTimeOrGamePaused)
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
			if (TimeManipulation.ManipulableTime.IsApplyingRecords || TimeManipulation.ManipulableTime.IsTimeOrGamePaused)
			{
				return;
			}
			enemiesInRange.Remove(collision);
		}

		private void Awake()
		{
			dynamicLight = GetComponentInChildren<DynamicLight2D.DynamicLight>(true);
		}

		private void Update()
		{
			if (TimeManipulation.ManipulableTime.IsApplyingRecords || TimeManipulation.ManipulableTime.IsTimeOrGamePaused)
			{
				return;
			}
			dynamicLight.enabled = enemiesInRange.Count > 0;
		}

		public class TimelineRecord_LineOfSightEnableController : TimeManipulation.TimelineRecordForComponent
		{
			public DynamicLight2D.DynamicLight dynamicLight;
			public Collider2D[] enemiesInRange;
		}
	}
}
