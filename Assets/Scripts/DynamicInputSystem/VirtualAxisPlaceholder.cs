using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary></summary>*/
public class VirtualAxisPlaceholder : MonoBehaviour
{
}
namespace TechnoWolf.DynamicInputSystem
{
	/**<summary></summary>*/
	public class VirtualAxisPlaceholder : VirtualAxis
	{
		public override float GetAxisRaw()
		{
			return 0.0f;
		}

		public override float GetAxis()
		{
			return 0.0f;
		}
	}
}
