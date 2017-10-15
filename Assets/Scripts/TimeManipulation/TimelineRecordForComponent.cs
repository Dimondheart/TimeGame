using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Timeline record base for component records.</summary>*/
	public abstract class TimelineRecordForComponent<T> : TimelineRecord<T>
		where T : Component
	{
		/**<summary>Check if the specified component has a timeline maker
		 * implemented by this class.</summary>
		 */
		public static bool HasTimelineMaker(Component component)
		{
			return component is Transform
				|| component is SpriteRenderer
				|| component is Rigidbody2D;
		}

		public static Timeline MakeTimeline(Component component)
		{
			if (component is Transform)
			{
				return new Timeline(typeof(TimelineRecord_Transform), true);
			}
			else if (component is SpriteRenderer)
			{
				return new Timeline(typeof(TimelineRecord_SpriteRenderer), true);
			}
			else if (component is Rigidbody2D)
			{
				return new Timeline(typeof(TimelineRecord_Rigidbody2D), true);
			}
			Debug.LogError(
				"Attempted to use TimelineRecordForComponent to make a" +
				"timeline for a component that does not support it."
				);
			return null;
		}

		public static bool IsComponentWithEnabled(Component component)
		{
			return component is Collider
				|| component is LODGroup
				|| component is Cloth
				|| component is Renderer;
		}
	}
}
