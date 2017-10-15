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

		public RecordableMonoBehaviour() : base()
		{
			timeline = new Timeline(GetType(), false);
		}
	}
}
