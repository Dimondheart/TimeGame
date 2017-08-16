using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Move towards the specified transform.</summary>*/
public class FollowSpecifiedTransform : MonoBehaviour
{
	public Transform follow;
	public float maxSpeed = 4.0f;
	public float velocityBlendRate = 100.0f;

	private void Update()
	{
		if (GetComponent<Health>().currentHealth <= 0 || Mathf.Approximately(0.0f, Time.timeScale))
		{
			return;
		}
		Vector2 newVelocity =
				Vector3.ClampMagnitude(
					(follow.position - transform.position).normalized * maxSpeed,
					maxSpeed
					);
		float blendFactor = velocityBlendRate * Time.deltaTime;
		for (int c = 0; c < 2; c++)
		{
			newVelocity[c] = newVelocity[c] * blendFactor + GetComponent<Rigidbody2D>().velocity[c] * (1.0f - blendFactor);
		}
		newVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
		GetComponent<Rigidbody2D>().velocity = newVelocity;
	}
}
