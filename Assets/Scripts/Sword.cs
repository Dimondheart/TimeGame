using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Handles sword related stuff.</summary>*/
	public class Sword : RecordableMonoBehaviour<TimelineRecord_Sword>
	{
		public float idleAngle;
		public float swingAngleStart;
		public float swingAngleEnd;
		public Transform swingTransform;
		public GameObject owner;
		public bool isSwinging { get; private set; }
		public bool isEndingSwing { get; private set; }

		private ConvertableTime swingStartTime;
		private float swingDuration;
		private float freezeDuration;

		protected override void ApplyRecordedState(TimelineRecord_Sword record)
		{
			idleAngle = record.idleAngle;
			swingAngleStart = record.swingAngleStart;
			swingAngleEnd = record.swingAngleEnd;
			swingTransform = record.swingTransform;
			owner = record.owner;
			isSwinging = record.isSwinging;
			isEndingSwing = record.isEndingSwing;

			swingStartTime = record.swingStartTime;
			swingDuration = record.swingDuration;
			freezeDuration = record.freezeDuration;
		}

		protected override void WriteCurrentState(TimelineRecord_Sword record)
		{
			record.idleAngle = idleAngle;
			record.swingAngleStart = swingAngleStart;
			record.swingAngleEnd = swingAngleEnd;
			record.swingTransform = swingTransform;
			record.owner = owner;
			record.isSwinging = isSwinging;
			record.isEndingSwing = isEndingSwing;

			record.swingStartTime = swingStartTime;
			record.swingDuration = swingDuration;
			record.freezeDuration = freezeDuration;
		}

		private void Start()
		{
			swingStartTime = ConvertableTime.GetTime();
			swingTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, -160.0f);
		}

		protected override void FlowingUpdate()
		{
			if (!owner.GetComponent<Health>().IsAlive)
			{
				GetComponent<Collider2D>().enabled = false;
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
			if (ManipulableTime.IsTimeOrGamePaused || collision.isTrigger)
			{
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
			if (ManipulableTime.IsTimeOrGamePaused || isSwinging)
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
			GetComponent<Collider2D>().enabled = true;
		}

		public void CancelSwing()
		{
			if (ManipulableTime.IsTimeOrGamePaused)
			{
				return;
			}
			isSwinging = false;
			isEndingSwing = false;
			swingTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, idleAngle);
			GetComponent<Collider2D>().enabled = false;
		}
	}

	public class TimelineRecord_Sword : TimelineRecordForBehaviour
	{
		public float idleAngle;
		public float swingAngleStart;
		public float swingAngleEnd;
		public Transform swingTransform;
		public GameObject owner;
		public bool isSwinging;
		public bool isEndingSwing;

		public ConvertableTime swingStartTime;
		public float swingDuration;
		public float freezeDuration;
	}
}
