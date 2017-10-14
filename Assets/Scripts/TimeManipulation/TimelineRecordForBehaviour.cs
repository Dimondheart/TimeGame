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

		protected override void ApplyRecordedState(T behaviour)
		{
			base.ApplyRecordedState(behaviour);
			behaviour.enabled = enabled;
		}

		protected override void WriteCurrentState(T behaviour)
		{
			base.WriteCurrentState(behaviour);
			enabled = behaviour.enabled;
		}
	}
}
