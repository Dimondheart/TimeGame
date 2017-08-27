using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Records all timeline data for the game object, and
 * also handles playback of recorded data. Also implements ITimelineRecordable
 * itself to record data for the GameObject itself.</summary>
 */
public class TimelineRecorder : MonoBehaviour, ITimelineRecordable
{
	public Timeline timeline { get; private set; }

	TimelineRecord ITimelineRecordable.MakeTimelineRecord()
	{
		TimelineRecord_GameObject newTR = new TimelineRecord_GameObject();
		newTR.activeSelf = gameObject.activeSelf;
		return newTR;
	}

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_GameObject tr = (TimelineRecord_GameObject)record;
		gameObject.SetActive(tr.activeSelf);
	}

	private void Awake()
	{
		timeline = new Timeline();
	}

	private void OnDisable()
	{
		TimelineRecorderForceUpdate.AddDisabledTimeRecorder(this);
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
					snapshot.AddRecord(c, ((ITimelineRecordable)c).MakeTimelineRecord());
				}
				else if (TimelineRecord.HasTimelineRecordMaker(c))
				{
					snapshot.AddRecord(c, TimelineRecord.MakeTimelineRecord(c));
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

	public class TimelineRecord_GameObject : TimelineRecord
	{
		public bool activeSelf;
	}
}
