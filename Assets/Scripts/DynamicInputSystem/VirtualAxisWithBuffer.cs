using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary>A virtual axis that updates a stored axis value once each update,
 * and will also (eventually) support automatic smoothing for the non-raw axis value,
 * simmilar to the Unity Input axis smoothing.</summary>
 */
	public abstract class VirtualAxisWithBuffer : VirtualAxis
	{
		protected float rawAxisValue;

		public override void UpdateState()
		{
			// Smooth axisValue here
		}

		public override float GetAxisRaw()
		{
			return rawAxisValue;
		}

		public override float GetAxis()
		{
			return rawAxisValue;
		}
	}
}
