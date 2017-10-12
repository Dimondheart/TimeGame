using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.DynamicInputSystem;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Controls the time freezing ability of the player.</summary>*/
	public class ChronoFreeze : MonoBehaviour
	{
		private void Update()
		{
			if (!GetComponent<Health>().IsAlive)
			{
				return;
			}
			if (DynamicInput.GetButtonDown("Toggle Time Freeze") && !ManipulableTime.IsGamePaused)
			{
				if (GetComponent<ChronoPoints>().isCharacterFreezingTime)
				{
					ManipulableTime.ChangeTimePaused(false);
					GetComponent<ChronoPoints>().isCharacterFreezingTime = false;
				}
				else if (!ManipulableTime.IsTimeOrGamePaused && GetComponent<ChronoPoints>().CanActivateChronoFreeze)
				{
					ManipulableTime.ChangeTimePaused(true);
					GetComponent<ChronoPoints>().isCharacterFreezingTime = true;
				}
			}
		}
	}
}
