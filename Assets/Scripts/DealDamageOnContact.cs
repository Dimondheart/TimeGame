using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Deal damage to opponents on contact.</summary>*/
	public class DealDamageOnContact : RecordableMonoBehaviour
	{
		/**<summary>Delay between attacks, in seconds.</summary>*/
		public float cooldown = 0.25f;
		/**<summary>HP damage per attack.</summary>*/
		public int damagePerHit = 5;
		/**<summary>Time the last attack was made.</summary>*/
		private ConvertableTime lastAttackTime;

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

		public sealed class TimelineRecord_DealDamageOnContact : TimelineRecordForBehaviour<DealDamageOnContact>
		{
			public float cooldown;
			public int damagePerHit;
			public ConvertableTime lastAttackTime;

			protected override void WriteCurrentState(DealDamageOnContact ddc)
			{
				cooldown = ddc.cooldown;
				damagePerHit = ddc.damagePerHit;
				lastAttackTime = ddc.lastAttackTime;
			}

			protected override void ApplyRecordedState(DealDamageOnContact ddc)
			{
				ddc.cooldown = cooldown;
				ddc.damagePerHit = damagePerHit;
				ddc.lastAttackTime = lastAttackTime;
			}
		}
	}
}
