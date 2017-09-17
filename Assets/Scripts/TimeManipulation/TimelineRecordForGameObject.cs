using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>A timeline record for a game object.</summary>*/
	public class TimelineRecordForGameObject : TimelineRecord
	{
		public bool activeSelf;

		public override void AddCommonData(Component component)
		{
			activeSelf = component.gameObject.activeSelf;
		}

		public override void ApplyCommonData(Component component)
		{
			component.gameObject.SetActive(activeSelf);
		}
	}
}
