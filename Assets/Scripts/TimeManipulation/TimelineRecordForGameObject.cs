using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>A timeline record for a GameObject.</summary>*/
	public sealed class TimelineRecordForGameObject : TimelineRecord<GameObject>
	{
		public bool activeSelf;

		protected override void ApplyRecordedState(GameObject gameObject)
		{
			base.ApplyRecordedState(gameObject);
			gameObject.SetActive(activeSelf);
		}

		protected override void WriteCurrentState(GameObject gameObject)
		{
			base.WriteCurrentState(gameObject);
			activeSelf = gameObject.activeSelf;
		}
	}
}
