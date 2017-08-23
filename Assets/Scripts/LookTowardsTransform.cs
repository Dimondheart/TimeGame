using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Look towards a specified transform.</summary>*/
public class LookTowardsTransform : MonoBehaviour
{
	/**<summary>The transform to look towards.</summary>*/
	public Transform lookTowards;
	/**<summary>Max distance to look towards the transform.</summary>*/
	public float maxDistance = 6.0f;

	private void Update()
	{
		Vector2 lookVector = lookTowards.position - transform.position;
		if (lookVector.magnitude <= maxDistance)
		{
			GetComponent<DirectionLooking>().Direction = lookVector;
		}
	}
}
