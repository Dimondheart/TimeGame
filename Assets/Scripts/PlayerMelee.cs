using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.DynamicInputSystem;
using TechnoWolf.TimeManipulation;
using System;

namespace TechnoWolf.Project1
{
	public class PlayerMelee : RecordableMonoBehaviour
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

		protected override void FlowingUpdate()
		{
			if (!GetComponent<Health>().IsAlive)
			{
				return;
			}
			if (GetComponent<SurfaceInteraction>().IsSwimming)
			{
				SetWeaponsEnabled(false);
			}
			else
			{
				SetLeftWeaponEnabled(false);
				SetRightWeaponEnabled(true);
				if (!GetComponent<PlayerMovementOld>().IsDashing)
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

		public sealed class TimelineRecord_PlayerMelee : TimelineRecordForBehaviour<PlayerMelee>
		{
			public float cooldown;
			public int damagePerHit;
			public float swingDuration;
			public float swordIdleAngle;
			public float swordSwingArc;
			public float comboLimit;
			public int currentSwingNumberRight;
			public int currentSwingNumberLeft;

			protected override void RecordState(PlayerMelee melee)
			{
				base.RecordState(melee);
				cooldown = melee.cooldown;
				damagePerHit = melee.damagePerHit;
				swingDuration = melee.swingDuration;
				swordIdleAngle = melee.swordIdleAngle;
				swordSwingArc = melee.swordSwingArc;
				comboLimit = melee.comboLimit;
				currentSwingNumberRight = melee.currentSwingNumberRight;
				currentSwingNumberLeft = melee.currentSwingNumberLeft;
			}

			protected override void ApplyRecord(PlayerMelee melee)
			{
				base.ApplyRecord(melee);
				melee.cooldown = cooldown;
				melee.damagePerHit = damagePerHit;
				melee.swingDuration = swingDuration;
				melee.swordIdleAngle = swordIdleAngle;
				melee.swordSwingArc = swordSwingArc;
				melee.comboLimit = comboLimit;
				melee.currentSwingNumberRight = currentSwingNumberRight;
				melee.currentSwingNumberLeft = currentSwingNumberLeft;
			}
		}
	}
}
