using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>A timeline record for a game object.</summary>*/
	public class TimelineRecordForGameObject : TimelineRecord
	{
		public bool activeSelf;

		public TimelineRecordForGameObject(GameObject gameObject)
		{
			AddCommonData(gameObject);
		}

		public void AddCommonData(GameObject gameObject)
		{
			activeSelf = gameObject.activeSelf;
		}

		public void ApplyCommonData(GameObject gameObject)
		{
			gameObject.SetActive(activeSelf);
		}
	}
}
