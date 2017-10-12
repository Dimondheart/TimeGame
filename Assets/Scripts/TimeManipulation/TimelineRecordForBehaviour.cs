using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	public abstract class TimelineRecordForBehaviour<T> : TimelineRecordForComponent<T> where T : Behaviour
	{
		public bool enabled { get; private set; }

		public override void AddCommonData(T behaviour)
		{
			enabled = behaviour.enabled;
		}

		public override void ApplyCommonData(T behaviour)
		{
			behaviour.enabled = enabled;
		}
	}

	/**<summary>Base class for all timeline record classes created for
 * components.</summary>
 */
	public abstract class TimelineRecordForBehaviour : TimelineRecordForComponent<Component>
	{
	}
}
