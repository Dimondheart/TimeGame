using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary></summary>*/
	public abstract class PausableMonoBehaviour : MonoBehaviour
	{
		/**<summary>Called when time is first paused. By default this will call the
		 * NormalUpdate() method, so this should be overrided when subclasses require
		 * different behaviour for this cycle.</summary>
		 */
		protected virtual void PauseTimeUpdate()
		{
			PausedUpdate();
		}

		/**<summary>Called when time is first resumed. By default this will call the
		 * NormalUpdate() method, so this should be overrided when subclasses require
		 * different behaviour for this cycle.</summary>
		 */
		protected virtual void ResumeTimeUpdate()
		{
			NormalUpdate();
		}

		/**<summary>Called while time is advancing normally.</summary>*/
		protected virtual void NormalUpdate()
		{
		}

		/**<summary>Called while time is paused, but not rewinding or replaying.</summary>*/
		protected virtual void PausedUpdate()
		{
		}

		void Update()
		{
			if (ManipulableTime.IsApplyingRecords || ManipulableTime.IsGamePaused)
			{
				return;
			}
			switch (ManipulableTime.timePauseState)
			{
				case ManipulableTime.TimePauseState.Flowing:
					NormalUpdate();
					break;
				case ManipulableTime.TimePauseState.Paused:
					PausedUpdate();
					break;
				case ManipulableTime.TimePauseState.JustResumed:
					ResumeTimeUpdate();
					break;
				case ManipulableTime.TimePauseState.JustPaused:
					PauseTimeUpdate();
					break;
				case ManipulableTime.TimePauseState.ResumingNextCycle:
					PausedUpdate();
					break;
				case ManipulableTime.TimePauseState.PausingNextCycle:
					NormalUpdate();
					break;
				default:
					Debug.LogWarning("Unhandled time pause state:" + ManipulableTime.timePauseState);
					break;
			}
		}
	}
}
