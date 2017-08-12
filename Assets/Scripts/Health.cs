using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>HP tracking and releated data/functionality.</summary>*/
public class Health : MonoBehaviour
{
	/**<summary>Current HP</summary>*/
	public float currentHealth = 100;
	/**<summary>If the GameObject is friendly/neutral towards the player.</summary>*/
	public bool isAlignedWithPlayer = false;
}
