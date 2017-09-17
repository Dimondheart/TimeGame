using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary>A virtual button type for using an axis like a button. The button
 * is considered pressed when the absolute value of the axis is greater than
 * a minimum value.</summary>
 */
	public class VirtualButtonFromAxis : VirtualButtonWithState
	{
		public static readonly float minPressValue = 0.2f;

		public string axisName { get; private set; }

		public VirtualButtonFromAxis(string axisName)
		{
			this.axisName = axisName;
		}

		public override bool GetButton()
		{
			return Mathf.Abs(Input.GetAxisRaw(axisName)) >= minPressValue;
		}
	}
}
