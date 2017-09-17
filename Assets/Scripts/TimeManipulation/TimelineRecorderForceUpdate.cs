using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Allows update functions to be called on TimeRecorder scripts
	 * that have been disabled because their game object was made inactive.</summary>
	 */
	public class TimelineRecorderForceUpdate : MonoBehaviour
	{
		private static readonly List<TimelineRecorder> disabledRecorders =
			new List<TimelineRecorder>();

		public static void AddDisabledRecorder(TimelineRecorder recorder)
		{
			if (!disabledRecorders.Contains(recorder))
			{
				disabledRecorders.Add(recorder);
			}
		}

		private void Awake()
		{
			disabledRecorders.Clear();
		}

		private void Update()
		{
			List<TimelineRecorder> noLongerDisabled = new List<TimelineRecorder>();
			foreach (TimelineRecorder tr in disabledRecorders)
			{
				if (tr.gameObject.activeInHierarchy && tr.enabled)
				{
					noLongerDisabled.Add(tr);
				}
				else
				{
					tr.Update();
				}
			}
			foreach (TimelineRecorder tr in noLongerDisabled)
			{
				disabledRecorders.Remove(tr);
			}
		}
	}
}
