using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>A timeline record for a GameObject.</summary>*/
	public sealed class TimelineRecordForGameObject : TimelineRecord<GameObject>
	{
		public bool activeSelf;

		protected override void ApplyRecord(GameObject gameObject)
		{
			base.ApplyRecord(gameObject);
			gameObject.SetActive(activeSelf);
		}

		protected override void RecordState(GameObject gameObject)
		{
			base.RecordState(gameObject);
			activeSelf = gameObject.activeSelf;
		}
	}
}
