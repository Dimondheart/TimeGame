using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.Project1
{
	/**<summary>Helps the chracter tracker by relaying data for the attached
	 * player character.</summary>
	 */
	public class PlayerTrackerHelper : CharacterTrackerHelper
	{
		protected override void Awake()
		{
			base.Awake();
			tracker.AddPlayer(this);
		}
	}
}
