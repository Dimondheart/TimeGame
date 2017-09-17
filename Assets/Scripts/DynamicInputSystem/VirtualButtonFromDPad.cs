using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary>A virtual button from a single direction of the controller DPad.</summary>*/
	public class VirtualButtonFromDPad : VirtualButtonWithState
	{
		public bool horizontalAxis { get; private set; }
		public bool positiveDirection { get; private set; }

		public VirtualButtonFromDPad(bool horizontalAxis, bool positiveDirection)
		{
			this.horizontalAxis = horizontalAxis;
			this.positiveDirection = positiveDirection;
		}
		public override bool GetButton()
		{
			int axisValue = (horizontalAxis) ? ControllerDPad.HorizontalAxis() : ControllerDPad.VerticalAxis();
			if (positiveDirection)
			{
				return axisValue > 0;
			}
			else
			{
				return axisValue < 0;
			}
		}
	}
}
