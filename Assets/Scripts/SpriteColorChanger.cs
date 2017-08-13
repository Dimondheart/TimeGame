using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteColorChanger : MonoBehaviour
{
	public Color SpriteColor
	{
		get
		{
			return GetComponent<SpriteRenderer>().color;
		}
		set
		{
			GetComponent<SpriteRenderer>().color = value;
		}
	}
}
