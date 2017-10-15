using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	public abstract class TimelineRecord_ComponentWithEnabled<T> : TimelineRecordForComponent<T>
		where T : Component
	{
		/**<summary>If component was enabled at this point in time.</summary>*/
		public bool enabled;

		protected override void ApplyRecordedState(T component)
		{
			base.ApplyRecordedState(component);
			if (component is Collider)
			{
				((Collider)(object)component).enabled = enabled;
			}
			else if (component is LODGroup)
			{
				((LODGroup)(object)component).enabled = enabled;
			}
			else if (component is Cloth)
			{
				((Cloth)(object)component).enabled = enabled;
			}
			else if (component is Renderer)
			{
				((Renderer)(object)component).enabled = enabled;
			}
		}

		protected override void WriteCurrentState(T component)
		{
			base.WriteCurrentState(component);
			if (component is Collider)
			{
				enabled = ((Collider)(object)component).enabled;
			}
			else if (component is LODGroup)
			{
				enabled = ((LODGroup)(object)component).enabled;
			}
			else if (component is Cloth)
			{
				enabled = ((Cloth)(object)component).enabled;
			}
			else if (component is Renderer)
			{
				enabled = ((Renderer)(object)component).enabled;
			}
		}
	}
	/**<summary>Timeline record for non-behaviour Unity components that have an
	 * enabled property, but don't have their own timeline record.</summary>
	 */
	public sealed class TimelineRecord_ComponentWithEnabled : TimelineRecord_ComponentWithEnabled<Component>
	{
	}
}
