using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>An alternate version of the Unity Time class that
 * supports the time manipulation mechanics.</summary>
 */
public class ManipulableTime : MonoBehaviour
{
	private static bool isTimeFrozenInternal;
	private static TimeFreezeState timeFreezeState;

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
			if (value != isTimeFrozenInternal)
			{
				if (timeFreezeState == TimeFreezeState.ChangedLastFrame)
				{
					timeFreezeState = TimeFreezeState.ChangedThisFrameAndLastFrame;
				}
				else if (timeFreezeState != TimeFreezeState.ChangedThisFrameAndLastFrame)
				{
					timeFreezeState = TimeFreezeState.ChangedThisFrame;
				}
			}
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
	/**<summary>Check if the status of time freeze was changed during the previous
	 * frame.</summary>
	 */
	public static bool TimeFreezeChanged
	{
		get
		{
			return timeFreezeState == TimeFreezeState.ChangedLastFrame || timeFreezeState == TimeFreezeState.ChangedThisFrameAndLastFrame;
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
		switch (timeFreezeState)
		{
			case TimeFreezeState.NotChangedRecently:
				break;
			case TimeFreezeState.ChangedLastFrame:
				timeFreezeState = TimeFreezeState.NotChangedRecently;
				break;
			case TimeFreezeState.ChangedThisFrame:
				timeFreezeState = TimeFreezeState.ChangedLastFrame;
				break;
			case TimeFreezeState.ChangedThisFrameAndLastFrame:
				timeFreezeState = TimeFreezeState.ChangedLastFrame;
				break;
			default:
				Debug.LogWarning("Unhandled TimeFreezeState ID:" + (int)timeFreezeState);
				break;
		}
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

	private enum TimeFreezeState
	{
		NotChangedRecently = 0,
		ChangedThisFrame,
		ChangedLastFrame,
		ChangedThisFrameAndLastFrame
	}
}
