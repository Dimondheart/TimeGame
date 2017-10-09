using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary></summary>*/
	public abstract class TimelineRecordForComponent<T> : TimelineRecord<T> where T : Component
	{
		public T component { get; private set; }

		public abstract void RecordState();
		public abstract void ApplyRecord();

		public virtual void SetupRecord(T component)
		{
			this.component = component;
		}
	}
}
