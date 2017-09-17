using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary>Base class for for the different variants of dynamic control types.</summary>*/
	public abstract class DynamicControl
	{
		public readonly string description;

		public DynamicControl(string description)
		{
			this.description = description;
		}

		public abstract void UpdateControlStates();
	}

	/**<summary>Base class for dynamic controls with a gamepad input and 1-2
	 * keyboard/mouse inputs.</summary>
	 */
	public abstract class DynamicControl<T> : DynamicControl where T : VirtualInput
	{
		public T gamepadInput { get; private set; }
		public T keyMouseInput { get; private set; }
		public T keyMouseAltInput { get; private set; }

		public DynamicControl(string description) : base(description)
		{
		}

		public override void UpdateControlStates()
		{
			if (gamepadInput != null)
			{
				gamepadInput.UpdateState();
			}
			if (keyMouseInput != null)
			{
				keyMouseInput.UpdateState();
			}
			if (keyMouseAltInput != null)
			{
				keyMouseAltInput.UpdateState();
			}
		}

		public virtual void SetGamepadInput(T newInput)
		{
			gamepadInput = newInput;
		}

		public virtual void SetKeyMouseInput(T newInput)
		{
			keyMouseInput = newInput;
		}

		public virtual void SetKeyMouseAltInput(T newInput)
		{
			keyMouseAltInput = newInput;
		}
	}
}
