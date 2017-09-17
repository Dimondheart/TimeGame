using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary>Base class for all types of inputs that read/provide the input
	 * values for the dynamic controls.</summary>
	 */
	public abstract class VirtualInput
	{
		public virtual void UpdateState()
		{
		}
	}
}
