using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Handles sword related stuff.</summary>*/
public class Sword : MonoBehaviour
{
	public float swingAngleStart;
	public float swingAngleEnd;
	public Transform swingTransform;
	public GameObject owner;
	public bool isSwinging { get; private set; }
	public bool isEndingSwing { get; private set; }

	private ConvertableTimeRecord swingStartTime;
	private float swingDuration;
	private float freezeDuration;
	private bool reverseDirection;

	private void Start()
	{
		swingStartTime = ConvertableTimeRecord.GetTime();
	}

	private void Update()
	{
		if (ManipulableTime.ApplyingTimelineRecords || ManipulableTime.IsTimeFrozen)
		{
			return;
		}
		float swingTime = ManipulableTime.time - swingStartTime.manipulableTime;
		if (isSwinging || isEndingSwing)
		{
			if (swingTime <= swingDuration)
			{
				if (reverseDirection)
				{
					swingTransform.localRotation =
						Quaternion.Lerp(
							Quaternion.Euler(0.0f, 0.0f, swingAngleEnd),
							Quaternion.Euler(0.0f, 0.0f, swingAngleStart),
							swingTime / swingDuration
							);
				}
				else
				{
					swingTransform.localRotation =
						Quaternion.Lerp(
							Quaternion.Euler(0.0f, 0.0f, swingAngleStart),
							Quaternion.Euler(0.0f, 0.0f, swingAngleEnd),
							swingTime / swingDuration
							);
				}
			}
			else if (swingTime <= swingDuration + freezeDuration)
			{
				if (reverseDirection)
				{
					swingTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, swingAngleStart);
				}
				else
				{
					swingTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, swingAngleEnd);
				}
				isEndingSwing = true;
				isSwinging = false;
				GetComponent<Collider2D>().enabled = false;
			}
			else
			{
				CancelSwing();
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (ManipulableTime.ApplyingTimelineRecords || ManipulableTime.IsTimeFrozen || collision.isTrigger)
		{
			return;
		}
		Health otherHealth = collision.GetComponent<Health>();
		if (otherHealth != null && otherHealth.isAlignedWithPlayer != owner.GetComponent<Health>().isAlignedWithPlayer)
		{
			HitInfo hit = new HitInfo();
			hit.damage = owner.GetComponent<PlayerMelee>().damagePerHit;
			hit.hitBy = GetComponent<Collider2D>();
			otherHealth.Hit(hit);
		}
	}

	public void Swing(float duration, float freezeDuration)
	{
		if (ManipulableTime.ApplyingTimelineRecords || ManipulableTime.IsTimeFrozen || isSwinging)
		{
			return;
		}
		if (isEndingSwing)
		{
			reverseDirection = !reverseDirection;
		}
		isSwinging = true;
		isEndingSwing = false;
		swingStartTime.SetToCurrent();
		swingDuration = duration;
		this.freezeDuration = freezeDuration;
		if (reverseDirection)
		{
			swingTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, swingAngleEnd);
		}
		else
		{
			swingTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, swingAngleStart);
		}
		GetComponent<SpriteRenderer>().enabled = true;
		GetComponent<Collider2D>().enabled = true;
	}

	public void CancelSwing()
	{
		isSwinging = false;
		isEndingSwing = false;
		reverseDirection = false;
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Collider2D>().enabled = false;
	}
}
