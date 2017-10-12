using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Resets the Timeline on scene load and advances the Timeline
	 * to the next cycle when needed.</summary>
	 */
	public class TimelineController : MonoBehaviour
	{
		private void Awake()
		{
			Timeline.Reset();
		}

		private void Update()
		{
			if (!ManipulableTime.IsTimePaused)
			{
				Timeline.AdvanceToNextCycle();
			}
		}
	}
}
