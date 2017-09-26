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
		public Sword rightHandWeapon;
		public Sword leftHandWeapon;
		/**<summary>Delay between attacks, in seconds.</summary>*/
		public float cooldown = 0.5f;
		/**<summary>HP damage per attack.</summary>*/
		public int damagePerHit = 10;
		public float swingDuration = 0.2f;
		public float swordIdleAngle = -160.0f;
		public float swordSwingArc = 160.0f;
		public float comboLimit = 3;
		private int currentSwingNumberRight;
		private int currentSwingNumberLeft;

		public bool IsSwinging
		{
			get
			{
				return rightHandWeapon.isSwinging || rightHandWeapon.isEndingSwing || leftHandWeapon.isSwinging || leftHandWeapon.isEndingSwing;
			}
		}

		public bool IsInCooldown
		{
			get
			{
				return rightHandWeapon.isEndingSwing || leftHandWeapon.isEndingSwing;
			}
		}

		public bool RightWeaponEnabled
		{
			get
			{
				return rightHandWeapon.GetComponent<SpriteRenderer>().enabled;
			}
		}

		public bool LeftWeaponEnabled
		{
			get
			{
				return leftHandWeapon.GetComponent<SpriteRenderer>().enabled;
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
			rec.swordIdleAngle = swordIdleAngle;
			rec.swordSwingArc = swordSwingArc;
			rec.comboLimit = comboLimit;
			rec.currentSwingNumberRight = currentSwingNumberRight;
			rec.currentSwingNumberLeft = currentSwingNumberLeft;
		}

		public override void ApplyTimelineRecord(TimelineRecordForComponent record)
		{
			base.ApplyTimelineRecord(record);
			TimelineRecord_PlayerMelee rec = (TimelineRecord_PlayerMelee)record;
			cooldown = rec.cooldown;
			damagePerHit = rec.damagePerHit;
			swingDuration = rec.swingDuration;
			swordIdleAngle = rec.swordIdleAngle;
			swordSwingArc = rec.swordSwingArc;
			comboLimit = rec.comboLimit;
			currentSwingNumberRight = rec.currentSwingNumberRight;
			currentSwingNumberLeft = rec.currentSwingNumberLeft;
		}

		protected override void ChangeToOffensive()
		{
			SetWeaponsEnabled(true);
		}

		protected override void ChangeToDefensive()
		{
			SetRightWeaponEnabled(true);
			SetLeftWeaponEnabled(false);
		}

		protected override void ChangeToRanged()
		{
			SetWeaponsEnabled(false);
		}

		protected override void ChangeToUnarmed()
		{
			SetWeaponsEnabled(false);
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
				SetWeaponsEnabled(false);
			}
			else
			{
				SetLeftWeaponEnabled(currentCombatMode == PlayerCombatMode.CombatMode.Offensive);
				SetRightWeaponEnabled(currentCombatMode == PlayerCombatMode.CombatMode.Offensive || currentCombatMode == PlayerCombatMode.CombatMode.Defensive);
				if (!GetComponent<PlayerMovement>().IsDashing)
				{
					if (DynamicInput.GetButtonDown("Right Hand Action") && RightWeaponEnabled)
					{
						if (!rightHandWeapon.isSwinging && !rightHandWeapon.isEndingSwing)
						{
							currentSwingNumberRight = 0;
						}
						if (rightHandWeapon.isEndingSwing || (!rightHandWeapon.isSwinging && !rightHandWeapon.isEndingSwing))
						{
							if (currentSwingNumberRight == 0)
							{
								rightHandWeapon.Swing(swingDuration, cooldown - swingDuration, swordIdleAngle, swordIdleAngle, swordSwingArc / 2.0f);
								currentSwingNumberRight++;
							}
							else if (currentSwingNumberRight < comboLimit)
							{
								float angle = swordSwingArc / 2.0f;
								if (currentSwingNumberRight % 2 != 0)
								{
									angle = -angle;
								}
								rightHandWeapon.Swing(swingDuration, cooldown - swingDuration, swordIdleAngle, -angle, angle);
								currentSwingNumberRight++;
							}
						}
					}
					if (DynamicInput.GetButtonDown("Left Hand Action") && LeftWeaponEnabled)
					{
						if (!leftHandWeapon.isSwinging && !leftHandWeapon.isEndingSwing)
						{
							currentSwingNumberLeft = 0;
						}
						if (leftHandWeapon.isEndingSwing || (!leftHandWeapon.isSwinging && !leftHandWeapon.isEndingSwing))
						{
							if (currentSwingNumberLeft == 0)
							{
								leftHandWeapon.Swing(swingDuration, cooldown - swingDuration, swordIdleAngle, swordIdleAngle, swordSwingArc / 2.0f);
								currentSwingNumberLeft++;
							}
							else if (currentSwingNumberLeft < comboLimit)
							{
								float angle = swordSwingArc / 2.0f;
								if (currentSwingNumberLeft % 2 != 0)
								{
									angle = -angle;
								}
								leftHandWeapon.Swing(swingDuration, cooldown - swingDuration, swordIdleAngle, -angle, angle);
								currentSwingNumberLeft++;
							}
						}
					}
				}
			}
		}

		/**<summary>Stop swinging both weapons.</summary>*/
		public void StopSwinging()
		{
			StopRightSwinging();
			StopLeftSwinging();
		}

		public void StopRightSwinging()
		{
			if (rightHandWeapon.isSwinging || rightHandWeapon.isEndingSwing)
			{
				rightHandWeapon.CancelSwing();
			}
			currentSwingNumberRight = 0;
		}

		public void StopLeftSwinging()
		{
			if (leftHandWeapon.isSwinging || leftHandWeapon.isEndingSwing)
			{
				leftHandWeapon.CancelSwing();
			}
			currentSwingNumberLeft = 0;
		}

		private void SetWeaponsEnabled(bool enabled)
		{
			SetRightWeaponEnabled(enabled);
			SetLeftWeaponEnabled(enabled);
		}

		private void SetRightWeaponEnabled(bool enabled)
		{
			if (enabled == RightWeaponEnabled)
			{
				return;
			}
			StopRightSwinging();
			rightHandWeapon.GetComponent<SpriteRenderer>().enabled = enabled;
			rightHandWeapon.GetComponent<Collider2D>().enabled = false;
		}

		private void SetLeftWeaponEnabled(bool enabled)
		{
			if (enabled == LeftWeaponEnabled)
			{
				return;
			}
			StopLeftSwinging();
			leftHandWeapon.GetComponent<SpriteRenderer>().enabled = enabled;
			leftHandWeapon.GetComponent<Collider2D>().enabled = false;
		}

		public class TimelineRecord_PlayerMelee : TimelineRecord_PlayerCombatModeUser
		{
			public float cooldown;
			public int damagePerHit;
			public float swingDuration;
			public float swordIdleAngle;
			public float swordSwingArc;
			public float comboLimit;
			public int currentSwingNumberRight;
			public int currentSwingNumberLeft;
		}
	}
}
