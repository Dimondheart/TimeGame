using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary>A virtual button type for using an axis like a button, but only
 * in one direction (positive/negative.) The button is considered pressed when
 * the axis value is at least minimumValue (or at most -minValue if in the
 * negative direction.)</summary>
 */
	public class VirtualButtonFromAxisDirection : VirtualButtonWithState
	{
		public static readonly float minPressValue = 0.2f;
		public static readonly float minPressValueNeg = -0.2f;

		public string axisName { get; private set; }
		public bool positiveDirection { get; private set; }

		public VirtualButtonFromAxisDirection(string axisName, bool positiveDirection)
		{
			this.axisName = axisName;
			this.positiveDirection = positiveDirection;
		}

		public override bool GetButton()
		{
			if (positiveDirection)
			{
				return Input.GetAxisRaw(axisName) >= minPressValue;
			}
			else
			{
				return Input.GetAxisRaw(axisName) <= minPressValueNeg;
			}
		}
	}
}
