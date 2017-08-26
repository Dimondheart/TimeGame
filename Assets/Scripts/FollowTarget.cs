using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Move towards a target.</summary>*/
public class FollowTarget : ControlledMovement
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
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		if (ManipulableTime.IsTimeFrozen)
		{
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
			return;
		}
		else
		{
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		}
		if ((TargetTransform == null && targetLocationReached)
			|| GetComponent<Health>().health <= 0
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
		float blendFactor = velocityBlendRate * ManipulableTime.deltaTime;
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
