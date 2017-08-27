using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour, ITimelineRecordable
{
	/**<summary>Color to change the player sprite to while melee is 
	 * active. Placeholder for animations.</summary>
	 */
	public Color colorDuringAction;
	/**<summary>Delay between attacks, in seconds.</summary>*/
	public float cooldown = 0.25f;
	/**<summary>HP damage per attack.</summary>*/
	public int damagePerHit = 5;
	/**<summary>Time the last attack was made.</summary>*/
	private ConvertableTimeRecord lastAttackTime;
	/**<summary>List of things that are hit when melee is active.</summary>*/
	private List<Collider2D> attackable = new List<Collider2D>();

	TimelineRecord ITimelineRecordable.MakeTimelineRecord()
	{
		TimelineRecord_PlayerMelee record = new TimelineRecord_PlayerMelee();
		record.colorDuringAction = colorDuringAction;
		record.cooldown = cooldown;
		record.damagePerHit = damagePerHit;
		record.lastAttackTime = lastAttackTime;
		record.attackable = attackable.ToArray();
		return record;
	}

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_PlayerMelee rec = (TimelineRecord_PlayerMelee)record;
		colorDuringAction = rec.colorDuringAction;
		cooldown = rec.cooldown;
		damagePerHit = rec.damagePerHit;
		lastAttackTime = rec.lastAttackTime;
		attackable.Clear();
		attackable.AddRange(rec.attackable);
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
		if (DynamicInput.GetButton("Melee") && GetComponent<Health>().health > 0)
		{
			GetComponent<SpriteColorChanger>().SpriteColor = colorDuringAction;
			if (ManipulableTime.time - lastAttackTime.manipulableTime >= cooldown)
			{
				bool atLeastOneAttacked = false;
				foreach (Collider2D col2D in attackable)
				{
					Health otherHealth = col2D.GetComponent<Health>();

					if (
						otherHealth.health > 0
						&& otherHealth.isAlignedWithPlayer != GetComponent<Health>().isAlignedWithPlayer
					)
					{
						otherHealth.DoDamage(damagePerHit);
						atLeastOneAttacked = true;
					}
				}
				if (atLeastOneAttacked)
				{
					lastAttackTime.SetToCurrent();
				}
			}
		}
		else if (!DynamicInput.GetButton("Guard"))
		{
			GetComponent<SpriteColorChanger>().SpriteColor = Color.white;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		Health otherHealth = collision.gameObject.GetComponent<Health>();
		if (collision.isTrigger || otherHealth == null)
		{
			attackable.Remove(collision);
		}
		else if (!attackable.Contains(collision))
		{
			attackable.Add(collision);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		attackable.Remove(collision);
	}

	public class TimelineRecord_PlayerMelee : TimelineRecord
	{
		public Color colorDuringAction;
		public float cooldown;
		public int damagePerHit;
		public ConvertableTimeRecord lastAttackTime;
		public Collider2D[] attackable;
	}
}
