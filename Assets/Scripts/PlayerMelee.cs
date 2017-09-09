using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour, ITimelineRecordable
{
	public Sword sword;
	/**<summary>Delay between attacks, in seconds.</summary>*/
	public float cooldown = 0.3f;
	/**<summary>HP damage per attack.</summary>*/
	public int damagePerHit = 10;
	public float swingDuration = 0.25f;
	/**<summary>Time the last attack was made.</summary>*/
	private ConvertableTimeRecord lastAttackTime;

	public bool IsSwinging
	{
		get
		{
			return sword.isSwinging || sword.isEndingSwing;
		}
	}

	public bool IsInCooldown
	{
		get
		{
			return !sword.isSwinging;
		}
	}

	TimelineRecord ITimelineRecordable.MakeTimelineRecord()
	{
		TimelineRecord_PlayerMelee record = new TimelineRecord_PlayerMelee();
		record.cooldown = cooldown;
		record.damagePerHit = damagePerHit;
		record.swingDuration = swingDuration;
		record.lastAttackTime = lastAttackTime;
		return record;
	}

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_PlayerMelee rec = (TimelineRecord_PlayerMelee)record;
		cooldown = rec.cooldown;
		damagePerHit = rec.damagePerHit;
		swingDuration = rec.swingDuration;
		lastAttackTime = rec.lastAttackTime;
	}

	private void Awake()
	{
		lastAttackTime = ConvertableTimeRecord.GetTime();
	}

	private void Update()
	{
		if (ManipulableTime.ApplyingTimelineRecords || ManipulableTime.IsTimeFrozen || !GetComponent<Health>().IsAlive)
		{
			return;
		}
		if (GetComponent<SurfaceInteraction>().IsSwimming)
		{
			StopSwinging();
		}
		else if (DynamicInput.GetButtonDown("Melee") && !GetComponent<PlayerMovement>().IsDashing)
		{
			if (ManipulableTime.time - lastAttackTime.manipulableTime >= swingDuration)
			{
				sword.Swing(swingDuration, cooldown - swingDuration);
				lastAttackTime.SetToCurrent();
			}
		}
	}

	public void StopSwinging()
	{
		if (ManipulableTime.ApplyingTimelineRecords || ManipulableTime.IsTimeFrozen)
		{
			return;
		}
		if (sword.isSwinging || sword.isEndingSwing)
		{
			if (!sword.isEndingSwing)
			{
				lastAttackTime = ConvertableTimeRecord.zeroTime;
			}
			sword.CancelSwing();
		}
	}

	public class TimelineRecord_PlayerMelee : TimelineRecordForComponent
	{
		public float cooldown;
		public int damagePerHit;
		public float swingDuration;
		public ConvertableTimeRecord lastAttackTime;
	}
}
