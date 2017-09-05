using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Constaintly deals max HP damage to itself.</summary>*/
public class DealPerminentDamageOverTime : MonoBehaviour, ITimelineRecordable
{
	public float damageRate = 1.0f;

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_DealPerminentDamageOverTime rec = (TimelineRecord_DealPerminentDamageOverTime)record;
		damageRate = rec.damageRate;
	}

	TimelineRecord ITimelineRecordable.MakeTimelineRecord()
	{
		TimelineRecord_DealPerminentDamageOverTime record = new TimelineRecord_DealPerminentDamageOverTime();
		record.damageRate = damageRate;
		return record;
	}

	private void Update()
	{
		if (ManipulableTime.ApplyingTimelineRecords || ManipulableTime.IsTimeFrozen)
		{
			return;
		}
		GetComponent<Health>().DoPerminentDamage(damageRate * ManipulableTime.deltaTime);
	}

	public class TimelineRecord_DealPerminentDamageOverTime : TimelineRecordForComponent
	{
		public float damageRate;
	}
}
