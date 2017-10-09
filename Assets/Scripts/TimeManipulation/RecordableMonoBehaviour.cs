using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary></summary>*/
	public abstract class RecordableMonoBehaviour<T> : PausableMonoBehaviour where T : TimelineRecordForBehaviour, new()
	{
		public Timeline<T> timeline;

		public abstract void RecordCurrentState(TimelineRecordForBehaviour record);
		public abstract void ApplyTimelineRecord(TimelineRecordForBehaviour record);

		void 
	}
}
