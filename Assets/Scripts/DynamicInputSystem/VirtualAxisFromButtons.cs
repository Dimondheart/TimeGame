using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary>Uses two virtual buttons to make an axis. The virtual buttons
 * given should not be used within other controls, as this would cause
 * them to be updated multiple times in one cycle.</summary>
 */
	public class VirtualAxisFromButtons : VirtualAxisWithBuffer
	{
		public VirtualButton negative { get; private set; }
		public VirtualButton positive { get; private set; }

		public VirtualAxisFromButtons(VirtualButton negative, VirtualButton positive)
		{
			this.negative = negative;
			this.positive = positive;
		}

		public override void UpdateState()
		{
			negative.UpdateState();
			positive.UpdateState();
			if (positive.GetButton())
			{
				if (negative.GetButton())
				{
					rawAxisValue = 0.0f;
				}
				else
				{
					rawAxisValue = 1.0f;
				}
			}
			else if (negative.GetButton())
			{
				rawAxisValue = -1.0f;
			}
			else
			{
				rawAxisValue = 0.0f;
			}
			base.UpdateState();
		}
	}
}
