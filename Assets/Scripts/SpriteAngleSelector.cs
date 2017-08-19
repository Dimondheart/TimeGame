using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Selcts which sprite to display based on which direction this thing
 * is currently facing, and other factors.</summary>
 */
public class SpriteAngleSelector : MonoBehaviour
{
	/**<summary>Sprite facing the camera.</summary>*/
	public Sprite frontSprite;
	/**<summary>Sprite facing away from the camera.</summary>*/
	public Sprite backSprite;
	/**<summary>Sprite facing to the left.</summary>*/
	public Sprite leftSprite;
	/**<summary>Sprite facing to the right.</summary>*/
	public Sprite rightSprite;
	/**<summary>Dead version of frontSprite.</summary>*/
	public Sprite frontDeadSprite;
	/**<summary>Dead version of backSprite.</summary>*/
	public Sprite backDeadSprite;
	/**<summary>Dead version of leftSprite.</summary>*/
	public Sprite leftDeadSprite;
	/**<summary>Dead version of rightSprite.</summary>*/
	public Sprite rightDeadSprite;

	private void Update()
	{
		if (Mathf.Approximately(0.0f, Time.timeScale))
		{
			return;
		}
		if (GetComponent<PlayerMovement>() != null && GetComponent<PlayerMovement>().freezeMovement)
		{
			return;
		}
		float verticalAngle = Vector2.Angle(GetComponent<Rigidbody2D>().velocity, Vector2.up);
		if (verticalAngle <= 45.0f)
		{
			if (GetComponent<Health>().currentHealth <= 0)
			{
				GetComponent<SpriteRenderer>().sprite = backDeadSprite;
			}
			else
			{
				GetComponent<SpriteRenderer>().sprite = backSprite;
			}
		}
		else if (verticalAngle >= 135.0f)
		{
			if (GetComponent<Health>().currentHealth <= 0)
			{
				GetComponent<SpriteRenderer>().sprite = frontDeadSprite;
			}
			else
			{
				GetComponent<SpriteRenderer>().sprite = frontSprite;
			}
		}
		else
		{
			float horizontalAngle = Vector2.Angle(GetComponent<Rigidbody2D>().velocity, Vector2.right);
			if (horizontalAngle < 45.0f)
			{
				if (GetComponent<Health>().currentHealth <= 0)
				{
					GetComponent<SpriteRenderer>().sprite = rightDeadSprite;
				}
				else
				{
					GetComponent<SpriteRenderer>().sprite = rightSprite;
				}
			}
			else if (horizontalAngle > 135.0f)
			{
				if (GetComponent<Health>().currentHealth <= 0)
				{
					GetComponent<SpriteRenderer>().sprite = leftDeadSprite;
				}
				else
				{
					GetComponent<SpriteRenderer>().sprite = leftSprite;
				}
			}
		}
	}
}
