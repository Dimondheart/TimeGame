using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	public abstract class TimelineRecordForBehaviour<T> : TimelineRecordForComponent<T>
		where T : Behaviour
	{
		public bool enabled;

		protected override void ApplyRecord(T behaviour)
		{
			base.ApplyRecord(behaviour);
			behaviour.enabled = enabled;
		}

		protected override void RecordState(T behaviour)
		{
			base.RecordState(behaviour);
			enabled = behaviour.enabled;
		}
	}
}
