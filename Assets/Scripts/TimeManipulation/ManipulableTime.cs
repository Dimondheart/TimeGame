using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>An alternate version of the Unity Time class that
	 * supports the time manipulation mechanics.</summary>
	 */
	public class ManipulableTime : MonoBehaviour
	{
		/**<summary>The furthest that time can rewind from the newest recorded
		 * cycle.</summary>
		 */
		public static readonly float rewindTimeLimit = 2.01f;
		/**<summary>Internal variable for when manipulable time is paused.</summary>*/
		private static bool isTimePausedInternal;
		/**<summary>How recently manipulable time pause has been changed. This affects
		 * certain updates.</summary>
		 */
		private static TimePauseChangeState timePauseState;
		private static TimelineState timelineState;
		private static ExcessDataClearingState excessDataClearingState;
		private static Dictionary<int, TimeDataForCycle> timeDataForCycles;
		private static Stack<TimeDataForCycle> timeDataForCyclePool;

		/**<summary>The time when the current Update cycle started.</summary>*/
		public static float time { get; private set; }
		/**<summary>The time that passed between this Update cycle and
		 * the previous one.</summary>
		 */
		public static float deltaTime { get; private set; }
		/**<summary>The time when the current FixedUpdate cycle started.</summary>*/
		public static float fixedTime { get; private set; }
		/**<summary>The time that passed between this FixedUpdate cycle and
		 * the previous one.</summary>
		 */
		public static float fixedDeltaTime { get; private set; }
		public static int cycleNumber { get; private set; }
		public static int oldestRecordedCycle { get; private set; }
		public static int newestRecordedCycle { get; private set; }
		public static int oldestCycleWithinRewindLimit { get; private set; }
		/**<summary>Indicates and controls if the manipulable time is frozen. This cannnot
		 * be changed while the game is paused.</summary>
		 */
		public static bool IsTimePaused
		{
			get
			{
				return isTimePausedInternal;
			}
			set
			{
				if (IsGamePaused)
				{
					return;
				}
				if (value != isTimePausedInternal)
				{
					if (timePauseState == TimePauseChangeState.ChangedLastUpdate)
					{
						timePauseState = TimePauseChangeState.ChangedThisUpdateAndLastUpdate;
					}
					else if (timePauseState != TimePauseChangeState.ChangedThisUpdateAndLastUpdate)
					{
						timePauseState = TimePauseChangeState.ChangedThisUpdate;
					}
				}
				isTimePausedInternal = value;
				if (!value)
				{
					StopRewind();
					StopReplay();
					InitiateExcessDataClearing();
				}
			}
		}
		/**<summary>Check if manipulable time is frozen and/or the game is paused.</summary>*/
		public static bool IsTimeOrGamePaused
		{
			get
			{
				return IsTimePaused || IsGamePaused;
			}
		}
		/**<summary>Check and set if the game is paused. The game is considered paused when
		 * the Unity Time.timeScale is set to exactly 0.0f.</summary>
		 */
		public static bool IsGamePaused
		{
			get
			{
				return Time.timeScale == 0.0f;
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
				return timePauseState == TimePauseChangeState.ChangedLastUpdate || timePauseState == TimePauseChangeState.ChangedThisUpdateAndLastUpdate;
			}
		}
		public static bool RecordModeEnabled
		{
			get
			{
				return !IsTimeOrGamePaused &&
					(timelineState == TimelineState.Recording || timelineState == TimelineState.RecordingAndRewindInitiated);
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
		public static bool IsApplyingRecords
		{
			get
			{
				return RewindModeEnabled || ReplayModeEnabled;
			}
		}
		/**<summary>If timeline data that is no longer needed (older than
		 * the max rewind time, or data that is newer than the current cycle
		 * when time is unfrozen) should be cleared. Check oldestCycleWithinRewindLimit
		 * for the oldest cycle to keep records for.</summary>
		 */
		public static bool ClearingExcessTimelineData
		{
			get
			{
				return excessDataClearingState == ExcessDataClearingState.Clearing;
			}
		}

		/**<summary>Set the state to begin rewinding on the next update.
		 * Will not initiate the state if the game is frozen, time is
		 * not frozen, or if there is nothing to rewind to.</summary>
		 */
		public static void InitiateRewind()
		{
			if (IsGamePaused || !IsTimeOrGamePaused || cycleNumber == oldestCycleWithinRewindLimit)
			{
				return;
			}
			switch (timelineState)
			{
				case TimelineState.Flowing:
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
			if (IsGamePaused)
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
					//timelineState = TimelineState.Flowing;
					timelineState = TimelineState.Recording;
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
			if (IsGamePaused || !IsTimeOrGamePaused || cycleNumber == newestRecordedCycle)
			{
				return;
			}
			switch (timelineState)
			{
				case TimelineState.Recording:
				// Fall through
				case TimelineState.Flowing:
					if (IsTimeOrGamePaused)
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
			if (IsGamePaused)
			{
				return;
			}
			switch (timelineState)
			{
				case TimelineState.ReplayInitiated:
				// Fall through
				case TimelineState.Replaying:
					timelineState = TimelineState.Recording;
					//timelineState = TimelineState.Flowing;
					break;
				default:
					break;
			}
		}

		/**<summary>Set the excess data clearing state to be active next update.</summary>*/
		private static void InitiateExcessDataClearing()
		{
			if (excessDataClearingState != ExcessDataClearingState.None)
			{
				return;
			}
			excessDataClearingState = ExcessDataClearingState.ClearingNextUpdate;
		}

		/**<summary>Clear excess cycle data that is no longer needed and update
		 * oldest/newest recorded cycle values accordingly. This only clears
		 * data stored within ManipulableTime. Other classes that record timeline
		 * data are responsible for clearing their own excess data, and can check
		 * ClearingExcessTimelineData to see when excess data should be
		 * cleared.</summary>
		 */
		private static void ClearExcessData()
		{
			for (int cn = oldestCycleWithinRewindLimit - 1; cn >= oldestRecordedCycle; cn--)
			{
				if (timeDataForCycles.ContainsKey(cn))
				{
					timeDataForCyclePool.Push(timeDataForCycles[cn]);
					timeDataForCycles.Remove(cn);
				}
			}
			oldestRecordedCycle = oldestCycleWithinRewindLimit;
			for (int cn = cycleNumber + 1; cn <= newestRecordedCycle; cn++)
			{
				if (timeDataForCycles.ContainsKey(cn))
				{
					timeDataForCyclePool.Push(timeDataForCycles[cn]);
					timeDataForCycles.Remove(cn);
				}
			}
			newestRecordedCycle = cycleNumber;
		}

		private void Awake()
		{
			cycleNumber = -1;
			oldestRecordedCycle = 0;
			newestRecordedCycle = 0;
			oldestCycleWithinRewindLimit = oldestRecordedCycle;
			//timelineState = TimelineState.Flowing;
			timelineState = TimelineState.Recording;
			//excessDataClearingState = ExcessDataClearingState.Clearing;
			IsGamePaused = false;
			time = Time.time;
			deltaTime = Time.deltaTime;
			fixedTime = Time.fixedTime;
			fixedDeltaTime = Time.fixedDeltaTime;
			timeDataForCycles = new Dictionary<int, TimeDataForCycle>();
			timeDataForCyclePool = new Stack<TimeDataForCycle>();
		}

		private void Update()
		{
			if (IsGamePaused)
			{
				return;
			}
			switch (timelineState)
			{
				case TimelineState.Flowing:
					UpdateInternalTime();
					timelineState = TimelineState.Recording;
					/*if (!IsTimeFrozen && (cycleNumber % 2 == 0 || cycleNumber == 0))
					{
						timelineState = TimelineState.Recording;
					}*/
					break;
				case TimelineState.Recording:
					UpdateInternalTime();
					//timelineState = TimelineState.Flowing;
					break;
				case TimelineState.RecordingAndRewindInitiated:
				// Fall through
				case TimelineState.RewindInitiated:
					if (IsTimeOrGamePaused)
					{
						timelineState = TimelineState.Rewinding;
					}
					else
					{
						UpdateInternalTime();
						//timelineState = TimelineState.Flowing;
						timelineState = TimelineState.Recording;
					}
					break;
				case TimelineState.Rewinding:
					if (cycleNumber > oldestCycleWithinRewindLimit)
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
			if (!IsTimeOrGamePaused)
			{
				fixedDeltaTime = Time.fixedDeltaTime;
				fixedTime += fixedDeltaTime;
			}
		}

		private void UpdateInternalTime()
		{
			switch (timePauseState)
			{
				case TimePauseChangeState.NotChangedRecently:
					break;
				case TimePauseChangeState.ChangedLastUpdate:
					timePauseState = TimePauseChangeState.NotChangedRecently;
					break;
				case TimePauseChangeState.ChangedThisUpdate:
					timePauseState = TimePauseChangeState.ChangedLastUpdate;
					break;
				case TimePauseChangeState.ChangedThisUpdateAndLastUpdate:
					timePauseState = TimePauseChangeState.ChangedLastUpdate;
					break;
				default:
					Debug.LogWarning("Unhandled TimeFreezeState ID:" + (int)timePauseState);
					break;
			}
			switch (excessDataClearingState)
			{
				case ExcessDataClearingState.ClearingNextUpdate:
					excessDataClearingState = ExcessDataClearingState.Clearing;
					ClearExcessData();
					break;
				case ExcessDataClearingState.Clearing:
					excessDataClearingState = ExcessDataClearingState.None;
					break;
				default:
					break;
			}
			if (timeDataForCycles.Count > 400)
			{
				InitiateExcessDataClearing();
			}
			if (IsTimeOrGamePaused)
			{
				return;
			}
			deltaTime = Time.deltaTime;
			time += deltaTime;
			cycleNumber++;
			newestRecordedCycle = cycleNumber;
			TimeDataForCycle td = timeDataForCyclePool.Count > 0 ? timeDataForCyclePool.Pop() : new TimeDataForCycle();
			td.time = time;
			td.deltaTime = deltaTime;
			td.fixedTime = fixedTime;
			td.fixedDeltaTime = fixedDeltaTime;
			timeDataForCycles[cycleNumber] = td;
			for (int cn = newestRecordedCycle; cn >= 0; cn--)
			{
				// When older timeline data has been cleared
				if (!timeDataForCycles.ContainsKey(cn))
				{
					break;
				}
				if (timeDataForCycles[newestRecordedCycle].time - timeDataForCycles[cn].time <= rewindTimeLimit)
				{
					oldestCycleWithinRewindLimit = cn;
				}
				else
				{
					break;
				}
			}
		}

		private void SetCurrentCycle(int newCycleNumber)
		{
			timePauseState = TimePauseChangeState.NotChangedRecently;
			cycleNumber = newCycleNumber;
			TimeDataForCycle td = timeDataForCycles[cycleNumber];
			time = td.time;
			deltaTime = td.deltaTime;
			fixedTime = td.fixedTime;
			fixedDeltaTime = td.fixedDeltaTime;
		}

		/**<summary>The states of time freeze relating to how recently it has
		 * been changed.</summary>
		 */
		private enum TimePauseChangeState
		{
			NotChangedRecently = 0,
			ChangedThisUpdate,
			ChangedLastUpdate,
			ChangedThisUpdateAndLastUpdate
		}

		private enum ExcessDataClearingState
		{
			None = 0,
			ClearingNextUpdate,
			Clearing
		}

		private enum TimelineState
		{
			/**<summary>Nothing should be done with timelines, and time is
			 * moving forward like normal or is frozen.</summary>
			 */
			Flowing = 0,
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

		private class TimeDataForCycle
		{
			public float time;
			public float deltaTime;
			public float fixedTime;
			public float fixedDeltaTime;
		}
	}
}
