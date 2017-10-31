using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary>The basic type of dynamic control for buttons.</summary>*/
	public class DynamicControlButton : DynamicControl<VirtualButton>
	{
		public DynamicControlButton() : base()
		{
		}

		public DynamicControlButton(string description, VirtualButton gamepadButton, VirtualButton keyMouseButton, VirtualButton keyMouseButtonAlt) : base(description)
		{
			SetGamepadInput(gamepadButton);
			SetKeyMouseInput(keyMouseButton);
			SetKeyMouseAltInput(keyMouseButtonAlt);
		}

		public override void SetGamepadInput(VirtualButton newInput)
		{
			base.SetGamepadInput(VirtualButtonPlaceholder.GetPlaceholderIfNull(newInput));
		}

		public override void SetKeyMouseInput(VirtualButton newInput)
		{
			base.SetKeyMouseInput(VirtualButtonPlaceholder.GetPlaceholderIfNull(newInput));
		}

		public override void SetKeyMouseAltInput(VirtualButton newInput)
		{
			base.SetKeyMouseAltInput(VirtualButtonPlaceholder.GetPlaceholderIfNull(newInput));
		}

		public bool GetButtonDown()
		{
			return gamepadInput.GetButtonDown() || keyMouseInput.GetButtonDown() || keyMouseAltInput.GetButtonDown();
		}

		public bool GetButton()
		{
			return gamepadInput.GetButton() || keyMouseInput.GetButton() || keyMouseAltInput.GetButton();
		}

		public bool GetButtonUp()
		{
			return gamepadInput.GetButtonUp() || keyMouseInput.GetButtonUp() || keyMouseAltInput.GetButtonUp();
		}
	}
}
