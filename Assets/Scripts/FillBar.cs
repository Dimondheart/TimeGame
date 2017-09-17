using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.Project1
{
	/**<summary>Base class for a bar that indicates the state of something such as
	 * health/HP.</summary>
	 */
	public abstract class FillBar : MonoBehaviour, IPrimaryValue
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
		/**<summary>Component with an implementation for IMaxValue to use for
		 * the max value of this fill bar.</summary>
		 */
		public Component primaryValueContainer;

		public float MaxValue
		{
			get
			{
				return ((IPrimaryValue)primaryValueContainer).MaxValue;
			}
		}

		public float MaxCurrentValue
		{
			get
			{
				return ((IPrimaryValue)primaryValueContainer).MaxCurrentValue;
			}
		}

		public float CurrentValue
		{
			get
			{
				return ((IPrimaryValue)primaryValueContainer).CurrentValue;
			}
		}

		public float FillRatio
		{
			get
			{
				return Mathf.Clamp01(CurrentValue / MaxValue);
			}
		}

		public float FillRatioMax
		{
			get
			{
				return Mathf.Clamp01(MaxCurrentValue / MaxValue);
			}
		}

		protected abstract void SetBarColor(Color color);
		/**<summary>Set the bar length. The length passed in is the ratio
		 * new_length/full_length.</summary>
		 */
		protected abstract void SetBarLength(float length);
		protected abstract void SetMaxBarLength(float length);

		private void LateUpdate()
		{
			float fillRat = FillRatio;
			SetBarLength(fillRat);
			SetMaxBarLength(FillRatioMax);
			if (blendColors)
			{
				if (fillRat >= 1.0f)
				{
					SetBarColor(fullColor);
				}
				else if (fillRat >= 0.5f)
				{
					SetBarColor(Color.Lerp(halfColor, fullColor, (fillRat - 0.5f) * 2.0f));
				}
				else
				{
					SetBarColor(Color.Lerp(lowColor, halfColor, fillRat * 2.0f));
				}
			}
			else
			{
				if (fillRat > 0.5f)
				{
					SetBarColor(fullColor);
				}
				else if (fillRat > 0.25f)
				{
					SetBarColor(halfColor);
				}
				else
				{
					SetBarColor(lowColor);
				}
			}
		}
	}
}
