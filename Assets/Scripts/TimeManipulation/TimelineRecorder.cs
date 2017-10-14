﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Records all timeline data for the game object and its
	 * components, and also applies timeline records during
	 * rewind/replay.</summary>
	 */
	public class TimelineRecorder : MonoBehaviour
	{
		/**<summary>Timelines for components that do not inherit from the
		 * recordable classes (like core Unity components.)</summary>
		 */
		public Dictionary<Component, Timeline> otherComponentTimelines { get; private set; }

		private void Awake()
		{
			otherComponentTimelines = new Dictionary<Component, Timeline>();
		}

		private void Start()
		{
			// Create necessary timelines now to make the first record cycle smoother
			Component[] components = GetComponents<Component>();
			foreach (Component c in components)
			{
				if (c is RecordableMonoBehaviour)
				{
					continue;
				}
				if (TimelineRecordForComponent.HasTimelineMaker(c))
				{
					otherComponentTimelines[c] =
						TimelineRecordForComponent.MakeTimeline(c);
				}
				else if (TimelineRecordForComponent.IsComponentWithEnabled(c))
				{
					otherComponentTimelines[c] =
						new Timeline(typeof(TimelineRecord_ComponentWithEnabled), true);
				}
				else if (c is Behaviour)
				{
					otherComponentTimelines[c] =
						new Timeline(typeof(TimelineRecord_Behaviour), true);
				}
			}
		}

		private void OnDisable()
		{
			TimelineRecorderForceUpdate.AddDisabledRecorder(this);
		}

		public void Update()
		{
			if (ManipulableTime.IsRecording)
			{
				Component[] components = GetComponents<Component>();
				foreach (Component c in components)
				{
					if (c is RecordableMonoBehaviour)
					{
						((RecordableMonoBehaviour)c).WriteRecord();
					}
					else if (TimelineRecordForComponent.HasTimelineMaker(c))
					{
						if (!otherComponentTimelines.ContainsKey(c))
						{
							otherComponentTimelines[c] =
								TimelineRecordForComponent.MakeTimeline(c);
						}
						TimelineRecordForComponent.WriteRecord(otherComponentTimelines[c], c);
					}
					else if (TimelineRecordForComponent.IsComponentWithEnabled(c))
					{
						if (!otherComponentTimelines.ContainsKey(c))
						{
							otherComponentTimelines[c] =
								new Timeline(typeof(TimelineRecord_ComponentWithEnabled), true);
						}
						otherComponentTimelines[c].GetRecordForCurrentCycle().WriteCurrentState(c);
					}
					else if (c is Behaviour)
					{
						if (!otherComponentTimelines.ContainsKey(c))
						{
							otherComponentTimelines[c] =
								new Timeline(typeof(TimelineRecord_Behaviour), true);
						}
						((Timeline<TimelineRecord_Behaviour>)otherComponentTimelines[c])
							.GetRecordForCurrentCycle().AddCommonData((Behaviour)c);
					}
				}
			}
			else if (ManipulableTime.IsApplyingRecords)
			{
				Component[] components = GetComponents<Component>();
				foreach (Component c in components)
				{
					if (c is RecordableMonoBehaviour)
					{
						((RecordableMonoBehaviour)c).ApplyRecord(ManipulableTime.cycleNumber);
					}
					else if (TimelineRecordForComponent.HasTimelineMaker(c))
					{
						if (otherComponentTimelines.ContainsKey(c))
						{
							TimelineRecordForComponent.ApplyRecord(
								otherComponentTimelines[c],
								ManipulableTime.cycleNumber,
								c
								);
						}
					}
					else if (TimelineRecordForComponent.IsComponentWithEnabled(c))
					{
						if (otherComponentTimelines.ContainsKey(c))
						{
							((Timeline<TimelineRecord_ComponentWithEnabled>)otherComponentTimelines[c])
								.GetRecordForCurrentCycle().ApplyCommonData(c);
						}
					}
					else if (c is Behaviour)
					{
						if (otherComponentTimelines.ContainsKey(c))
						{
							((Timeline<TimelineRecord_Behaviour>)otherComponentTimelines[c])
								.GetRecordForCurrentCycle().ApplyCommonData((Behaviour)c);
						}
					}
				}
			}
		}
	}
}
