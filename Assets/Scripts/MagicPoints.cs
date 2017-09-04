using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Standard MP concept.</summary>*/
public class MagicPoints : MonoBehaviour, IPrimaryValue, ITimelineRecordable
{
	public float maxMP = 100.0f;
	public float regenRate = 20.0f;
	public float currentMP { get; private set; }

	float IPrimaryValue.MaxValue
	{
		get
		{
			return maxMP;
		}
	}

	float IPrimaryValue.CurrentValue
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
		record.regenRate = regenRate;
		record.currentMP = currentMP;
		return record;
	}

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_MagicPoints rec = (TimelineRecord_MagicPoints)record;
		maxMP = rec.maxMP;
		regenRate = rec.regenRate;
		currentMP = rec.currentMP;
	}

	private void Awake()
	{
		currentMP = 0.0f;
	}

	private void Update()
	{
		if (ManipulableTime.ApplyingTimelineRecords || ManipulableTime.IsTimeFrozen)
		{
			return;
		}
		float regenAmount = regenRate * ManipulableTime.deltaTime;
		if (!GetComponent<ElementalAlignment>().IsStable)
		{
			regenAmount *= 0.5f;
		}
		currentMP = Mathf.Clamp(currentMP + regenAmount, 0.0f, maxMP);
	}

	public class TimelineRecord_MagicPoints : TimelineRecordForComponent
	{
		public float maxMP;
		public float regenRate;
		public float currentMP;
	}
}
