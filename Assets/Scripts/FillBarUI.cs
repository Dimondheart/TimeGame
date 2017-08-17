using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>A sprite-based UI element for displaying values with a
 * maximum and a current value, such as character HP.</summary>
 */
public class FillBarUI : MonoBehaviour, IMaxValue, ICurrentValue
{
	/**<summary>The color of the bar when current equals max (50%-100% when
	 * not blending.)</summary>
	 */
	public Color fullColor;
	/**<summary>The color of the bar when current is 50% (25%-50% when
	 * not blending.</summary>
	 */
	public Color halfColor;
	/**<summary>The color of the bar when current is 0 (below
	 * 25% if not blending.)</summary>
	 */
	public Color lowColor;
	/**<summary>If colors should be blended for intermediate values.</summary>*/
	public bool blendColors;
	public Component maxValueContainer;
	public Component currentValueContainer;
	public GameObject bar;
	public GameObject mask;

	public float MaxValue
	{
		get
		{
			return ((IMaxValue)maxValueContainer).MaxValue;
		}
	}

	public float CurrentValue
	{
		get
		{
			return ((ICurrentValue)currentValueContainer).CurrentValue;
		}
	}

	public float FillRatio
	{
		get
		{
			if (CurrentValue <= 0.0f || Mathf.Approximately(0.0f, CurrentValue))
			{
				return 0.0f;
			}
			return Mathf.Clamp(CurrentValue / MaxValue, 0.02f, MaxValue);
		}
	}

	private void LateUpdate()
	{
		float fillRat = FillRatio;
		mask.transform.localPosition = new Vector3(
			-1.0f + fillRat,
			mask.transform.localPosition.y,
			mask.transform.localPosition.z
			);
		if (blendColors)
		{
			if (fillRat >= 1.0f)
			{
				bar.GetComponent<SpriteRenderer>().color = fullColor;
			}
			else if (fillRat >= 0.5f)
			{
				bar.GetComponent<SpriteRenderer>().color =
					Color.Lerp(halfColor, fullColor, (fillRat - 0.5f) * 2.0f);
			}
			else
			{
				bar.GetComponent<SpriteRenderer>().color =
					Color.Lerp(lowColor, halfColor, fillRat * 2.0f);
			}
		}
		else
		{
			if (fillRat > 0.5f)
			{
				bar.GetComponent<SpriteRenderer>().color = fullColor;
			}
			else if (fillRat > 0.25f)
			{
				bar.GetComponent<SpriteRenderer>().color = halfColor;
			}
			else
			{
				bar.GetComponent<SpriteRenderer>().color = lowColor;
			}
		}
	}
}
