using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Interface for all custom monobehaviors that support creating
	 * and using timeline records.</summary>
	 */
	public interface ITimelineRecordable
	{
		/**<summary>Make a new timeline record and record the current state into it.</summary>*/
		TimelineRecordForComponent MakeTimelineRecord();
		/**<summary>Record the property values needed to restore the class (and any base classes,
		 * stopping before MonoBehaviour and Behaviour if they are extended  to the state it is
		 * in at this time, excluding any data that should not be recorded.</summary>
		 */
		/**<summary>Record the property values needed to restore the class (and any base classes,
		 * stopping before MonoBehaviour and Behaviour if they are extended  to the state it is
		 * in at this time, excluding any data that should not be recorded.</summary>
		 */
		void RecordCurrentState(TimelineRecordForComponent record);
		/**<summary>Set the property values of the class (and its base classes, excluding
		 * MonoBehaviour and Behaviour if they are extended) to the values in the given
		 * record.</summary>
		 */
		void ApplyTimelineRecord(TimelineRecordForComponent record);
	}
}
