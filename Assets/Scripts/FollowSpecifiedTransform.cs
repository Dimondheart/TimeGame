using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Move towards the specified transform.</summary>*/
public class FollowSpecifiedTransform : ControlledMovement
{
	public float maxSpeed = 4.0f;
	public float velocityBlendRate = 100.0f;
	public bool targetLocationReached { get; private set; }

	public Transform TargetTransform
	{
		get
		{
			if (GetComponent<HostileTargetSelector>() == null
				|| GetComponent<HostileTargetSelector>().target == null
				)
			{
				return null;
			}
			return GetComponent<HostileTargetSelector>().target.transform;
		}
	}

	public Vector3 TargetPositon
	{
		get
		{
			if (TargetTransform == null)
			{
				if (GetComponent<HostileTargetSelector>() == null)
				{
					return new Vector3(float.MaxValue, 0.0f, 0.0f);
				}
				return GetComponent<HostileTargetSelector>().targetLastSpotted;
			}
			return TargetTransform.position;
		}
	}

	private void Start()
	{
		targetLocationReached = true;
	}

	private void Update()
	{
		if ((TargetTransform == null && targetLocationReached)
			|| GetComponent<Health>().currentHealth <= 0
			|| Mathf.Approximately(0.0f, Time.timeScale)
			|| Vector3.Distance(TargetPositon, transform.position) < 0.3f
			)
		{
			IsApplyingMotion = false;
			targetLocationReached = true;
			if (GetComponent<FollowDefinedPath>() != null)
			{
				GetComponent<FollowDefinedPath>().enabled = true;
			}
			return;
		}
		if (GetComponent<FollowDefinedPath>() != null)
		{
			GetComponent<FollowDefinedPath>().enabled = false;
		}
		targetLocationReached = false;
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
		GetComponent<DirectionLooking>().Direction = newVelocity;
	}
}
