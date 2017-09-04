using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Player control of the elemental alignment.</summary>*/
public class PlayerElementControl : MonoBehaviour
{
	private void Update()
	{
		float newTemp = DynamicInput.GetAxis("Temperature Element Adjust");
		float newMoist = DynamicInput.GetAxis("Moisture Element Adjust");
		ElementalAlignment.Element newFocus = ElementalAlignment.Element.None;
		ElementalAlignment.Element currentFocus = GetComponent<ElementalAlignment>().GainFocus;
		if (!Mathf.Approximately(0.0f, newTemp))
		{
			if (newTemp < 0.0f)
			{
				// Cold bit is 0
				if ((currentFocus & ElementalAlignment.Element.Cold) == ElementalAlignment.Element.None)
				{
					newFocus = (currentFocus | ElementalAlignment.Element.Hot) ^ ElementalAlignment.Element.Temp;
				}
				// Cold bit is 1
				else
				{
					newFocus = currentFocus ^ ElementalAlignment.Element.Cold;
				}
			}
			else
			{
				// Hot bit is 0
				if ((currentFocus & ElementalAlignment.Element.Hot) == ElementalAlignment.Element.None)
				{
					newFocus = (currentFocus | ElementalAlignment.Element.Cold) ^ ElementalAlignment.Element.Temp;
				}
				// Hot bit is 1
				else
				{
					newFocus = currentFocus ^ ElementalAlignment.Element.Hot;
				}
			}
		}
		if (!Mathf.Approximately(0.0f, newMoist))
		{
			if (newMoist < 0.0f)
			{
				// Dry bit is 0
				if ((currentFocus & ElementalAlignment.Element.Dry) == ElementalAlignment.Element.None)
				{
					newFocus = (currentFocus | ElementalAlignment.Element.Wet) ^ ElementalAlignment.Element.Moist;
				}
				// Dry bit is 1
				else
				{
					newFocus = currentFocus ^ ElementalAlignment.Element.Dry;
				}
			}
			else
			{
				// Wet bit is 0
				if ((currentFocus & ElementalAlignment.Element.Wet) == ElementalAlignment.Element.None)
				{
					newFocus = (currentFocus | ElementalAlignment.Element.Dry) ^ ElementalAlignment.Element.Moist;
				}
				// Wet bit is 1
				else
				{
					newFocus = currentFocus ^ ElementalAlignment.Element.Wet;
				}
			}
		}
		GetComponent<ElementalAlignment>().GainFocus = newFocus;
	}
}
