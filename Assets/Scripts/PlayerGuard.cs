using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuard : MonoBehaviour
{
	public Color colorDuringAction;

	private void Update()
	{

		if (Mathf.Approximately(0.0f, Time.timeScale))
		{
			return;
		}
		if (Input.GetButton("Guard"))
		{
			GetComponent<SpriteColorChanger>().SpriteColor = colorDuringAction;
			GetComponent<Health>().takeDamage = false;
		}
		else
		{
			GetComponent<Health>().takeDamage = true;
			if (!Input.GetButton("Melee"))
			{
				GetComponent<SpriteColorChanger>().SpriteColor = Color.white;
			}
		}
	}
}
