using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.Project1
{
	/**<summary>A sprite-based UI element for displaying values with a
	 * maximum and a current value, such as character HP. Renders
	 * in the scene space.</summary>
	 */
	public class FillBarInScene : FillBar
	{
		public SpriteRenderer bar;
		public SpriteRenderer maxBar;

		protected override void SetBarColor(Color color)
		{
			bar.color = color;
		}

		protected override void SetBarLength(float length)
		{
			bar.transform.localPosition = new Vector3(
				-1.0f + length,
				bar.transform.localPosition.y,
				bar.transform.localPosition.z
				);
		}

		protected override void SetMaxBarLength(float length)
		{
			maxBar.transform.localPosition = new Vector3(
				-1.0f + length,
				maxBar.transform.localPosition.y,
				maxBar.transform.localPosition.z
				);
		}
	}
}
