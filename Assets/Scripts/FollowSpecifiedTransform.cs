using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Move towards the specified transform.</summary>*/
public class FollowSpecifiedTransform : ControlledMovement
{
	public Transform followOverride;
	public float maxSpeed = 4.0f;
	public float velocityBlendRate = 100.0f;

	public Vector3 TargetPositon
	{
		get
		{
			if (followOverride == null && GetComponent<HostileTargetSelector>() != null)
			{
				if (GetComponent<HostileTargetSelector>().target == null)
				{
					return GetComponent<HostileTargetSelector>().targetLastSpotted;
				}
				return GetComponent<HostileTargetSelector>().target.transform.position;
			}
			return followOverride.position;
		}
	}

	private void Update()
	{
		if (TargetPositon == null
			|| GetComponent<Health>().currentHealth <= 0
			|| Mathf.Approximately(0.0f, Time.timeScale)
			|| Vector3.Distance(TargetPositon, transform.position) < 0.3f
			)
		{
			IsApplyingMotion = false;
			return;
		}
		Vector2 newVelocity =
				Vector3.ClampMagnitude(
					(TargetPositon - transform.position).normalized * maxSpeed,
					maxSpeed
					);
		float blendFactor = velocityBlendRate * Time.deltaTime;
		for (int c = 0; c < 2; c++)
		{
			newVelocity[c] = newVelocity[c] * blendFactor + GetComponent<Rigidbody2D>().velocity[c] * (1.0f - blendFactor);
		}
		newVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
		GetComponent<Rigidbody2D>().velocity = newVelocity;
		IsApplyingMotion = true;
	}
}
