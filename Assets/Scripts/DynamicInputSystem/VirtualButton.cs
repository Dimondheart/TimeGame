using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary>A button type control that can be pressed or not pressed, and
	 * can also say if it was just pressed/released this update.</summary>
	 */
	public abstract class VirtualButton : VirtualInput
	{
		/**<summary>The button is pressed, and was not pressed the previous update.</summary>*/
		public abstract bool GetButtonDown();
		/**<summary>The button is pressed.</summary>*/
		public abstract bool GetButton();
		/**<summary>The button is not pressed, but was pressed the previous update.</summary>*/
		public abstract bool GetButtonUp();
	}
}
