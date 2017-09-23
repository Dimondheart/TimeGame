using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Look towards a specified transform.</summary>*/
	public class LookTowardsTransform : MonoBehaviour, ITimelineRecordable
	{
		/**<summary>The transform to look towards.</summary>*/
		public Transform lookTowards;
		/**<summary>Max distance to look towards the transform.</summary>*/
		public float maxDistance = 6.0f;

		TimelineRecordForComponent ITimelineRecordable.MakeTimelineRecord()
		{
			return new TimelineRecord_LookTowardsTransform();
		}

		void ITimelineRecordable.RecordCurrentState(TimelineRecordForComponent record)
		{
			TimelineRecord_LookTowardsTransform rec = (TimelineRecord_LookTowardsTransform)record;
			rec.lookTowards = lookTowards;
			rec.maxDistance = maxDistance;
		}

		void ITimelineRecordable.ApplyTimelineRecord(TimelineRecordForComponent record)
		{
			TimelineRecord_LookTowardsTransform rec = (TimelineRecord_LookTowardsTransform)record;
			lookTowards = rec.lookTowards;
			maxDistance = rec.maxDistance;
		}

		private void Update()
		{
			if (ManipulableTime.ApplyingTimelineRecords || ManipulableTime.IsTimeFrozen || !GetComponent<Health>().IsAlive)
			{
				return;
			}
			Vector2 lookVector = lookTowards.position - transform.position;
			if (lookVector.magnitude <= maxDistance)
			{
				GetComponent<DirectionLooking>().Direction = lookVector;
			}
		}

		public class TimelineRecord_LookTowardsTransform : TimelineRecordForComponent
		{
			public Transform lookTowards;
			public float maxDistance;
		}
	}
}
