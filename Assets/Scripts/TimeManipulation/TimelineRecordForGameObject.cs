using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>A timeline record for a game object.</summary>*/
	public class TimelineRecordForGameObject : TimelineRecord<GameObject>
	{
		public bool activeSelf;

		public TimelineRecordForGameObject(GameObject gameObject)
		{
			AddCommonData(gameObject);
		}

		public override void AddCommonData(GameObject gameObject)
		{
			activeSelf = gameObject.activeSelf;
		}

		public override void ApplyCommonData(GameObject gameObject)
		{
			gameObject.SetActive(activeSelf);
		}
	}
}
