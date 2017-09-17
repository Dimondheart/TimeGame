using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary>Virtual buttons that do not have an existing way to check all
 * the button states (such as using a controller trigger as a button.) This
 * class handles state updates and checking for the button down and button
 * up states, using the GetButton() function implemented by child classes.
 * </summary>
 */
	public abstract class VirtualButtonWithState : VirtualButton
	{
		private ButtonState currentState;

		public override void UpdateState()
		{
			switch (currentState)
			{
				case ButtonState.NotPressed:
					if (GetButton())
					{
						currentState = ButtonState.ButtonDown;
					}
					break;
				case ButtonState.ButtonDown:
					if (GetButton())
					{
						currentState = ButtonState.ButtonHeld;
					}
					else
					{
						currentState = ButtonState.ButtonUp;
					}
					break;
				case ButtonState.ButtonHeld:
					if (!GetButton())
					{
						currentState = ButtonState.ButtonUp;
					}
					break;
				case ButtonState.ButtonUp:
					if (GetButton())
					{
						currentState = ButtonState.ButtonDown;
					}
					else
					{
						currentState = ButtonState.NotPressed;
					}
					break;
				default:
					Debug.LogWarning("Invalid button state (int):" + (int)currentState);
					break;
			}
		}

		public override bool GetButtonDown()
		{
			return currentState == ButtonState.ButtonDown;
		}

		public override bool GetButtonUp()
		{
			return currentState == ButtonState.ButtonUp;
		}

		/**<summary>The states a button can be in.</summary>*/
		private enum ButtonState : byte
		{
			NotPressed = 0,
			ButtonDown,
			ButtonHeld,
			ButtonUp
		}
	}
}
