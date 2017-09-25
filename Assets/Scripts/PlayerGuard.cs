using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.DynamicInputSystem;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Player controlled guarding.</summary>*/
	public class PlayerGuard : MonoBehaviour, ITimelineRecordable, IHitTaker
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

		TimelineRecordForComponent ITimelineRecordable.MakeTimelineRecord()
		{
			return new TimelineRecord_PlayerGuard();
		}

		void ITimelineRecordable.RecordCurrentState(TimelineRecordForComponent record)
		{
			TimelineRecord_PlayerGuard rec = (TimelineRecord_PlayerGuard)record;
			rec.shield = shield;
			rec.sideShieldLeft = sideShieldLeft;
			rec.sideShieldRight = sideShieldRight;
		}

		void ITimelineRecordable.ApplyTimelineRecord(TimelineRecordForComponent record)
		{
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

		private void Update()
		{
			if (ManipulableTime.IsApplyingRecords || ManipulableTime.IsTimeOrGamePaused)
			{
				return;
			}
			if (GetComponent<SurfaceInteraction>().IsSwimming)
			{
				Enabled(false);
			}
			else
			{
				if (DynamicInput.GetButtonDown("Guard"))
				{
					if (!GetComponent<PlayerMovement>().IsDashing)
					{
						GetComponent<PlayerMelee>().StopSwinging();
						Enabled(true);
					}
					else
					{
						Enabled(false);
					}
				}
				else if (DynamicInput.GetButtonHeld("Guard"))
				{
					if (!GetComponent<PlayerMovement>().IsDashing && GetComponent<PlayerMelee>().IsInCooldown)
					{
						GetComponent<PlayerMelee>().StopSwinging();
						Enabled(true);
					}
					else
					{
						Enabled(false);
					}
				}
				else
				{
					Enabled(false);
				}
			}
		}

		private void Enabled(bool enabled)
		{
			shield.GetComponent<SpriteRenderer>().enabled = enabled;
			shield.GetComponent<Collider2D>().isTrigger = !enabled;
		}

		public class TimelineRecord_PlayerGuard : TimelineRecordForComponent
		{
			public GameObject shield;
			public GameObject sideShieldLeft;
			public GameObject sideShieldRight;
		}
	}
}
