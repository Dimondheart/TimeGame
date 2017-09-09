using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuard : MonoBehaviour, ITimelineRecordable, IHitTaker
{
	public GameObject shield;

	public bool IsGuarding
	{
		get
		{
			return shield.GetComponent<SpriteRenderer>().enabled;
		}
	}

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
		return !shield.GetComponent<Collider2D>().isTrigger && hit.hitCollider == shield.GetComponent<Collider2D>();
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
		if (DynamicInput.GetButtonDown("Guard"))
		{
			if (!GetComponent<PlayerMovement>().IsDashing)
			{
				GetComponent<PlayerMelee>().StopSwinging();
				Enabled(true);
			}
			else
			{
				Enabled(false);
			}
		}
		else if (DynamicInput.GetButton("Guard"))
		{
			if (!GetComponent<PlayerMovement>().IsDashing && GetComponent<PlayerMelee>().IsInCooldown)
			{
				GetComponent<PlayerMelee>().StopSwinging();
				Enabled(true);
			}
			else
			{
				Enabled(false);
			}
		}
		else
		{
			Enabled(false);
		}
	}

	private void Enabled(bool enabled)
	{
		shield.GetComponent<SpriteRenderer>().enabled = enabled;
		shield.GetComponent<Collider2D>().isTrigger = !enabled;
	}

	public class TimelineRecord_PlayerGuard : TimelineRecordForComponent
	{
	}
}
