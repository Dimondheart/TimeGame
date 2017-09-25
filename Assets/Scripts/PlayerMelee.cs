using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.DynamicInputSystem;
using TechnoWolf.TimeManipulation;
using System;

namespace TechnoWolf.Project1
{
	public class PlayerMelee : PlayerCombatModeUser
	{
		public Sword sword;
		/**<summary>Delay between attacks, in seconds.</summary>*/
		public float cooldown = 0.5f;
		/**<summary>HP damage per attack.</summary>*/
		public int damagePerHit = 10;
		public float swingDuration = 0.2f;
		public float swordIdleAngle = -160.0f;
		public float swordSwingArc = 160.0f;
		public float comboLimit = 3;
		private int currentSwingNumber;

		public bool IsSwinging
		{
			get
			{
				return sword.isSwinging || sword.isEndingSwing;
			}
		}

		public bool IsInCooldown
		{
			get
			{
				return !sword.isSwinging;
			}
		}

		public override TimelineRecordForComponent MakeTimelineRecord()
		{
			return new TimelineRecord_PlayerMelee();
		}

		public override void RecordCurrentState(TimelineRecordForComponent record)
		{
			base.RecordCurrentState(record);
			TimelineRecord_PlayerMelee rec = (TimelineRecord_PlayerMelee)record;
			rec.cooldown = cooldown;
			rec.damagePerHit = damagePerHit;
			rec.swingDuration = swingDuration;
		}

		public override void ApplyTimelineRecord(TimelineRecordForComponent record)
		{
			base.ApplyTimelineRecord(record);
			TimelineRecord_PlayerMelee rec = (TimelineRecord_PlayerMelee)record;
			cooldown = rec.cooldown;
			damagePerHit = rec.damagePerHit;
			swingDuration = rec.swingDuration;
		}

		protected override void ChangeToOffensive()
		{
			// TODO
		}

		protected override void ChangeToDefensive()
		{
			// TODO
		}

		protected override void ChangeToRanged()
		{
			// TODO
		}

		protected override void ChangeToUnarmed()
		{
			// TODO
		}

		protected override void Update()
		{
			base.Update();
			if (ManipulableTime.IsTimeOrGamePaused || !GetComponent<Health>().IsAlive)
			{
				return;
			}
			if (GetComponent<SurfaceInteraction>().IsSwimming)
			{
				StopSwinging();
			}
			else if (DynamicInput.GetButtonDown("Melee") && !GetComponent<PlayerMovement>().IsDashing)
			{
				if (!sword.isSwinging && !sword.isEndingSwing)
				{
					currentSwingNumber = 0;
				}
				if (sword.isEndingSwing || (!sword.isSwinging && !sword.isEndingSwing))
				{
					if (currentSwingNumber == 0)
					{
						sword.Swing(swingDuration, cooldown - swingDuration, swordIdleAngle, swordIdleAngle, swordSwingArc / 2.0f);
						currentSwingNumber++;
					}
					else if (currentSwingNumber < comboLimit)
					{
						float angle = swordSwingArc / 2.0f;
						if (currentSwingNumber % 2 != 0)
						{
							angle = -angle;
						}
						sword.Swing(swingDuration, cooldown - swingDuration, swordIdleAngle, -angle, angle);
						currentSwingNumber++;
					}
				}
			}
		}

		public void StopSwinging()
		{
			if (ManipulableTime.IsApplyingRecords || ManipulableTime.IsTimeOrGamePaused)
			{
				return;
			}
			if (sword.isSwinging || sword.isEndingSwing)
			{
				sword.CancelSwing();
			}
			currentSwingNumber = 0;
		}

		public class TimelineRecord_PlayerMelee : TimelineRecord_PlayerCombatModeUser
		{
			public float cooldown;
			public int damagePerHit;
			public float swingDuration;
		}
	}
}
