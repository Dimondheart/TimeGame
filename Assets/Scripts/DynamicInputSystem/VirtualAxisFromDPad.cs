using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary></summary>*/
	public class VirtualAxisFromDPad : VirtualAxisWithBuffer
	{
		public bool useVertical = false;

		public override void UpdateState()
		{
			rawAxisValue = useVertical ? ControllerDPad.VerticalAxis() : ControllerDPad.HorizontalAxis();
			base.UpdateState();
		}
	}
}
