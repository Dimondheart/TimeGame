using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Records all timeline data for the game object, and
	 * also handles playback of recorded data. Also implements ITimelineRecordable
	 * itself to record data for the GameObject itself.</summary>
	 */
	public class TimelineRecorder : MonoBehaviour, ITimelineRecordable
	{
		public Timeline timeline { get; private set; }

		TimelineRecord ITimelineRecordable.MakeTimelineRecord()
		{
			return new TimelineRecordForGameObject();
		}

		void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
		{
			TimelineRecordForGameObject tr = (TimelineRecordForGameObject)record;
			gameObject.SetActive(tr.activeSelf);
		}

		private void Awake()
		{
			timeline = new Timeline();
		}

		private void OnDisable()
		{
			TimelineRecorderForceUpdate.AddDisabledRecorder(this);
		}

		public void Update()
		{
			if (ManipulableTime.ClearingExcessTimelineData)
			{
				timeline.ClearSnapshotsOutsideRange(ManipulableTime.oldestRecordedCycle, ManipulableTime.newestRecordedCycle);
			}
			if (ManipulableTime.RecordModeEnabled)
			{
				TimelineSnapshot snapshot = new TimelineSnapshot();
				Component[] components = gameObject.GetComponents(typeof(Component));
				foreach (Component c in components)
				{
					if (c is ITimelineRecordable)
					{
						TimelineRecord rec = ((ITimelineRecordable)c).MakeTimelineRecord();
						rec.AddCommonData(c);
						snapshot.AddRecord(c, rec);
					}
					else if (TimelineRecordForComponent.HasTimelineRecordMaker(c))
					{
						TimelineRecord rec = TimelineRecordForComponent.MakeTimelineRecord(c);
						rec.AddCommonData(c);
						snapshot.AddRecord(c, rec);
					}
				}
				timeline.AddSnapshot(ManipulableTime.cycleNumber, snapshot);
			}
			else if (ManipulableTime.ApplyingTimelineRecords)
			{
				if (timeline.HasSnapshot(ManipulableTime.cycleNumber))
				{
					timeline.ApplySnapshot(ManipulableTime.cycleNumber);
				}
			}
		}
	}
}
