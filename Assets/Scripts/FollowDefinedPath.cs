using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Follow a path using a predefined set of points, which
 * loop back to the first point.</summary>
 */
public class FollowDefinedPath : MonoBehaviour
{
	public Vector3[] points = new Vector3[1];
	public float maxSpeed = 4.0f;
	public int targetPointIndex = 0;

	private void Update()
	{
		if (GetComponent<Health>().currentHealth <= 0 || Mathf.Approximately(0.0f, Time.timeScale))
		{
			return;
		}
		if (targetPointIndex >= points.Length)
		{
			targetPointIndex = 0;
		}
		if (Vector3.Distance(transform.position, points[targetPointIndex]) <= 0.25f)
		{
			targetPointIndex++;
			if (targetPointIndex >= points.Length)
			{
				targetPointIndex = 0;
			}
		}
		Vector3 newVelocity =
				Vector3.ClampMagnitude(
					(points[targetPointIndex] - transform.position).normalized * maxSpeed,
					maxSpeed
					);
		GetComponent<Rigidbody2D>().velocity = newVelocity;
	}
}
