using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.Project1
{
	/**<summary>Base class for classes that help relay information to
	 * the character tracking system.</summary>
	 */
	public abstract class CharacterTrackerHelper : MonoBehaviour
	{
		public CharacterTracker tracker { get; private set; }

		protected virtual void Awake()
		{
			tracker = GameObject.FindGameObjectWithTag("GameController").GetComponent<CharacterTracker>();
		}
	}
}
