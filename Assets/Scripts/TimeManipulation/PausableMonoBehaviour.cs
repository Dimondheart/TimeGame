using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Base for all MonoBehaviours that are paused when the game
	 * is paused, and have different functionality depending on if
	 * manipulable time is paused or not. Update, LateUpdate, and
	 * FixedUpdate are all covered by this class.</summary>
	 */
	public abstract class PausableMonoBehaviour : MonoBehaviour
	{

		/**<summary>Called when time is first resumed. By default this will call the
		 * FlowingUpdate() method.</summary>
		 */
		protected virtual void FirstResumedUpdate()
		{
			FlowingUpdate();
		}

		protected virtual void FirstResumedLateUpdate()
		{
			FlowingLateUpdate();
		}

		protected virtual void FirstResumedFixedUpdate()
		{
			FlowingFixedUpdate();
		}

		/**<summary>Called when time is first paused. This will be called after
		 * the equivalent has been done for Unity components (like freezing rigidbodies.)
		 * By default this will call the PausedUpdate() method.</summary>
		 */
		protected virtual void FirstPausedUpdate()
		{
			PausedUpdate();
		}

		protected virtual void FirstPausedLateUpdate()
		{
			PausedLateUpdate();
		}

		protected virtual void FirstPausedFixedUpdate()
		{
			PausedFixedUpdate();
		}

		/**<summary>Called while time is not paused. Is not called on the first
		 * cycle after time is resumed, unless called from another method like
		 * FirstResumedUpdate().</summary>
		 */
		protected virtual void FlowingUpdate()
		{
		}

		protected virtual void FlowingLateUpdate()
		{
		}

		protected virtual void FlowingFixedUpdate()
		{
		}

		/**<summary>Called while time is paused, but not when the game is paused or
		 * time is rewinding/replaying.</summary>
		 */
		protected virtual void PausedUpdate()
		{
		}

		protected virtual void PausedLateUpdate()
		{
		}

		protected virtual void PausedFixedUpdate()
		{
		}

		/**<summary>This base method implements the functionality for calling the 
		 * special update methods.</summary>
		 */
		public virtual void Update()
		{
			if (ManipulableTime.IsGamePaused)
			{
				return;
			}
			if (ManipulableTime.TimePauseState.JustResumed == (ManipulableTime.timePauseState & ManipulableTime.TimePauseState.JustResumed))
			{
				FirstResumedUpdate();
			}
			else if (ManipulableTime.TimePauseState.JustPaused == (ManipulableTime.timePauseState & ManipulableTime.TimePauseState.JustPaused))
			{
				FirstPausedUpdate();
			}
			else if (ManipulableTime.TimePauseState.Flowing == (ManipulableTime.timePauseState & ManipulableTime.TimePauseState.Flowing))
			{
				FlowingUpdate();
			}
			else if (ManipulableTime.TimePauseState.Paused == (ManipulableTime.timePauseState & ManipulableTime.TimePauseState.Paused))
			{
				PausedUpdate();
			}
		}

		public virtual void LateUpdate()
		{
			if (ManipulableTime.IsGamePaused)
			{
				return;
			}
			if (ManipulableTime.TimePauseState.JustResumed == (ManipulableTime.timePauseState & ManipulableTime.TimePauseState.JustResumed))
			{
				FirstResumedLateUpdate();
			}
			else if (ManipulableTime.TimePauseState.JustPaused == (ManipulableTime.timePauseState & ManipulableTime.TimePauseState.JustPaused))
			{
				FirstPausedLateUpdate();
			}
			else if (ManipulableTime.TimePauseState.Flowing == (ManipulableTime.timePauseState & ManipulableTime.TimePauseState.Flowing))
			{
				FlowingLateUpdate();
			}
			else if (ManipulableTime.TimePauseState.Paused == (ManipulableTime.timePauseState & ManipulableTime.TimePauseState.Paused))
			{
				PausedLateUpdate();
			}
		}

		public virtual void FixedUpdate()
		{
			if (ManipulableTime.IsGamePaused)
			{
				return;
			}
			if (ManipulableTime.TimePauseState.JustResumed == (ManipulableTime.timePauseState & ManipulableTime.TimePauseState.JustResumed))
			{
				FirstResumedFixedUpdate();
			}
			else if (ManipulableTime.TimePauseState.JustPaused == (ManipulableTime.timePauseState & ManipulableTime.TimePauseState.JustPaused))
			{
				FirstPausedFixedUpdate();
			}
			else if (ManipulableTime.TimePauseState.Flowing == (ManipulableTime.timePauseState & ManipulableTime.TimePauseState.Flowing))
			{
				FlowingFixedUpdate();
			}
			else if (ManipulableTime.TimePauseState.Paused == (ManipulableTime.timePauseState & ManipulableTime.TimePauseState.Paused))
			{
				PausedFixedUpdate();
			}
		}
	}
}
