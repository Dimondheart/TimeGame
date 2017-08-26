using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Allows update functions to be called on TimeRecorder scripts
 * that have been disabled because their game object was made inactive.</summary>
 */
public class TimeRecorderForceUpdate : MonoBehaviour
{
	private static readonly List<TimelineRecorder> disabledTimeRecorders =
		new List<TimelineRecorder>();

	public static void AddDisabledTimeRecorder(TimelineRecorder recorder)
	{
		if (!disabledTimeRecorders.Contains(recorder))
		{
			disabledTimeRecorders.Add(recorder);
		}
	}

	private void Awake()
	{
		disabledTimeRecorders.Clear();
	}

	private void Update()
	{
		List<TimelineRecorder> noLongerDisabled = new List<TimelineRecorder>();
		foreach (TimelineRecorder tr in disabledTimeRecorders)
		{
			tr.Update();
			if (tr.gameObject.activeInHierarchy && tr.enabled)
			{
				noLongerDisabled.Add(tr);
			}
		}
		foreach (TimelineRecorder tr in noLongerDisabled)
		{
			disabledTimeRecorders.Remove(tr);
		}
	}
}
