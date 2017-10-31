using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary>Standard dynamic control for a set of virtual axes.</summary>*/
	public class DynamicControlAxis : DynamicControl<VirtualAxis>
	{
		public DynamicControlAxis() : base()
		{
		}

		public DynamicControlAxis(string description, VirtualAxis gamepadAxis, VirtualAxis keyMouseAxis) : base(description)
		{
			SetGamepadInput(gamepadAxis);
			SetKeyMouseInput(keyMouseAxis);
		}

		public float GetAxisRaw()
		{
			if (DynamicInput.GamepadModeEnabled)
			{
				return gamepadInput.GetAxisRaw();
			}
			return keyMouseInput.GetAxisRaw();
		}

		public float GetAxis()
		{
			if (DynamicInput.GamepadModeEnabled)
			{
				return gamepadInput.GetAxis();
			}
			return keyMouseInput.GetAxis();
		}
	}
}
