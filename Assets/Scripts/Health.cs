﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>HP tracking and releated data/functionality.</summary>*/
public class Health : MonoBehaviour, IPrimaryValue, ITimelineRecordable
{
	/**<summary>Maximum HP.</summary>*/
	public int maxHealth;
	/**<summary>Current HP.</summary>*/
	public int health { get; private set; }
	/**<summary>If the GameObject is friendly/neutral towards the player.</summary>*/
	public bool isAlignedWithPlayer = false;
	/**<summary>If damage should be delt or ignored.</summary>*/
	public bool takeDamage = true;

	float IPrimaryValue.MaxValue
	{
		get
		{
			return maxHealth;
		}
	}

	float IPrimaryValue.CurrentValue
	{
		get
		{
			return health;
		}
	}

	TimelineRecord ITimelineRecordable.MakeTimelineRecord()
	{
		TimelineRecord_Health record = new TimelineRecord_Health();
		record.maxHealth = maxHealth;
		record.health = health;
		record.isAlignedWithPlayer = isAlignedWithPlayer;
		record.takeDamage = takeDamage;
		return record;
	}

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_Health rec = (TimelineRecord_Health)record;
		maxHealth = rec.maxHealth;
		health = rec.health;
		isAlignedWithPlayer = rec.isAlignedWithPlayer;
		takeDamage = rec.takeDamage;
	}

	private void Awake()
	{
		health = maxHealth;
	}

	/**<summary>Deal damage to this thing. Will not be applied if takeDamage
	 * is false.</summary>
	 * <param name="damage">The amount of damage to deal</param>
	 */
	public void DoDamage(int damage)
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		if (takeDamage)
		{
			health = Mathf.Clamp(health - damage, 0, maxHealth);
		}
	}

	public class TimelineRecord_Health : TimelineRecordForComponent
	{
		public int maxHealth;
		public int health;
		public bool isAlignedWithPlayer;
		public bool takeDamage;
	}
}
