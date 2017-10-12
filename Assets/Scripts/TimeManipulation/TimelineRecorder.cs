using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Records all timeline data for the game object, and
	 * also handles playback of recorded data. Also implements ITimelineRecordable
	 * itself to record data for the GameObject itself.</summary>
	 */
	public class TimelineRecorder : PausableMonoBehaviour
	{
		public Dictionary<Component, Timeline> otherComponentTimelines { get; private set; }

		private void Awake()
		{
			otherComponentTimelines = new Dictionary<Component, Timeline>();
		}

		private void Start()
		{
			Component[] components = GetComponents<Component>();
			foreach (Component c in components)
			{
				if (c is RecordableMonoBehaviour)
				{
					continue;
				}
				if (TimelineRecordForComponent<Component>.HasTimelineMaker(c))
				{

				}
			}
		}

		private void OnDisable()
		{
			TimelineRecorderForceUpdate.AddDisabledRecorder(this);
		}

		public override void Update()
		{
			base.Update();
			if (ManipulableTime.IsRecording)
			{
				Component[] components = GetComponents<Component>();
				foreach (Component c in components)
				{
					if (c is RecordableMonoBehaviour)
					{
						((RecordableMonoBehaviour)c).WriteRecord();
					}
					else if (TimelineRecordForComponent<Component>.HasTimelineMaker(c))
					{
						if (!otherComponentTimelines.ContainsKey(c))
						{
							otherComponentTimelines[c] =
								TimelineRecordForComponent<Component>.MakeTimeline(c);
						}
						TimelineRecordForComponent<Component>.WriteRecord(otherComponentTimelines[c], c);
					}
					else if (TimelineRecordForComponentWithEnabled.IsComponentWithEnabled(c))
					{
						if (!otherComponentTimelines.ContainsKey(c))
						{
							otherComponentTimelines[c] =
								new Timeline<TimelineRecordForComponentWithEnabled>();
						}
						((Timeline<TimelineRecordForComponentWithEnabled>)otherComponentTimelines[c])
							.GetRecordForCurrentCycle().AddCommonData(c);
					}
					else if (c is Behaviour)
					{
						if (!otherComponentTimelines.ContainsKey(c))
						{
							otherComponentTimelines[c] = new Timeline<TimelineRecord_Behaviour>();
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
					else if (TimelineRecordForComponent<Component>.HasTimelineMaker(c))
					{
						if (otherComponentTimelines.ContainsKey(c))
						{
							TimelineRecordForComponent<Component>.ApplyRecord(
								otherComponentTimelines[c],
								ManipulableTime.cycleNumber,
								c
								);
						}
					}
					else if (TimelineRecordForComponentWithEnabled.IsComponentWithEnabled(c))
					{
						if (otherComponentTimelines.ContainsKey(c))
						{
							((Timeline<TimelineRecordForComponentWithEnabled>)otherComponentTimelines[c])
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
