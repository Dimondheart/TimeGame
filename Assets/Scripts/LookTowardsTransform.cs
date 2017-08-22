using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Look towards a specified transform. Currently will
 * affect the velocity of a unit if its rigidbody is not
 * locked in place.</summary>
 */
public class LookTowardsTransform : MonoBehaviour
{
	/**<summary>The transform to look towards.</summary>
	 */
	public Transform lookTowards;
	public float maxDistance = 6.0f;

	private void Update()
	{
		Vector2 lookVector = lookTowards.position - transform.position;
		if (lookVector.magnitude <= maxDistance)
		{
			GetComponent<Rigidbody2D>().velocity = lookVector;
		}
	}
}
