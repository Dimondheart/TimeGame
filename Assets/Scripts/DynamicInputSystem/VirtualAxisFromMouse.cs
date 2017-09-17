using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary>A virtual axis that calculates a value in the range [-1,1] using
 * the position of the mouse relative to the center of the window, with the bounds
 * being the edges of the player.</summary>
 */
	public class VirtualAxisFromMouse : VirtualAxisWithBuffer
	{
		public static readonly float deadzone = 0.05f;

		public bool usingMouseX { get; private set; }

		public VirtualAxisFromMouse(bool usingMouseX)
		{
			this.usingMouseX = usingMouseX;
		}

		public override void UpdateState()
		{
			if (usingMouseX)
			{
				rawAxisValue = Input.mousePosition.x / Screen.width * 2.0f - 1.0f;
			}
			else
			{
				rawAxisValue = Input.mousePosition.y / Screen.height * 2.0f - 1.0f;
			}
			if (Mathf.Abs(rawAxisValue) < deadzone)
			{
				rawAxisValue = 0.0f;
			}
			base.UpdateState();
		}
	}
}
