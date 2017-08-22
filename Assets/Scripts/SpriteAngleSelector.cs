using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Selcts which sprite to display based on which direction this thing
 * is currently facing, and other factors.</summary>
 */
public class SpriteAngleSelector : MonoBehaviour
{
	private static readonly Quaternion upRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
	private static readonly Quaternion downRotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
	private static readonly Quaternion rightRotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
	private static readonly Quaternion leftRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
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
	/**<summary>Transforms that should be rotated to match
	 * the current sprite direction. A rotation of 0 is
	 * when the back sprite is being shown.</summary>
	 */
	public List<Transform> syncronizeRotations = new List<Transform>();

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
		float angle = GetComponent<DirectionLooking>().Angle;
		if (Mathf.Abs(angle) <= 45.0f)
		{
			SetSelectedRotation(0, GetComponent<Health>().currentHealth <= 0);
		}
		else if (Mathf.Abs(angle) >= 135.0f)
		{
			SetSelectedRotation(180, GetComponent<Health>().currentHealth <= 0);
		}
		else if (angle < 0.0f)
		{
			SetSelectedRotation(-90, GetComponent<Health>().currentHealth <= 0);
		}
		else
		{
			SetSelectedRotation(90, GetComponent<Health>().currentHealth <= 0);
		}
		/*
		float verticalAngle = Vector2.Angle(GetComponent<Rigidbody2D>().velocity, Vector2.up);
		if (verticalAngle <= 45.0f)
		{
			SetSelectedRotation(0, GetComponent<Health>().currentHealth <= 0);
		}
		else if (verticalAngle >= 135.0f)
		{
			SetSelectedRotation(180, GetComponent<Health>().currentHealth <= 0);
		}
		else
		{
			float horizontalAngle = Vector2.Angle(GetComponent<Rigidbody2D>().velocity, Vector2.right);
			if (horizontalAngle < 45.0f)
			{
				SetSelectedRotation(-90, GetComponent<Health>().currentHealth <= 0);
			}
			else if (horizontalAngle > 135.0f)
			{
				SetSelectedRotation(90, GetComponent<Health>().currentHealth <= 0);
			}
		}*/
	}

	private void SetSelectedRotation(int angle, bool useDeadVersion)
	{
		Quaternion synchRotation;
		Sprite selSprite;
		switch (angle)
		{
			// Back sprite
			case 0:
				synchRotation = upRotation;
				selSprite = useDeadVersion ? backDeadSprite : backSprite;
				break;
			// Left sprite
			case 90:
				synchRotation = leftRotation;
				selSprite = useDeadVersion ? leftDeadSprite : leftSprite;
				break;
			// Right sprite
			case -90:
				synchRotation = rightRotation;
				selSprite = useDeadVersion ? rightDeadSprite : rightSprite;
				break;
			// Front sprite
			case 180:
				synchRotation = downRotation;
				selSprite = useDeadVersion ? frontDeadSprite : frontSprite;
				break;
			default:
				Debug.LogWarning("Attempted to select a sprite with an unsupported angle:" + angle);
				return;
		}
		GetComponent<SpriteRenderer>().sprite = selSprite;
		foreach (Transform t in syncronizeRotations)
		{
			t.localRotation = synchRotation;
		}
	}
}
