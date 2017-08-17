using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Chrono-Points for use by chrono-abilities.</summary>*/
public class ChronoPoints : MonoBehaviour, IMaxValue, ICurrentValue
{
	public float maxCP;
	public float currentCP { get; private set; }

	float IMaxValue.MaxValue
	{
		get
		{
			return maxCP;
		}
	}

	float ICurrentValue.CurrentValue
	{
		get
		{
			return currentCP;
		}
	}

	private void Awake()
	{
		currentCP = maxCP;
	}

	private void Update()
	{
		currentCP -= 20.0f * Time.deltaTime;
		currentCP = currentCP < 0.0f ? 0.0f : currentCP;
	}
}
