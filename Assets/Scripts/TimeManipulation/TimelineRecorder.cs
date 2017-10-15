using System.Collections;
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
			foreach (Component c in GetComponents<Component>())
			{
				if (c is RecordableMonoBehaviour)
				{
					continue;
				}
				if (TimelineRecordForComponent<Component>.HasTimelineMaker(c))
				{
					otherComponentTimelines[c] =
						TimelineRecordForComponent<Component>.MakeTimeline(c);
				}
				else if (TimelineRecordForComponent<Component>.IsComponentWithEnabled(c))
				{
					otherComponentTimelines[c] =
						new Timeline(typeof(TimelineRecord_ComponentWithEnabled), true);
				}
				else if (c is Behaviour && !(c is TimelineRecorder))
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
				foreach (Component c in GetComponents<Component>())
				{
					if (c is RecordableMonoBehaviour)
					{
						((RecordableMonoBehaviour)c).timeline.GetRecordForCurrentCycle().WriteRecord(c);
					}
					else if (TimelineRecordForComponent<Component>.HasTimelineMaker(c))
					{
						if (!otherComponentTimelines.ContainsKey(c))
						{
							otherComponentTimelines[c] =
								TimelineRecordForComponent<Component>.MakeTimeline(c);
						}
						otherComponentTimelines[c].GetRecordForCurrentCycle().WriteRecord(c);
					}
					else if (TimelineRecordForComponent<Component>.IsComponentWithEnabled(c))
					{
						if (!otherComponentTimelines.ContainsKey(c))
						{
							otherComponentTimelines[c] =
								new Timeline(typeof(TimelineRecord_ComponentWithEnabled), true);
						}
						otherComponentTimelines[c].GetRecordForCurrentCycle().WriteRecord(c);
					}
					else if (c is Behaviour && !(c is TimelineRecorder))
					{
						if (!otherComponentTimelines.ContainsKey(c))
						{
							otherComponentTimelines[c] =
								new Timeline(typeof(TimelineRecord_Behaviour), true);
						}
						otherComponentTimelines[c].GetRecordForCurrentCycle().WriteRecord(c);
					}
				}
			}
			else if (ManipulableTime.IsApplyingRecords)
			{
				foreach (Component c in GetComponents<Component>())
				{
					if (c is RecordableMonoBehaviour)
					{
						if (((RecordableMonoBehaviour)c).timeline.HasRecord(ManipulableTime.cycleNumber))
						{
							((RecordableMonoBehaviour)c).timeline
								.GetRecord(ManipulableTime.cycleNumber).ApplyRecord(c);
						}
					}
					else if (otherComponentTimelines.ContainsKey(c))
					{
						if (otherComponentTimelines[c].HasRecord(ManipulableTime.cycleNumber))
						{
							otherComponentTimelines[c].GetRecord(ManipulableTime.cycleNumber).ApplyRecord(c);
						}
					}
				}
			}
		}
	}
}
