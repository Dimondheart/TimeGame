using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary>Basic virtual axis that reads from the Unity Input class.</summary>*/
	public class VirtualAxisBasic : VirtualAxis
	{
		public string axisName;

		public VirtualAxisBasic(string axisName)
		{
			this.axisName = axisName;
		}

		public override float GetAxisRaw()
		{
			return Input.GetAxisRaw(axisName);
		}

		public override float GetAxis()
		{
			return Input.GetAxis(axisName);
		}
	}
}
