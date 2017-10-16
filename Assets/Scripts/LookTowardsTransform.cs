using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Look towards a specified transform.</summary>*/
	public class LookTowardsTransform : RecordableMonoBehaviour
	{
		/**<summary>The transform to look towards.</summary>*/
		public Transform lookTowards;
		/**<summary>Max distance to look towards the transform.</summary>*/
		public float maxDistance = 6.0f;

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

		public sealed class TimelineRecord_LookTowardsTransform : TimelineRecordForBehaviour<LookTowardsTransform>
		{
			public Transform lookTowards;
			public float maxDistance;

			protected override void RecordState(LookTowardsTransform ltt)
			{
				base.RecordState(ltt);
				lookTowards = ltt.lookTowards;
				maxDistance = ltt.maxDistance;
			}

			protected override void ApplyRecord(LookTowardsTransform ltt)
			{
				base.ApplyRecord(ltt);
				ltt.lookTowards = lookTowards;
				ltt.maxDistance = maxDistance;
			}
		}
	}
}
