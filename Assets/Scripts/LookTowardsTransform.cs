using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Look towards a specified transform.</summary>*/
	public class LookTowardsTransform : RecordableMonoBehaviour<TimelineRecord_LookTowardsTransform>
	{
		/**<summary>The transform to look towards.</summary>*/
		public Transform lookTowards;
		/**<summary>Max distance to look towards the transform.</summary>*/
		public float maxDistance = 6.0f;

		protected override void WriteCurrentState(TimelineRecord_LookTowardsTransform record)
		{
			record.lookTowards = lookTowards;
			record.maxDistance = maxDistance;
		}

		protected override void ApplyRecordedState(TimelineRecord_LookTowardsTransform record)
		{
			lookTowards = record.lookTowards;
			maxDistance = record.maxDistance;
		}

		protected override void FlowingUpdate()
		{
			if (!GetComponent<Health>().IsAlive)
			{
				return;
			}
			Vector2 lookVector = lookTowards.position - transform.position;
			if (lookVector.magnitude <= maxDistance)
			{
				GetComponent<DirectionLooking>().Direction = lookVector;
			}
		}
	}

	public class TimelineRecord_LookTowardsTransform : TimelineRecordForBehaviour
	{
		public Transform lookTowards;
		public float maxDistance;
	}
}
