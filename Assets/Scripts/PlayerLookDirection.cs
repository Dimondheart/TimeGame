using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.DynamicInputSystem;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Controls the direction the player is looking.</summary>*/
	[RequireComponent(typeof(DirectionLooking))]
	public class PlayerLookDirection : PausableMonoBehaviour
	{
		protected override void FlowingUpdate()
		{
			if (
				((GetComponent<PlayerMelee>().IsInCooldown || !GetComponent<PlayerMelee>().IsSwinging) && !GetComponent<PlayerMovement>().IsDashing)
				|| GetComponent<SurfaceInteraction>().IsSwimming
				)
			{
				Vector2 lookDirection =
					new Vector2(DynamicInput.GetAxisRaw("Look Horizontal"), DynamicInput.GetAxisRaw("Look Vertical"));
				Vector2 moveDirection =
					new Vector2(DynamicInput.GetAxisRaw("Move Horizontal"), DynamicInput.GetAxisRaw("Move Vertical"));
				if (!Mathf.Approximately(0.0f, lookDirection.magnitude))
				{
					GetComponent<DirectionLooking>().Direction = lookDirection;
				}
				else if (!Mathf.Approximately(0.0f, moveDirection.magnitude))
				{
					GetComponent<DirectionLooking>().Direction = moveDirection;
				}
			}
		}
	}
}
