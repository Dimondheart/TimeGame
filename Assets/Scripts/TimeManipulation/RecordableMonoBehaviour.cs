using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>A PausableMonoBehaviour that can be recorded and played back
	 * or rewound when manipulable time is doing the same.</summary>
	 */
	public class RecordableMonoBehaviour : PausableMonoBehaviour
	{
		/**<summary>The timeline of records for this recordable.</summary>*/
		public Timeline timeline { get; private set; }

		/**<summary>Record the current state into the current timeline position.</summary>*/
		public void WriteRecord()
		{
			timeline.GetRecordForCurrentCycle().WriteCurrentState(this);
		}

		/**<summary>Override the current state with the state recorded for the
		 * given cycle.</summary>
		 */
		public void ApplyRecord(int cycle)
		{
			timeline.GetRecord(cycle).ApplyRecordedState(this);
		}
	}
}
