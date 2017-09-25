using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.DynamicInputSystem;
using TechnoWolf.TimeManipulation;
using System;

namespace TechnoWolf.Project1
{
	/**<summary>Player controlled guarding.</summary>*/
	public class PlayerGuard : PlayerCombatModeUser, IHitTaker
	{
		public GameObject shield;
		public GameObject sideShieldLeft;
		public GameObject sideShieldRight;

		/**<summary>If the player has the front guard ability active (does not include passive
		 * shields like the side shields.)</summary>
		 */
		public bool IsGuarding
		{
			get
			{
				return shield.GetComponent<SpriteRenderer>().enabled;
			}
		}

		public override TimelineRecordForComponent MakeTimelineRecord()
		{
			return new TimelineRecord_PlayerGuard();
		}

		public override void RecordCurrentState(TimelineRecordForComponent record)
		{
			base.RecordCurrentState(record);
			TimelineRecord_PlayerGuard rec = (TimelineRecord_PlayerGuard)record;
			rec.shield = shield;
			rec.sideShieldLeft = sideShieldLeft;
			rec.sideShieldRight = sideShieldRight;
		}

		public override void ApplyTimelineRecord(TimelineRecordForComponent record)
		{
			base.ApplyTimelineRecord(record);
			TimelineRecord_PlayerGuard rec = (TimelineRecord_PlayerGuard)record;
			shield = rec.shield;
			sideShieldLeft = rec.sideShieldLeft;
			sideShieldRight = rec.sideShieldRight;
		}

		bool IHitTaker.TakeHit(HitInfo hit)
		{
			if (hit.hitCollider == shield.GetComponent<Collider2D>())
			{
				return !shield.GetComponent<Collider2D>().isTrigger;
			}
			else if (hit.hitCollider == sideShieldLeft.GetComponent<Collider2D>())
			{
				return !sideShieldLeft.GetComponent<Collider2D>().isTrigger;
			}
			else if (hit.hitCollider == sideShieldRight.GetComponent<Collider2D>())
			{
				return !sideShieldRight.GetComponent<Collider2D>().isTrigger;
			}
			return false;
		}

		int IHitTaker.Priority
		{
			get
			{
				return 100;
			}
		}

		protected override void ChangeToOffensive()
		{
			SetSideShieldLeftEnabled(false);
			SetSideShieldRightEnabled(false);
			shield.transform.localScale = new Vector3(0.75f, 0.5f, 1.0f);
			shield.transform.localPosition = new Vector3(0.0f, 0.355f, 0.0f);
		}

		protected override void ChangeToDefensive()
		{
			SetSideShieldLeftEnabled(true);
			SetSideShieldRightEnabled(true);
			shield.transform.localScale = Vector3.one;
			shield.transform.localPosition = new Vector3(0.0f, 0.335f, 0.0f);
		}

		protected override void ChangeToRanged()
		{
			SetSideShieldLeftEnabled(true);
			SetSideShieldRightEnabled(true);
			shield.transform.localScale = new Vector3(0.75f, 0.5f, 1.0f);
			shield.transform.localPosition = new Vector3(0.0f, 0.355f, 0.0f);
		}

		protected override void ChangeToUnarmed()
		{
			SetSideShieldLeftEnabled(false);
			SetSideShieldRightEnabled(false);
		}

		protected override void Update()
		{
			base.Update();
			if (ManipulableTime.IsTimeOrGamePaused)
			{
				return;
			}
			if (GetComponent<SurfaceInteraction>().IsSwimming || currentCombatMode == PlayerCombatMode.CombatMode.Unarmed)
			{
				SetGuardEnabled(false);
			}
			else
			{
				if (DynamicInput.GetButtonDown("Guard"))
				{
					if (!GetComponent<PlayerMovement>().IsDashing)
					{
						GetComponent<PlayerMelee>().StopSwinging();
						SetGuardEnabled(true);
					}
					else
					{
						SetGuardEnabled(false);
					}
				}
				else if (DynamicInput.GetButtonHeld("Guard"))
				{
					if (!GetComponent<PlayerMovement>().IsDashing && GetComponent<PlayerMelee>().IsInCooldown)
					{
						GetComponent<PlayerMelee>().StopSwinging();
						SetGuardEnabled(true);
					}
					else
					{
						SetGuardEnabled(false);
					}
				}
				else
				{
					SetGuardEnabled(false);
				}
			}
		}

		private void SetGuardEnabled(bool enabled)
		{
			shield.GetComponent<SpriteRenderer>().enabled = enabled;
			shield.GetComponent<Collider2D>().isTrigger = !enabled;
			shield.GetComponent<Collider2D>().enabled = enabled;
			if (currentCombatMode == PlayerCombatMode.CombatMode.Ranged)
			{
				SetSideShieldLeftEnabled(!enabled);
				SetSideShieldRightEnabled(!enabled);
			}
		}

		private void SetSideShieldLeftEnabled(bool enabled)
		{
			sideShieldLeft.GetComponent<SpriteRenderer>().enabled = enabled;
			sideShieldLeft.GetComponent<Collider2D>().isTrigger = !enabled;
			sideShieldLeft.GetComponent<Collider2D>().enabled = enabled;
		}

		private void SetSideShieldRightEnabled(bool enabled)
		{
			sideShieldRight.GetComponent<SpriteRenderer>().enabled = enabled;
			sideShieldRight.GetComponent<Collider2D>().isTrigger = !enabled;
			sideShieldRight.GetComponent<Collider2D>().enabled = enabled;
		}

		public class TimelineRecord_PlayerGuard : TimelineRecord_PlayerCombatModeUser
		{
			public GameObject shield;
			public GameObject sideShieldLeft;
			public GameObject sideShieldRight;
		}
	}
}
