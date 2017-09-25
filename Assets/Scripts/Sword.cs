using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Handles sword related stuff.</summary>*/
	public class Sword : MonoBehaviour, ITimelineRecordable
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

		void ITimelineRecordable.ApplyTimelineRecord(TimelineRecordForComponent record)
		{
			TimelineRecord_Sword rec = (TimelineRecord_Sword)record;
			idleAngle = rec.idleAngle;
			swingAngleStart = rec.swingAngleStart;
			swingAngleEnd = rec.swingAngleEnd;
			swingTransform = rec.swingTransform;
			owner = rec.owner;
			isSwinging = rec.isSwinging;
			isEndingSwing = rec.isEndingSwing;

			swingStartTime = rec.swingStartTime;
			swingDuration = rec.swingDuration;
			freezeDuration = rec.freezeDuration;
		}

		void ITimelineRecordable.RecordCurrentState(TimelineRecordForComponent record)
		{
			TimelineRecord_Sword rec = (TimelineRecord_Sword)record;
			rec.idleAngle = idleAngle;
			rec.swingAngleStart = swingAngleStart;
			rec.swingAngleEnd = swingAngleEnd;
			rec.swingTransform = swingTransform;
			rec.owner = owner;
			rec.isSwinging = isSwinging;
			rec.isEndingSwing = isEndingSwing;

			rec.swingStartTime = swingStartTime;
			rec.swingDuration = swingDuration;
			rec.freezeDuration = freezeDuration;
		}

		TimelineRecordForComponent ITimelineRecordable.MakeTimelineRecord()
		{
			return new TimelineRecord_Sword();
		}

		private void Start()
		{
			swingStartTime = ConvertableTimeRecord.GetTime();
			swingTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, -160.0f);
		}

		private void Update()
		{
			if (ManipulableTime.IsApplyingRecords || ManipulableTime.IsTimeOrGamePaused)
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
			if (ManipulableTime.IsApplyingRecords || ManipulableTime.IsTimeOrGamePaused || collision.isTrigger)
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
			if (ManipulableTime.IsApplyingRecords || ManipulableTime.IsTimeOrGamePaused || isSwinging)
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
			if (ManipulableTime.IsApplyingRecords || ManipulableTime.IsTimeOrGamePaused)
			{
				return;
			}
			isSwinging = false;
			isEndingSwing = false;
			swingTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, idleAngle);
			GetComponent<Collider2D>().enabled = false;
		}

		public class TimelineRecord_Sword : TimelineRecordForComponent
		{
			public float idleAngle;
			public float swingAngleStart;
			public float swingAngleEnd;
			public Transform swingTransform;
			public GameObject owner;
			public bool isSwinging;
			public bool isEndingSwing;

			public ConvertableTimeRecord swingStartTime;
			public float swingDuration;
			public float freezeDuration;
		}
	}
}
