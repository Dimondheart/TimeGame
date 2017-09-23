using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	public abstract class TimelineRecord
	{
	}

	/**<summary>Base class for all timeline record classes created for
	 * each ITimlineRecordable.</summary>
	 */
	public abstract class TimelineRecord<T> : TimelineRecord
	{
		public abstract void AddCommonData(T recorded);
		public abstract void ApplyCommonData(T recorded);
	}
}
