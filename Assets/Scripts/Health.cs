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
	public bool takeDamage = true;

	private void Start()
	{
		currentHealth = maxHealth;
	}

	public void DoDamage(int damage)
	{
		if (takeDamage)
		{
			currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
		}
	}
}
