using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Deal damage to opponents on contact.</summary>*/
	public class DealDamageOnContact : RecordableMonoBehaviour<TimelineRecord_DealDamageOnContact>
	{
		/**<summary>Delay between attacks, in seconds.</summary>*/
		public float cooldown = 0.25f;
		/**<summary>HP damage per attack.</summary>*/
		public int damagePerHit = 5;
		/**<summary>Time the last attack was made.</summary>*/
		private ConvertableTime lastAttackTime;

		protected override void WriteCurrentState(TimelineRecord_DealDamageOnContact record)
		{
			record.cooldown = cooldown;
			record.damagePerHit = damagePerHit;
			record.lastAttackTime = lastAttackTime;
		}

		protected override void ApplyRecordedState(TimelineRecord_DealDamageOnContact record)
		{
			cooldown = record.cooldown;
			damagePerHit = record.damagePerHit;
			lastAttackTime = record.lastAttackTime;
		}

		private void Awake()
		{
			lastAttackTime = ConvertableTime.GetTime();
		}

		private void OnCollisionStay2D(Collision2D collision)
		{
			if (
				ManipulableTime.IsTimeOrGamePaused
				|| !GetComponent<Health>().IsAlive
				|| collision.gameObject.GetComponent<Collider2D>().isTrigger
				|| ManipulableTime.time - lastAttackTime.manipulableTime < cooldown
			)
			{
				return;
			}
			Health otherHealth = collision.gameObject.GetComponent<Health>();
			if (
				otherHealth == null
				|| !otherHealth.IsAlive
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
	}

	public class TimelineRecord_DealDamageOnContact : TimelineRecordForBehaviour
	{
		public float cooldown;
		public int damagePerHit;
		public ConvertableTime lastAttackTime;
	}
}
