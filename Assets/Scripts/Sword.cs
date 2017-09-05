using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary></summary>*/
public class Sword : MonoBehaviour
{
	public GameObject owner;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.isTrigger)
		{
			return;
		}
		Health otherHealth = collision.GetComponent<Health>();
		if (otherHealth != null && otherHealth.isAlignedWithPlayer != owner.GetComponent<Health>().isAlignedWithPlayer)
		{
			HitInfo hit = new HitInfo();
			hit.damage = owner.GetComponent<PlayerMelee>().damagePerHit;
			hit.hitBy = GetComponent<Collider2D>();
			otherHealth.Hit(hit);
		}
	}
}
