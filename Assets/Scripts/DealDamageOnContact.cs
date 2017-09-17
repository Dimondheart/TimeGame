using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

/**<summary>Deal damage to opponents on contact.</summary>*/
public class DealDamageOnContact : MonoBehaviour, ITimelineRecordable
{
	/**<summary>Delay between attacks, in seconds.</summary>*/
	public float cooldown = 0.25f;
	/**<summary>HP damage per attack.</summary>*/
	public int damagePerHit = 5;
	/**<summary>Time the last attack was made.</summary>*/
	private ConvertableTimeRecord lastAttackTime;

	TimelineRecord ITimelineRecordable.MakeTimelineRecord()
	{
		TimelineRecord_DealDamageOnContact record = new TimelineRecord_DealDamageOnContact();
		record.cooldown = cooldown;
		record.damagePerHit = damagePerHit;
		record.lastAttackTime = lastAttackTime;
		return record;
	}

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_DealDamageOnContact rec = (TimelineRecord_DealDamageOnContact)record;
		cooldown = rec.cooldown;
		damagePerHit = rec.damagePerHit;
		lastAttackTime = rec.lastAttackTime;
	}

	private void Awake()
	{
		lastAttackTime = ConvertableTimeRecord.GetTime();
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		if (!GetComponent<Health>().IsAlive || ManipulableTime.IsTimeFrozen)
		{
			return;
		}
		Health otherHealth = collision.gameObject.GetComponent<Health>();
		if (
			otherHealth == null
			|| !otherHealth.IsAlive
			|| collision.gameObject.GetComponent<Collider2D>().isTrigger
			|| ManipulableTime.time - lastAttackTime.manipulableTime < cooldown
			|| otherHealth.isAlignedWithPlayer == GetComponent<Health>().isAlignedWithPlayer
		)
		{
			return;
		}
		HitInfo hit = new HitInfo();
		hit.damage = damagePerHit;
		hit.hitBy = collision.otherCollider;
		hit.hitCollider = collision.collider;
		otherHealth.Hit(hit);
		lastAttackTime.SetToCurrent();
	}

	public class TimelineRecord_DealDamageOnContact : TimelineRecordForComponent
	{
		public float cooldown;
		public int damagePerHit;
		public ConvertableTimeRecord lastAttackTime;
	}
}
