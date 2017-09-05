using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour, ITimelineRecordable
{
	public GameObject swordAnimator;
	/**<summary>Delay between attacks, in seconds.</summary>*/
	public float cooldown = 0.25f;
	/**<summary>HP damage per attack.</summary>*/
	public int damagePerHit = 5;
	/**<summary>Time the last attack was made.</summary>*/
	private ConvertableTimeRecord lastAttackTime;

	TimelineRecord ITimelineRecordable.MakeTimelineRecord()
	{
		TimelineRecord_PlayerMelee record = new TimelineRecord_PlayerMelee();
		record.cooldown = cooldown;
		record.damagePerHit = damagePerHit;
		record.lastAttackTime = lastAttackTime;
		return record;
	}

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_PlayerMelee rec = (TimelineRecord_PlayerMelee)record;
		cooldown = rec.cooldown;
		damagePerHit = rec.damagePerHit;
		lastAttackTime = rec.lastAttackTime;
	}

	private void Awake()
	{
		lastAttackTime = ConvertableTimeRecord.GetTime();
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
		if (DynamicInput.GetButton("Melee") && GetComponent<Health>().IsAlive)
		{
			if (ManipulableTime.time - lastAttackTime.manipulableTime >= cooldown)
			{
				swordAnimator.GetComponent<Animator>().enabled = true;
				lastAttackTime.SetToCurrent();
			}
		}
	}

	public class TimelineRecord_PlayerMelee : TimelineRecordForComponent
	{
		public float cooldown;
		public int damagePerHit;
		public ConvertableTimeRecord lastAttackTime;
	}
}
