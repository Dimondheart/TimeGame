using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Controls the time freezing ability of the player.</summary>*/
public class ChronoFreeze : MonoBehaviour
{
	private void Update()
	{
		if (!GetComponent<Health>().IsAlive)
		{
			return;
		}
		if (DynamicInput.GetButtonDown("Toggle Time Freeze") && !ManipulableTime.IsGameFrozen)
		{
			if (GetComponent<ChronoPoints>().isCharacterFreezingTime)
			{
				ManipulableTime.IsTimeFrozen = false;
				GetComponent<ChronoPoints>().isCharacterFreezingTime = false;
			}
			else if (!ManipulableTime.IsTimeFrozen && GetComponent<ChronoPoints>().CanActivateChronoFreeze)
			{
				ManipulableTime.IsTimeFrozen = true;
				GetComponent<ChronoPoints>().isCharacterFreezingTime = true;
			}
		}
	}
}
