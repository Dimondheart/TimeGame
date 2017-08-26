using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>An alternate version of the Unity Time class that
 * supports the time manipulation mechanics.</summary>
 */
public class ManipulableTime : MonoBehaviour
{
	private static readonly int recordInterval = 4;

	private static bool isTimeFrozenInternal;
	private static TimeFreezeState timeFreezeState;
	private static TimelineState timelineState;
	private static Dictionary<int, float> timeAtCycle;
	private static Dictionary<int, float> deltaTimeAtCycle;
	private static Dictionary<int, float> fixedTimeAtCycle;
	private static Dictionary<int, float> fixedDeltaTimeAtCycle;

	public static float time { get; private set; }
	public static float deltaTime { get; private set; }
	public static float fixedTime { get; private set; }
	public static float fixedDeltaTime { get; private set; }
	public static int cycleNumber { get; private set; }
	public static int oldestRecordedCycle { get; private set; }
	public static int newestRecordedCycle { get; private set; }
	/**<summary></summary>*/
	public static bool IsTimeFrozen
	{
		get
		{
			return isTimeFrozenInternal || IsGameFrozen;
		}
		set
		{
			if (IsGameFrozen)
			{
				return;
			}
			if (value != isTimeFrozenInternal)
			{
				if (timeFreezeState == TimeFreezeState.ChangedLastUpdate)
				{
					timeFreezeState = TimeFreezeState.ChangedThisUpdateAndLastUpdate;
				}
				else if (timeFreezeState != TimeFreezeState.ChangedThisUpdateAndLastUpdate)
				{
					timeFreezeState = TimeFreezeState.ChangedThisUpdate;
				}
			}
			isTimeFrozenInternal = value;
			if (!value)
			{
				StopRewind();
				StopReplay();
			}
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
	/**<summary>Check if the enabled status of time freeze was changed during
	 * the previous call to Update().</summary>
	 */
	public static bool TimeFreezeChanged
	{
		get
		{
			return timeFreezeState == TimeFreezeState.ChangedLastUpdate || timeFreezeState == TimeFreezeState.ChangedThisUpdateAndLastUpdate;
		}
	}
	public static bool RecordModeEnabled
	{
		get
		{
			return timelineState == TimelineState.Recording || timelineState == TimelineState.RecordingAndRewindInitiated;
		}
	}
	public static bool RewindModeEnabled
	{
		get
		{
			return timelineState == TimelineState.Rewinding;
		}
	}
	public static bool ReplayModeEnabled
	{
		get
		{
			return timelineState == TimelineState.Replaying;
		}
	}
	public static bool ApplyingTimelineRecords
	{
		get
		{
			return RewindModeEnabled || ReplayModeEnabled;
		}
	}

	/**<summary>Set the state to begin rewinding on the next update.
	 * Will not initiate the state if the game is frozen, time is
	 * not frozen, or if there is nothing to rewind to.</summary>
	 */
	public static void InitiateRewind()
	{
		if (IsGameFrozen || !IsTimeFrozen || cycleNumber == oldestRecordedCycle)
		{
			return;
		}
		switch (timelineState)
		{
			case TimelineState.Flowing:
				// Fall through
			case TimelineState.RecordInitiated:
				timelineState = TimelineState.RewindInitiated;
				break;
			case TimelineState.Recording:
				timelineState = TimelineState.RecordingAndRewindInitiated;
				break;
			case TimelineState.ReplayInitiated:
				// Fall through
			case TimelineState.Replaying:
				timelineState = TimelineState.RewindInitiated;
				break;
			default:
				break;
		}
	}

	/**<summary>End the rewind state. Will not end the state if the
	 * game is frozen.</summary>
	 */
	public static void StopRewind()
	{
		if (IsGameFrozen)
		{
			return;
		}
		switch (timelineState)
		{
			case TimelineState.RecordingAndRewindInitiated:
				timelineState = TimelineState.Recording;
				break;
			case TimelineState.RewindInitiated:
				// Fall through
			case TimelineState.Rewinding:
				if ((cycleNumber + 1) % recordInterval == 0)
				{
					timelineState = TimelineState.RecordInitiated;
				}
				else
				{
					timelineState = TimelineState.Flowing;
				}
				break;
			default:
				break;
		}
	}

	/**<summary>Set the state to begin replaying on the next frame.
	 * This only works if the state is currently rewinding, or time is
	 * frozen. Will not initiate the state if the game is
	 * frozen, time is not frozen, or if there is nothing to replay.</summary>
	 */
	public static void InitiateReplay()
	{
		if (IsGameFrozen || !IsTimeFrozen || cycleNumber == newestRecordedCycle)
		{
			return;
		}
		switch (timelineState)
		{
			case TimelineState.Flowing:
				if (IsTimeFrozen)
				{
					timelineState = TimelineState.ReplayInitiated;
				}
				break;
			case TimelineState.Rewinding:
				// Fall through
			case TimelineState.RewindInitiated:
				timelineState = TimelineState.ReplayInitiated;
				break;
			default:
				break;
		}
	}

	/**<summary>End the rewind state. Will not end the state if the
	 * game is frozen.</summary>
	 */
	public static void StopReplay()
	{
		if (IsGameFrozen)
		{
			return;
		}
		switch (timelineState)
		{
			case TimelineState.ReplayInitiated:
				// Fall through
			case TimelineState.Replaying:
				if ((cycleNumber + 1) % recordInterval == 0)
				{
					timelineState = TimelineState.RecordInitiated;
				}
				else
				{
					timelineState = TimelineState.Flowing;
				}
				break;
			default:
				break;
		}
	}

	private void Awake()
	{
		cycleNumber = -1;
		oldestRecordedCycle = 0;
		newestRecordedCycle = 0;
		timelineState = TimelineState.RecordInitiated;
		IsGameFrozen = false;
		time = Time.time;
		deltaTime = Time.deltaTime;
		fixedTime = Time.fixedTime;
		fixedDeltaTime = Time.fixedDeltaTime;
		timeAtCycle = new Dictionary<int, float>();
		deltaTimeAtCycle = new Dictionary<int, float>();
		fixedTimeAtCycle = new Dictionary<int, float>();
		fixedDeltaTimeAtCycle = new Dictionary<int, float>();
	}

	private void Update()
	{
		if (IsGameFrozen)
		{
			return;
		}
		switch (timelineState)
		{
			case TimelineState.Flowing:
				UpdateInternalTime();
				if (!IsTimeFrozen && (cycleNumber + 1) % recordInterval == 0)
				{
					timelineState = TimelineState.RecordInitiated;
				}
				break;
			case TimelineState.RecordInitiated:
				UpdateInternalTime();
				if (!IsTimeFrozen)
				{
					timelineState = TimelineState.Recording;
				}
				break;
			case TimelineState.Recording:
				UpdateInternalTime();
				timelineState = TimelineState.Flowing;
				break;
			case TimelineState.RecordingAndRewindInitiated:
				// Fall through
			case TimelineState.RewindInitiated:
				if (IsTimeFrozen)
				{
					timelineState = TimelineState.Rewinding;
				}
				else
				{
					UpdateInternalTime();
					if ((cycleNumber + 1) % recordInterval == 0)
					{
						timelineState = TimelineState.RecordInitiated;
					}
					else
					{
						timelineState = TimelineState.Flowing;
					}
				}
				break;
			case TimelineState.Rewinding:
				if (cycleNumber > oldestRecordedCycle)
				{
					SetCurrentCycle(cycleNumber - 1);
					//Debug.Log("Rewinding (new cycle):" + cycleNumber);
				}
				break;
			case TimelineState.ReplayInitiated:
				timelineState = TimelineState.Replaying;
				break;
			case TimelineState.Replaying:
				if (cycleNumber < newestRecordedCycle)
				{
					SetCurrentCycle(cycleNumber + 1);
					//Debug.Log("Replaying (new cycle):" + cycleNumber);
				}
				break;
			default:
				Debug.LogWarning("Unhandled TimelineState ID:" + (int)timelineState);
				break;
		}
	}

	private void FixedUpdate()
	{
		if (IsTimeFrozen || ApplyingTimelineRecords)
		{
			fixedDeltaTime = 0.0f;
		}
		else
		{
			fixedDeltaTime = Time.fixedDeltaTime;
			fixedTime += fixedDeltaTime;
		}
	}

	private void UpdateInternalTime()
	{
		switch (timeFreezeState)
		{
			case TimeFreezeState.NotChangedRecently:
				break;
			case TimeFreezeState.ChangedLastUpdate:
				timeFreezeState = TimeFreezeState.NotChangedRecently;
				break;
			case TimeFreezeState.ChangedThisUpdate:
				timeFreezeState = TimeFreezeState.ChangedLastUpdate;
				break;
			case TimeFreezeState.ChangedThisUpdateAndLastUpdate:
				timeFreezeState = TimeFreezeState.ChangedLastUpdate;
				break;
			default:
				Debug.LogWarning("Unhandled TimeFreezeState ID:" + (int)timeFreezeState);
				break;
		}
		if (IsTimeFrozen)
		{
			deltaTime = 0.0f;
			// Also set fixed delta time to 0 here in case Time.timeScale is 0
			fixedDeltaTime = 0.0f;
			return;
		}
		deltaTime = Time.deltaTime;
		time += deltaTime;
		cycleNumber++;
		newestRecordedCycle = cycleNumber;
		timeAtCycle[cycleNumber] = time;
		deltaTimeAtCycle[cycleNumber] = deltaTime;
		fixedTimeAtCycle[cycleNumber] = fixedTime;
		fixedDeltaTimeAtCycle[cycleNumber] = fixedDeltaTime;
	}

	private void SetCurrentCycle(int newCycleNumber)
	{
		timeFreezeState = TimeFreezeState.NotChangedRecently;
		cycleNumber = newCycleNumber;
		time = timeAtCycle[cycleNumber];
		deltaTime = deltaTimeAtCycle[cycleNumber];
		fixedTime = fixedTimeAtCycle[cycleNumber];
		fixedDeltaTime = fixedDeltaTimeAtCycle[cycleNumber];
	}

	private enum TimeFreezeState
	{
		NotChangedRecently = 0,
		ChangedThisUpdate,
		ChangedLastUpdate,
		ChangedThisUpdateAndLastUpdate
	}

	private enum TimelineState
	{
		/**<summary>Nothing should be done with timelines, and time is
		 * moving forward like normal or is frozen.</summary>
		 */
		Flowing = 0,
		/**<summary>Timelines will be recording next cycle.</summary>*/
		RecordInitiated,
		/**<summary>Timelines should be recording states, and time is
		 * moving forward like normal.</summary>
		 */
		Recording,
		/**<summary>Time will be rewinding starting next cycle.</summary>*/
		RewindInitiated,
		/**<summary>Timelines should be recording states, and a
		 * rewind will be starting next cycle.</summary>
		 */
		RecordingAndRewindInitiated,
		/**<summary>Time is rewinding.</summary>*/
		Rewinding,
		/**<summary>Recorded time will be replaying starting next cycle.</summary>*/
		ReplayInitiated,
		Replaying
	}
}
