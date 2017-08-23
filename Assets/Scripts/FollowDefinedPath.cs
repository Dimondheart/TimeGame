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

	private void OnEnable()
	{
		float shortestDist = float.PositiveInfinity;
		float dist;
		// Find the nearest target
		for (int i = 0; i < targets.Length; i++)
		{
			dist = Vector3.Distance(transform.position, targets[i].position);
			if (dist < shortestDist)
			{
				shortestDist = dist;
				targetIndex = i;
			}
		}
	}

	private void Update()
	{
		if (GetComponent<Health>().health <= 0 || Mathf.Approximately(0.0f, Time.timeScale))
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
		GetComponent<DirectionLooking>().Direction = newVelocity;
	}
}
