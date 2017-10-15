using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.DynamicInputSystem;
using TechnoWolf.TimeManipulation;
using System;

namespace TechnoWolf.Project1
{
	/**<summary>Player controlled guarding.</summary>*/
	public class PlayerGuard : RecordableMonoBehaviour, IHitTaker
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

		protected override void FlowingUpdate()
		{
			if (GetComponent<SurfaceInteraction>().IsSwimming)
			{
				SetGuardEnabled(false);
			}
			else
			{
				if (DynamicInput.GetButtonDown("Left Hand Action"))
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
				else if (DynamicInput.GetButtonHeld("Left Hand Action"))
				{
					if (!GetComponent<PlayerMovement>().IsDashing && (GetComponent<PlayerMelee>().IsInCooldown || !GetComponent<PlayerMelee>().IsSwinging))
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
			SetSideShieldLeftEnabled(false);
			SetSideShieldRightEnabled(false);
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

		public class TimelineRecord_PlayerGuard : TimelineRecordForBehaviour<PlayerGuard>
		{
			public GameObject shield;
			public GameObject sideShieldLeft;
			public GameObject sideShieldRight;

			protected override void WriteCurrentState(PlayerGuard guard)
			{
				base.WriteCurrentState(guard);
				shield = guard.shield;
				sideShieldLeft = guard.sideShieldLeft;
				sideShieldRight = guard.sideShieldRight;
			}

			protected override void ApplyRecordedState(PlayerGuard guard)
			{
				base.ApplyRecordedState(guard);
				guard.shield = shield;
				guard.sideShieldLeft = sideShieldLeft;
				guard.sideShieldRight = sideShieldRight;
			}
		}
	}
}
