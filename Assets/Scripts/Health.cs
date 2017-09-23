using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
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
		private float absoluteMaxHP;
		/**<summary>Maximum HP, accounting for perminent damage.</summary>*/
		private float currentmaxHP = float.PositiveInfinity;
		/**<summary>Current HP.</summary>*/
		private float currentHP = float.PositiveInfinity;

		public float AbsoluteMaxHP
		{
			get
			{
				return absoluteMaxHP;
			}
			set
			{
				absoluteMaxHP = value;
				// Easy auto adjust of the 2 dependent values
				CurrentMaxHP = CurrentMaxHP;
			}
		}

		public float CurrentMaxHP
		{
			get
			{
				return currentmaxHP;
			}
			set
			{
				currentmaxHP = Mathf.Clamp(value, 0.0f, absoluteMaxHP);
				// Easy auto adjust
				CurrentHP = CurrentHP;
			}
		}

		public float CurrentHP
		{
			get
			{
				return currentHP;
			}
			private set
			{
				float val = Mathf.Clamp(value, 0.0f, currentmaxHP);
				if (val < 0.95f)
				{
					currentHP = 0.0f;
				}
				else
				{
					currentHP = val;
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
				return absoluteMaxHP;
			}
		}

		float IPrimaryValue.MaxCurrentValue
		{
			get
			{
				return currentmaxHP;
			}
		}

		float IPrimaryValue.CurrentValue
		{
			get
			{
				return currentHP;
			}
		}

		TimelineRecordForComponent ITimelineRecordable.MakeTimelineRecord()
		{
			return new TimelineRecord_Health();
		}

		void ITimelineRecordable.RecordCurrentState(TimelineRecordForComponent record)
		{
			TimelineRecord_Health rec = (TimelineRecord_Health)record;
			rec.absoluteMaxHP = absoluteMaxHP;
			rec.currentMaxHP = currentmaxHP;
			rec.currentHP = currentHP;
			rec.isAlignedWithPlayer = isAlignedWithPlayer;
		}

		void ITimelineRecordable.ApplyTimelineRecord(TimelineRecordForComponent record)
		{
			TimelineRecord_Health rec = (TimelineRecord_Health)record;
			absoluteMaxHP = rec.absoluteMaxHP;
			currentmaxHP = rec.currentMaxHP;
			currentHP = rec.currentHP;
			isAlignedWithPlayer = rec.isAlignedWithPlayer;
		}

		private void Awake()
		{
			absoluteMaxHP = initialMaxHealth;
			CurrentMaxHP = currentmaxHP;
			CurrentHP = currentmaxHP;
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
			CurrentHP -= damage;
			if (damage / CurrentMaxHP >= percentForPerminentDamage)
			{
				DoPerminentDamage(maxPerminentDamageHit);
			}
		}

		/**<summary>Hit this health and deal damage. Returns true if the hit
		 * was successful (was not negated by something.)</summary>
		 */
		public bool Hit(HitInfo hit)
		{
			if (ManipulableTime.ApplyingTimelineRecords)
			{
				return false;
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
				return true;
			}
			return false;
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
			public float absoluteMaxHP;
			public float currentMaxHP;
			public float currentHP;
			public bool isAlignedWithPlayer;
		}
	}
}
