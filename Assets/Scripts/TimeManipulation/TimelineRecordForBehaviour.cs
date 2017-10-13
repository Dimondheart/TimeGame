using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	public abstract class TimelineRecordForBehaviour : TimelineRecordForComponent
	{
		public bool enabled { get; private set; }

		public override void AddCommonData(Component behaviour)
		{
			enabled = ((Behaviour)behaviour).enabled;
		}

		public override void ApplyCommonData(Component behaviour)
		{
			((Behaviour)behaviour).enabled = enabled;
		}
	}
}
