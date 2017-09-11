using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Handles sword related stuff.</summary>*/
public class Sword : MonoBehaviour
{
	public float idleAngle;
	public float swingAngleStart;
	public float swingAngleEnd;
	public Transform swingTransform;
	public GameObject owner;
	public bool isSwinging { get; private set; }
	public bool isEndingSwing { get; private set; }

	private ConvertableTimeRecord swingStartTime;
	private float swingDuration;
	private float freezeDuration;

	private void Start()
	{
		swingStartTime = ConvertableTimeRecord.GetTime();
		swingTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, -160.0f);
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
				swingTransform.localRotation =
					Quaternion.Euler(
						Vector3.Lerp(
							new Vector3(0.0f, 0.0f, swingAngleStart),
							new Vector3(0.0f, 0.0f, swingAngleEnd),
							swingTime / swingDuration
							)
						);
			}
			else if (swingTime <= swingDuration + freezeDuration)
			{
				swingTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, swingAngleEnd);
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
		if (!GetComponent<Collider2D>().enabled)
		{
			Debug.Log("it is needed");
			return;
		}
		Health otherHealth = collision.GetComponent<Health>();
		if (otherHealth != null && otherHealth.isAlignedWithPlayer != owner.GetComponent<Health>().isAlignedWithPlayer)
		{
			HitInfo hit = new HitInfo();
			hit.damage = owner.GetComponent<PlayerMelee>().damagePerHit;
			hit.hitBy = GetComponent<Collider2D>();
			hit.hitCollider = collision;
			otherHealth.Hit(hit);
		}
	}

	public void Swing(float duration, float freezeDuration, float idleAngle, float startAngle, float endAngle)
	{
		if (ManipulableTime.ApplyingTimelineRecords || ManipulableTime.IsTimeFrozen || isSwinging)
		{
			return;
		}
		this.idleAngle = idleAngle;
		this.swingAngleStart = startAngle;
		this.swingAngleEnd = endAngle;
		isSwinging = true;
		isEndingSwing = false;
		swingStartTime.SetToCurrent();
		swingDuration = duration;
		this.freezeDuration = freezeDuration;
		swingTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, swingAngleStart);
		//GetComponent<SpriteRenderer>().enabled = true;
		GetComponent<Collider2D>().enabled = true;
	}

	public void CancelSwing()
	{
		isSwinging = false;
		isEndingSwing = false;
		swingTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, idleAngle);
		GetComponent<Collider2D>().enabled = false;
	}
}
