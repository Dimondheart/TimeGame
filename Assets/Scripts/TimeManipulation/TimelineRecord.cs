using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Base class for all timeline record classes created for
	 * each TimlineRecordable.</summary>
	 */
	public abstract class TimelineRecord
	{
		public abstract void AddCommonData(Component component);
		public abstract void ApplyCommonData(Component component);
	}
}
