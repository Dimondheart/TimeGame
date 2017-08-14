using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAngleSelector : MonoBehaviour
{
	public Sprite frontSprite;
	public Sprite backSprite;
	public Sprite leftSprite;
	public Sprite rightSprite;

	private void Update()
	{
		float verticalAngle = Vector2.Angle(GetComponent<Rigidbody2D>().velocity, Vector2.up);
		if (verticalAngle <= 45.0f)
		{
			GetComponent<SpriteRenderer>().sprite = backSprite;
		}
		else if (verticalAngle >= 135.0f)
		{
			GetComponent<SpriteRenderer>().sprite = frontSprite;
		}
		else
		{
			float horizontalAngle = Vector2.Angle(GetComponent<Rigidbody2D>().velocity, Vector2.right);
			if (horizontalAngle < 45.0f)
			{
				GetComponent<SpriteRenderer>().sprite = rightSprite;
			}
			else if (horizontalAngle > 135.0f)
			{
				GetComponent<SpriteRenderer>().sprite = leftSprite;
			}
		}
	}
}
