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
		/**<summary>Rate at which the smoothed axis value moves towards
		 * the raw axis value, in units per second.</summary>
		 */
		public float smoothRate = 0.1f;
		/**<summary>Raw axis value.</summary>*/
		protected float rawAxisValue;
		/**<summary>Smoothed axis value.</summary>*/
		protected float axisValue;

		public override void UpdateState()
		{
			float offBy = rawAxisValue - axisValue;
			if (offBy > 0)
			{
				axisValue += Mathf.Clamp(smoothRate * Time.deltaTime, 0.0f, offBy);
			}
			else
			{
				axisValue += Mathf.Clamp(-smoothRate * Time.deltaTime, offBy, 0.0f);
			}
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
