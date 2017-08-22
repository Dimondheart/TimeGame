using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Follow a path using a predefined set of points, which
 * loop back to the first point.</summary>
 */
public class FollowDefinedPath : ControlledMovement
{
	public Transform[] targets;
	public float maxSpeed = 4.0f;
	public int targetIndex = 0;

	private void Update()
	{
		if (GetComponent<Health>().currentHealth <= 0 || Mathf.Approximately(0.0f, Time.timeScale)
			)
		{
			IsApplyingMotion = false;
			return;
		}
		if (targetIndex >= targets.Length)
		{
			targetIndex = 0;
		}
		if (Vector3.Distance(transform.position, targets[targetIndex].position) <= 0.25f)
		{
			targetIndex++;
			if (targetIndex >= targets.Length)
			{
				targetIndex = 0;
			}
		}
		Vector3 newVelocity =
				Vector3.ClampMagnitude(
					(targets[targetIndex].position - transform.position).normalized * maxSpeed,
					maxSpeed
					);
		GetComponent<Rigidbody2D>().velocity = newVelocity;
		IsApplyingMotion = true;
	}
}
