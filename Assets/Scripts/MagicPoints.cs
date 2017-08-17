using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Standard MP concept.</summary>*/
public class MagicPoints : MonoBehaviour, IMaxValue, ICurrentValue
{
	public float maxMP;
	public float currentMP { get; private set; }

	float IMaxValue.MaxValue
	{
		get
		{
			return maxMP;
		}
	}

	float ICurrentValue.CurrentValue
	{
		get
		{
			return currentMP;
		}
	}

	private void Awake()
	{
		currentMP = maxMP;
	}

	private void Update()
	{
		currentMP -= 20.0f * Time.deltaTime;
		currentMP = currentMP < 0.0f ? 0.0f : currentMP;
	}
}
