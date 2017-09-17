using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.DynamicInputSystem;
using TechnoWolf.TimeManipulation;

/**<summary>Handles the players ability to rewind time.</summary>*/
public class ChronoRewind : MonoBehaviour
{
	/**<summary>If the player is currently trying to rewind/replay.
	 * This will be true when the player is trying to use the ability,
	 * so it does not always mean it is actually active.</summary>
	 */
	public bool isAbilityActive { get; private set; }

	private void Update()
	{
		if (!GetComponent<Health>().IsAlive)
		{
			return;
		}
		float controlValue = DynamicInput.GetAxisRaw("Rewind/Replay");
		isAbilityActive = !Mathf.Approximately(0.0f, controlValue);
		if (isAbilityActive)
		{
			if (controlValue < 0.0f)
			{
				ManipulableTime.InitiateRewind();
			}
			else
			{
				ManipulableTime.InitiateReplay();
			}
		}
		else
		{
			ManipulableTime.StopRewind();
			ManipulableTime.StopReplay();
		}
	}
}
