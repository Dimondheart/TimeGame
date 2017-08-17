using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Base class for components that handle controlled motion
 * from the player or an AI.</summary>
 */
public abstract class ControlledMovement : MonoBehaviour
{
	public PhysicsMaterial2D stationaryMaterial;
	public PhysicsMaterial2D applyingMotionMaterial;

	private bool isApplyingMotion = false;
	
	public bool IsApplyingMotion
	{
		get
		{
			return isApplyingMotion;
		}
		protected set
		{
			isApplyingMotion = value;
			if (value)
			{
				GetComponent<Rigidbody2D>().sharedMaterial = applyingMotionMaterial;
			}
			else
			{
				GetComponent<Rigidbody2D>().sharedMaterial = stationaryMaterial;
			}
		}
	}
}
