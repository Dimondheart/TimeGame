using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary></summary>*/
	public class VirtualAxisFromMouseMovement : VirtualAxisWithBuffer
	{
		public bool usingMouseX { get; private set; }

		private float lastMousePosition = 0.0f;

		public VirtualAxisFromMouseMovement(bool usingMouseX)
		{
			this.usingMouseX = usingMouseX;
		}

		public override void UpdateState()
		{
			float delta =
				usingMouseX ?
				(Input.mousePosition.x - lastMousePosition)
				: (Input.mousePosition.y - lastMousePosition);
			if (Mathf.Abs(delta) < 0.9f)
			{
				rawAxisValue = 0.0f;
			}
			else
			{
				rawAxisValue = Mathf.Clamp(delta / 500.0f, -1.0f, 1.0f);
			}
			lastMousePosition =
				usingMouseX ?
				Input.mousePosition.x
				: Input.mousePosition.y;
			base.UpdateState();
		}
	}
}
