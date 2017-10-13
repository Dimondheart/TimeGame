using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Non-generic base for recordable monobehaviours. Defines methods
	 * to record and apply records without providing the record itself, which
	 * eliminates the need for the record to be casted several times before it
	 * is used.</summary>
	 */
	public abstract class RecordableMonoBehaviour : PausableMonoBehaviour
	{
		/**<summary>Record the current state into the current timeline position.</summary>*/
		public abstract void WriteRecord();
		/**<summary>Override the current state with the state recorded for the
		 * given cycle.</summary>
		 */
		public abstract void ApplyRecord(int cycle);
	}

	/**<summary>A PausableMonoBehaviour that can be recorded and played back
	 * or rewound when manipulable time is doing the same.</summary>
	 */
	public abstract class RecordableMonoBehaviour<T> : RecordableMonoBehaviour where T : TimelineRecordForBehaviour, new()
	{
		/**<summary>The timeline of records for this recordable.</summary>*/
		private Timeline<T> timeline = new Timeline<T>();

		/**<summary>Write the current state (values) of the class into the specified
		 * record.</summary>
		 */
		protected abstract void WriteCurrentState(T record);
		/**<summary>Override the current state (values) of the class with the given
		 * record.</summary>
		 */
		protected abstract void ApplyRecordedState(T record);

		public override void WriteRecord()
		{
			WriteCurrentState(timeline.GetRecordForCurrentCycle());
			timeline.GetRecordForCurrentCycle().AddCommonData(this);
		}

		public override void ApplyRecord(int cycle)
		{
			ApplyRecordedState(timeline.GetRecord(cycle));
			timeline.GetRecord(cycle).ApplyCommonData(this);
		}
	}
}
