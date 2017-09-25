using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Changes the combat mode, used by other scripts.</summary>*/
	public class PlayerCombatMode : MonoBehaviour
	{
		/**<summary>The current combat mode setting.</summary>*/
		public CombatMode currentMode { get; private set; }
		/**<summary>The combat mode to set the current mode to during late update.</summary>*/
		private CombatMode nextMode;

		private void Update()
		{
			if (ManipulableTime.IsTimeOrGamePaused)
			{
				return;
			}
		}

		private void LateUpdate()
		{
			if (ManipulableTime.IsTimeOrGamePaused)
			{
				return;
			}
			currentMode = nextMode;
		}

		/**<summary>Change the combat mode. Change is not applied until LateUpdate
		 * to synchronize all behaviours that are affected by the combat mode.</summary>
		 */
		public void SetCombatMode(CombatMode mode)
		{
		}

		public enum CombatMode : byte
		{
			Unarmed = 0,
			Offensive,
			Defensive,
			Ranged
		}
	}
}
