using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Records all timeline data for the game object, and
	 * also handles playback of recorded data. Also implements ITimelineRecordable
	 * itself to record data for the GameObject itself.</summary>
	 */
	public class TimelineRecorder : MonoBehaviour
	{
		public CombinedTimeline timeline { get; private set; }

		private void Awake()
		{
			timeline = new CombinedTimeline(gameObject);
		}

		private void OnDisable()
		{
			TimelineRecorderForceUpdate.AddDisabledRecorder(this);
		}

		public void Update()
		{
			if (ManipulableTime.ClearingExcessTimelineData)
			{
				timeline.RemoveSnapshotsOutsideRange(ManipulableTime.oldestRecordedCycle, ManipulableTime.newestRecordedCycle);
			}
			if (ManipulableTime.RecordModeEnabled)
			{
				TimelineSnapshot snapshot = timeline.GetSnapshotForRecording();
				if (snapshot.gameObjectRecord == null)
				{
					snapshot.gameObjectRecord = new TimelineRecordForGameObject(gameObject);
				}
				else
				{
					snapshot.gameObjectRecord.AddCommonData(gameObject);
				}
				Component[] components = gameObject.GetComponents(typeof(Component));
				foreach (Component c in components)
				{
					if (snapshot.HasRecord(c))
					{
						snapshot.GetRecord(c).RecordState();
					}
					else
					{
						if (c is ITimelineRecordable)
						{
							TimelineRecordForBehaviour rec = ((ITimelineRecordable)c).MakeTimelineRecord();
							rec.SetupRecord(c);
							rec.RecordState();
							snapshot.AddRecord(c, rec);
						}
						else if (TimelineRecordForBehaviour.HasTimelineRecordMaker(c))
						{
							TimelineRecordForBehaviour rec = TimelineRecordForBehaviour.MakeTimelineRecord(c);
							rec.SetupRecord(c);
							rec.RecordState();
							snapshot.AddRecord(c, rec);
						}
					}
				}
				timeline.AddSnapshot(ManipulableTime.cycleNumber, snapshot);
			}
			else if (ManipulableTime.IsApplyingRecords)
			{
				if (timeline.HasSnapshot(ManipulableTime.cycleNumber))
				{
					timeline.ApplySnapshot(ManipulableTime.cycleNumber);
				}
			}
		}
	}
}
