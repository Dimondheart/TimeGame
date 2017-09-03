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
	}
}
