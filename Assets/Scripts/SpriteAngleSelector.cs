using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Selcts which sprite to display based on which direction this thing
 * is currently facing.</summary>
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

	private void Update()
	{
		if (GetComponent<Health>().currentHealth <= 0 || Mathf.Approximately(0.0f, Time.timeScale))
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
