using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>HP tracking and releated data/functionality.</summary>*/
public class Health : MonoBehaviour, IPrimaryValue, ITimelineRecordable
{
	/**<summary>Minimum percent of current max health for a normal damage hit
	 * to deal perminent damage.</summary>
	 */
	public static readonly float percentForPerminentDamage = 0.05f;
	/**<summary>Max amount of damage that can be done to the current max
	 * health per normal damage hit.</summary>
	 */
	public static readonly float maxPerminentDamageHit = 1.0f;
	private static int CompareIHitTakersByPriority(IHitTaker a, IHitTaker b)
	{
		return a.Priority.CompareTo(b.Priority);
	}

	/**<summary>Set the health values to this when starting a new game.</summary>*/
	public float initialMaxHealth = 100.0f;
	/**<summary>If the GameObject is friendly/neutral towards the player.</summary>*/
	public bool isAlignedWithPlayer = false;
	/**<summary>The absolute max HP.</summary>*/
	private float absoluteMaxHealth;
	/**<summary>Maximum HP, accounting for perminent damage.</summary>*/
	private float maxHealth = float.PositiveInfinity;
	/**<summary>Current HP.</summary>*/
	private float health = float.PositiveInfinity;

	public float AbsoluteMaxHP
	{
		get
		{
			return absoluteMaxHealth;
		}
		set
		{
			absoluteMaxHealth = value;
			// Easy auto adjust of the 2 dependent values
			CurrentMaxHP = CurrentMaxHP;
		}
	}

	public float CurrentMaxHP
	{
		get
		{
			return maxHealth;
		}
		set
		{
			maxHealth = Mathf.Clamp(value, 0.0f, absoluteMaxHealth);
			// Easy auto adjust
			CurrentHP = CurrentHP;
		}
	}

	public float CurrentHP
	{
		get
		{
			return health;
		}
		private set
		{
			float val = Mathf.Clamp(value, 0.0f, maxHealth);
			if (val < 0.95f)
			{
				health = 0.0f;
			}
			else
			{
				health = val;
			}
		}
	}

	public bool IsAlive
	{
		get
		{
			return CurrentHP > 0.0f;
		}
	}

	float IPrimaryValue.MaxValue
	{
		get
		{
			return absoluteMaxHealth;
		}
	}

	float IPrimaryValue.MaxCurrentValue
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
		record.absoluteMaxHealth = absoluteMaxHealth;
		record.maxHealth = maxHealth;
		record.health = health;
		record.isAlignedWithPlayer = isAlignedWithPlayer;
		return record;
	}

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_Health rec = (TimelineRecord_Health)record;
		absoluteMaxHealth = rec.absoluteMaxHealth;
		maxHealth = rec.maxHealth;
		health = rec.health;
		isAlignedWithPlayer = rec.isAlignedWithPlayer;
	}

	private void Awake()
	{
		absoluteMaxHealth = initialMaxHealth;
		CurrentMaxHP = maxHealth;
		CurrentHP = maxHealth;
	}

	private void Update()
	{
		if (ManipulableTime.ApplyingTimelineRecords || ManipulableTime.IsTimeFrozen || !IsAlive)
		{
			return;
		}
		CurrentHP += 10.0f
			* ManipulableTime.deltaTime
			* (Mathf.Approximately(0.0f, GetComponent<Rigidbody2D>().velocity.magnitude) ? 1.0f : 0.5f);
	}

	/**<summary>Deal damage to this thing. Will not be applied if takeDamage
	 * is false.</summary>
	 * <param name="damage">The amount of damage to deal</param>
	 */
	private void DoDamage(float damage)
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		Debug.Log("Doing damage:" + damage + " (to:" + gameObject.name);
		CurrentHP -= damage;
		if (damage / CurrentMaxHP >= percentForPerminentDamage)
		{
			DoPerminentDamage(maxPerminentDamageHit);
		}
	}

	public void Hit(HitInfo hit)
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		List<IHitTaker> hitTakers = new List<IHitTaker>(GetComponents<IHitTaker>());
		hitTakers.Sort(CompareIHitTakersByPriority);
		bool hitNullified = false;
		if (hitTakers.Count > 0)
		{
			foreach (IHitTaker ht in hitTakers)
			{
				if (ht.TakeHit(hit))
				{
					hitNullified = true;
					break;
				}
			}
		}
		if (!hitNullified)
		{
			DoDamage(hit.damage);
			DoPerminentDamage(hit.permanentDamage);
		}
	}

	private void DoPerminentDamage(float damage)
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		CurrentMaxHP -= damage;
	}

	public class TimelineRecord_Health : TimelineRecordForComponent
	{
		public float absoluteMaxHealth;
		public float maxHealth;
		public float health;
		public bool isAlignedWithPlayer;
	}
}
