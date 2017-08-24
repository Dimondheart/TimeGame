using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
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

	private void Awake()
	{
		lastAttackTime = ConvertableTimeRecord.GetTime();
	}

	private void Update()
	{
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
		attackable.Remove(collision);
	}
}
