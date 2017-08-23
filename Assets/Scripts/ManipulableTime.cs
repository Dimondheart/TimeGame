using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>An alternate version of the Unity Time class that
 * supports the time manipulation mechanics.</summary>
 */
public class ManipulableTime : MonoBehaviour
{
	private static bool isTimeFrozenInternal = false;

	public static float time { get; private set; }
	public static float deltaTime { get; private set; }
	public static float fixedTime { get; private set; }
	public static float fixedDeltaTime { get; private set; }
	public static bool IsTimeFrozen
	{
		get
		{
			return isTimeFrozenInternal || IsGameFrozen;
		}
		set
		{
			isTimeFrozenInternal = value;
		}
	}
	public static bool IsGameFrozen
	{
		get
		{
			return Mathf.Approximately(0.0f, Time.timeScale);
		}
		set
		{
			Time.timeScale = value ? 0.0f : 1.0f;
		}
	}

	private void Awake()
	{
		IsGameFrozen = false;
		time = Time.time;
		deltaTime = Time.deltaTime;
		fixedTime = Time.fixedTime;
		fixedDeltaTime = Time.fixedDeltaTime;
	}

	private void Update()
	{
		if (IsTimeFrozen)
		{
			deltaTime = 0.0f;
		}
		else
		{
			deltaTime = Time.deltaTime;
			time += deltaTime;
		}
	}

	private void FixedUpdate()
	{
		if (IsTimeFrozen)
		{
			fixedDeltaTime = 0.0f;
		}
		else
		{
			fixedDeltaTime = Time.fixedDeltaTime;
			fixedTime += fixedDeltaTime;
		}
	}
}
