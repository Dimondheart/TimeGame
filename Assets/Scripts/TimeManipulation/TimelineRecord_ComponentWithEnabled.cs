using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Timeline record for non-behaviour Unity components that have an
	 * enabled property, but don't have their own timeline record.</summary>
	 */
	public class TimelineRecord_ComponentWithEnabled : TimelineRecordForComponent
	{
		/**<summary>If component was enabled at this point in time.</summary>*/
		public bool enabled { get; private set; }

		public override void AddCommonData(Component component)
		{
			if (component is Collider)
			{
				enabled = ((Collider)component).enabled;
			}
			else if (component is LODGroup)
			{
				enabled = ((LODGroup)component).enabled;
			}
			else if (component is Cloth)
			{
				enabled = ((Cloth)component).enabled;
			}
			else if (component is Renderer)
			{
				enabled = ((Renderer)component).enabled;
			}
		}

		public override void ApplyCommonData(Component component)
		{
			if (component is Collider)
			{
				((Collider)component).enabled = enabled;
			}
			else if (component is LODGroup)
			{
				((LODGroup)component).enabled = enabled;
			}
			else if (component is Cloth)
			{
				((Cloth)component).enabled = enabled;
			}
			else if (component is Renderer)
			{
				((Renderer)component).enabled = enabled;
			}
		}
	}
}
