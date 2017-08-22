using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Defines the direction something is facing without
 * applying a transform rotation.</summary>
 */
public class DirectionLooking : MonoBehaviour
{
	private Vector2 direction = Vector2.down;

	public Vector2 Direction
	{
		get
		{
			return direction;
		}
		set
		{
			direction = value;
			direction.Normalize();
		}
	}

	public float Angle
	{
		get
		{
			float angle = Vector2.Angle(Direction, Vector2.up);
			return Direction.x < 0 ? angle : -angle;
		}
	}
}
