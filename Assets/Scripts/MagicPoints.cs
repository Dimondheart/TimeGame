using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Standard MP concept.</summary>*/
public class MagicPoints : MonoBehaviour, IMaxValue, ICurrentValue, ITimelineRecordable
{
	public float maxMP;
	public float currentMP { get; private set; }

	float IMaxValue.MaxValue
	{
		get
		{
			return maxMP;
		}
	}

	float ICurrentValue.CurrentValue
	{
		get
		{
			return currentMP;
		}
	}

	TimelineRecord ITimelineRecordable.MakeTimelineRecord()
	{
		TimelineRecord_MagicPoints record = new TimelineRecord_MagicPoints();
		record.maxMP = maxMP;
		record.currentMP = currentMP;
		return record;
	}

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_MagicPoints rec = (TimelineRecord_MagicPoints)record;
		maxMP = rec.maxMP;
		currentMP = rec.currentMP;
	}

	private void Awake()
	{
		currentMP = maxMP;
	}

	private void Update()
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		if (ManipulableTime.IsTimeFrozen)
		{
			return;
		}
		currentMP -= 20.0f * ManipulableTime.deltaTime;
		currentMP = currentMP < 0.0f ? 0.0f : currentMP;
	}

	public class TimelineRecord_MagicPoints : TimelineRecordForComponent
	{
		public float maxMP;
		public float currentMP;
	}
}
