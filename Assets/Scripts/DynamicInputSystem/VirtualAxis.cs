using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary>Base for any virtual input of an axis type, like a joystick axis.</summary>*/
	public abstract class VirtualAxis : VirtualInput
	{
		public abstract float GetAxisRaw();
		public abstract float GetAxis();
	}
}
