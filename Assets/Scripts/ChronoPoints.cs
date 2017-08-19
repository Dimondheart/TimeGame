using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Chrono-Points for use by chrono-abilities.</summary>*/
public class ChronoPoints : MonoBehaviour, IMaxValue, ICurrentValue
{
	public float maxCP;
	public float currentCP { get; private set; }
	public float timeFreezeDrainRate = 5.0f;
	public bool isTimeFreezeActive;

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
		if (isTimeFreezeActive)
		{
			currentCP = Mathf.Clamp(currentCP - Time.deltaTime * timeFreezeDrainRate, 0.0f, maxCP);
		}
	}
}
