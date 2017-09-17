using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary>Implementation of a virtual button that returns false for
	 * all values.</summary>
	 */
	public class VirtualButtonPlaceholder : VirtualButton
	{
		public static readonly VirtualButtonPlaceholder placeholder = new VirtualButtonPlaceholder();

		public static VirtualButton GetPlaceholderIfNull(VirtualButton button)
		{
			return (button == null) ? placeholder : button;
		}

		private VirtualButtonPlaceholder()
		{
		}

		public override bool GetButtonDown()
		{
			return false;
		}

		public override bool GetButton()
		{
			return false;
		}

		public override bool GetButtonUp()
		{
			return false;
		}
	}
}
