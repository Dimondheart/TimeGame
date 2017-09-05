using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuard : MonoBehaviour, ITimelineRecordable, IHitTaker
{
	public GameObject shield;

	TimelineRecord ITimelineRecordable.MakeTimelineRecord()
	{
		TimelineRecord_PlayerGuard record = new TimelineRecord_PlayerGuard();
		return record;
	}

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_PlayerGuard rec = (TimelineRecord_PlayerGuard)record;
	}

	bool IHitTaker.TakeHit(HitInfo hit)
	{
		if (!DynamicInput.GetButton("Guard") || hit.hitBy == null)
		{
			return false;
		}
		return hit.hitBy.IsTouching(shield.GetComponent<Collider2D>());
	}

	int IHitTaker.Priority
	{
		get
		{
			return 100;
		}
	}

	private void Update()
	{
		if (ManipulableTime.ApplyingTimelineRecords || ManipulableTime.IsTimeFrozen)
		{
			return;
		}
		shield.GetComponent<SpriteRenderer>().enabled = DynamicInput.GetButton("Guard");
	}

	public class TimelineRecord_PlayerGuard : TimelineRecordForComponent
	{
	}
}
