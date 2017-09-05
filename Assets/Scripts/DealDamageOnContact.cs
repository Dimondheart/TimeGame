using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	private void OnTriggerStay2D(Collider2D other)
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		if (!GetComponent<Health>().IsAlive || ManipulableTime.IsTimeFrozen)
		{
			return;
		}
		Health otherHealth = other.GetComponent<Health>();
		if (
			other.isTrigger
			|| otherHealth == null
			|| ManipulableTime.time - lastAttackTime.manipulableTime < cooldown
			|| !otherHealth.IsAlive
			|| otherHealth.isAlignedWithPlayer == GetComponent<Health>().isAlignedWithPlayer
		)
		{
			return;
		}
		HitInfo hit = new HitInfo();
		//hit.moistureAlignment = GetComponent<ElementalAlignment>().Moisture;
		//hit.temperatureAlignment = GetComponent<ElementalAlignment>().Temperature;
		hit.damage = damagePerHit;
		Collider2D[] colliders = GetComponents<Collider2D>();
		if (colliders.Length > 2)
		{
			Debug.LogWarning("More than 3 colliders found");
		}
		foreach (Collider2D c in colliders)
		{
			if (c.isTrigger)
			{
				hit.hitBy = c;
				break;
			}
		}
		otherHealth.Hit(hit);
		//otherHealth.DoDamage(damagePerHit);
		lastAttackTime.SetToCurrent();
	}

	public class TimelineRecord_DealDamageOnContact : TimelineRecordForComponent
	{
		public float cooldown;
		public int damagePerHit;
		public ConvertableTimeRecord lastAttackTime;
	}
}
