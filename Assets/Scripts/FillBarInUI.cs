using System;
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
	public Image maxBar;
	private float initialBarWidth;
	private float initialMaxBarWidth;

	private void Start()
	{
		initialBarWidth = bar.rectTransform.sizeDelta.x;
		initialMaxBarWidth = maxBar.rectTransform.sizeDelta.x;
	}

	protected override void SetBarColor(Color color)
	{
		bar.color = color;
	}

	protected override void SetBarLength(float length)
	{
		float newLength = length * initialBarWidth;
		bar.rectTransform.sizeDelta = new Vector2(newLength, bar.rectTransform.sizeDelta.y);
	}

	protected override void SetMaxBarLength(float length)
	{
		float newLength = length * initialMaxBarWidth;
		maxBar.rectTransform.sizeDelta = new Vector2(newLength, maxBar.rectTransform.sizeDelta.y);
	}
}
