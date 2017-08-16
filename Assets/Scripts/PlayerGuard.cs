using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuard : MonoBehaviour
{
	/**<summary>Color to make the player sprite while guarding, a placeholder
	 * for animations.</summary>
	 */
	public Color colorDuringAction;

	private void Update()
	{

		if (Mathf.Approximately(0.0f, Time.timeScale))
		{
			return;
		}
		if (DynamicInput.GetButton("Guard"))
		{
			GetComponent<SpriteColorChanger>().SpriteColor = colorDuringAction;
			GetComponent<Health>().takeDamage = false;
		}
		else
		{
			GetComponent<Health>().takeDamage = true;
			if (!DynamicInput.GetButton("Melee"))
			{
				GetComponent<SpriteColorChanger>().SpriteColor = Color.white;
			}
		}
	}
}
