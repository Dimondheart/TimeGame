using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>HP tracking and releated data/functionality.</summary>*/
public class Health : MonoBehaviour
{
	/**<summary>Maximum HP.</summary>*/
	public int maxHealth;
	/**<summary>Current HP.</summary>*/
	public int currentHealth { get; private set; }
	/**<summary>If the GameObject is friendly/neutral towards the player.</summary>*/
	public bool isAlignedWithPlayer = false;
	/**<summary>If damage should be delt or ignored.</summary>*/
	public bool takeDamage = true;

	private void Start()
	{
		currentHealth = maxHealth;
	}

	/**<summary>Deal damage to this thing. Will not be applied if takeDamage
	 * is false.</summary>
	 * <param name="damage">The amount of damage to deal</param>
	 */
	public void DoDamage(int damage)
	{
		if (takeDamage)
		{
			currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
		}
	}
}
