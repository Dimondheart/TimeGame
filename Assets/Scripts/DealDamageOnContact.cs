using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Deal damage to opponents on contact.</summary>*/
public class DealDamageOnContact : MonoBehaviour
{
	/**<summary>Delay between attacks, in seconds.</summary>*/
	public float cooldown = 0.25f;
	/**<summary>HP damage per attack.</summary>*/
	public int damagePerHit = 5;
	/**<summary>Time the last attack was made.</summary>*/
	private float lastAttackTime = 0.0f;

	private void OnTriggerStay2D(Collider2D other)
	{
		if (GetComponent<Health>().health <= 0 || ManipulableTime.IsTimeFrozen)
		{
			return;
		}
		Health otherHealth = other.GetComponent<Health>();
		if (
			other.isTrigger
			|| otherHealth == null
			|| ManipulableTime.time - lastAttackTime < cooldown
			|| otherHealth.health <= 0
			|| otherHealth.isAlignedWithPlayer == GetComponent<Health>().isAlignedWithPlayer
		)
		{
			return;
		}
		otherHealth.DoDamage(damagePerHit);
		lastAttackTime = ManipulableTime.time;
	}
}
