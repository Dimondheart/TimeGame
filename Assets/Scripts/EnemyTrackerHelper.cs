﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.Project1
{
	/**<summary>Helps a character tracker by attaching to each enemy and
	 * relaying useful information.</summary>
	 */
	public class EnemyTrackerHelper : CharacterTrackerHelper
	{
		protected override void Awake()
		{
			base.Awake();
			tracker.AddEnemy(this);
		}
	}
}
