using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**<summary>A version of FillBarUI that renders in screen space
 * instead of in the scene.</summary>
 */
public class FillBarInUI : FillBar
{
	public Image bar;
	private float initialBarWidth;

	private void Start()
	{
		initialBarWidth = bar.rectTransform.sizeDelta.x;
	}

	protected override void SetBarColor(Color color)
	{
		bar.color = color;
	}

	protected override void SetBarLength(float length)
	{
		float newLength = Mathf.RoundToInt(length * initialBarWidth);
		bar.rectTransform.sizeDelta = new Vector2(newLength, bar.rectTransform.sizeDelta.y);
	}
}
