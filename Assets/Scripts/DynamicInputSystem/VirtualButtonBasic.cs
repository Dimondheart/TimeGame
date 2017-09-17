using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary>A simple button which gets its state from the Unity
	 * Input class.</summary>
	 */
	public class VirtualButtonBasic : VirtualButton
	{
		public KeyCode keyCode;

		public VirtualButtonBasic(KeyCode keyCode)
		{
			this.keyCode = keyCode;
		}

		public override bool GetButtonDown()
		{
			return Input.GetKeyDown(keyCode);
		}

		public override bool GetButton()
		{
			return Input.GetKey(keyCode);
		}

		public override bool GetButtonUp()
		{
			return Input.GetKeyUp(keyCode);
		}
	}
}
