using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Timeline record for colliders.</summary>*/
	public class TimelineRecordForCollider : TimelineRecordForComponent<Collider>
	{
		public bool enabled { get; private set; }

		public override void AddCommonData(Collider recorded)
		{
			enabled = component.enabled;
		}

		public override void ApplyCommonData(Collider recorded)
		{
			component.enabled = enabled;
		}

		public override void RecordState()
		{
			// TODO?
			AddCommonData(null);
		}

		public override void ApplyRecord()
		{
			// TODO?
			ApplyCommonData(null);
		}
	}
}
