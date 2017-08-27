using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>A sprite-based UI element for displaying values with a
 * maximum and a current value, such as character HP. Renders
 * in the scene space.</summary>
 */
public class FillBarInScene : FillBar
{
	public GameObject bar;
	public GameObject mask;

	protected override void SetBarColor(Color color)
	{
		bar.GetComponent<SpriteRenderer>().color = color;
	}

	protected override void SetBarLength(float length)
	{
		mask.transform.localPosition = new Vector3(
			-1.0f + length,
			mask.transform.localPosition.y,
			mask.transform.localPosition.z
			);
	}
}
