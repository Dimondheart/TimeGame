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
		private static Dictionary<int, TimelineRecord_ManipulableTime> timeDataForCycles;
		private static Stack<TimelineRecord_ManipulableTime> timeDataForCyclePool;

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
		public static TimePauseState timePauseState { get; private set; }
		private static Timeline timeline;
		private static int shiftTimelineAmount;
		/**<summary>Indicates and controls if the manipulable time is frozen. This cannnot
		 * be changed while the game is paused.</summary>
		 */
		public static bool IsTimePaused
		{
			get
			{
				return TimePauseState.Paused == (timePauseState & TimePauseState.Paused);
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
		/**<summary>Check if manipulable time is paused and/or the game is paused.</summary>*/
		public static bool IsTimeOrGamePaused
		{
			get
			{
				return IsTimePaused || IsGamePaused;
			}
		}
		/**<summary>Check if records will be taken at the end of the current
		 * cycle.</summary>
		 */
		public static bool IsRecording
		{
			get
			{
				return (timePauseState & TimePauseState.Flowing) == TimePauseState.Flowing;
			}
		}
		/**<summary>Check if time is currently rewinding through past events.</summary>*/
		public static bool IsRewinding
		{
			get
			{
				return TimePauseState.Rewinding == (timePauseState & TimePauseState.Rewinding);
			}
		}
		/**<summary>Check if time is currently playing back past events.</summary>*/
		public static bool IsReplaying
		{
			get
			{
				return TimePauseState.Replaying == (timePauseState & TimePauseState.Replaying);
			}
		}
		/**<summary>Check if time is rewinding or replaying.</summary>*/
		public static bool IsApplyingRecords
		{
			get
			{
				return IsRewinding || IsReplaying;
			}
		}

		/**<summary>Change if time is paused or not. Will not be changed until the
		 * next Update, meaning future calls to this method can prevent the
		 * change.</summary>
		 */
		public static void ChangeTimePaused(bool paused)
		{
			if (paused == IsTimePaused)
			{
				return;
			}
			if (TimePauseState.ChangingNextCycle == (timePauseState & TimePauseState.ChangingNextCycle))
			{
				timePauseState = timePauseState & (~TimePauseState.ChangingNextCycle);
			}
			else
			{
				timePauseState = timePauseState | TimePauseState.ChangingNextCycle;
			}
			// TODO move the following to update
			if (!paused)
			{
				StopRewind();
				StopReplay();
			}
		}

		/**<summary>Set time to start rewinding next update.</summary>*/
		public static void StartRewind()
		{
			timePauseState =
				(timePauseState | TimePauseState.RewindRequest) & (~TimePauseState.ReplayRequest);
		}

		/**<summary>Stop rewinding time next update.</summary>*/
		public static void StopRewind()
		{
			timePauseState = timePauseState & (~TimePauseState.RewindRequest);
		}

		/**<summary>Set time to start replaying next update.</summary>*/
		public static void StartReplay()
		{
			timePauseState =
				(timePauseState | TimePauseState.ReplayRequest) & (~TimePauseState.RewindRequest);
		}

		/**<summary>Stop replaying time next update.</summary>*/
		public static void StopReplay()
		{
			timePauseState = timePauseState & (~TimePauseState.ReplayRequest);
		}

		private void Awake()
		{
			timePauseState = TimePauseState.Flowing;
			cycleNumber = -1;
			oldestRecordedCycle = -1;
			newestRecordedCycle = -1;
			oldestCycleWithinRewindLimit = -1;
			time = Time.time;
			deltaTime = Time.deltaTime;
			fixedTime = Time.fixedTime;
			fixedDeltaTime = Time.fixedDeltaTime;
			timeline = new Timeline(typeof(TimelineRecord_ManipulableTime), true);
			shiftTimelineAmount = 0;
		}

		private void Start()
		{
			time = Time.time;
			deltaTime = Time.deltaTime;
			fixedTime = Time.fixedTime;
			fixedDeltaTime = Time.fixedDeltaTime;
		}

		private void Update()
		{
			if (IsGamePaused)
			{
				return;
			}
			if (TimePauseState.Flowing == (timePauseState & TimePauseState.Flowing))
			{
				if (TimePauseState.PausingNextCycle == (timePauseState & TimePauseState.PausingNextCycle))
				{
					timePauseState =
						(timePauseState | TimePauseState.Paused | TimePauseState.JustChanged)
						& (~TimePauseState.Flowing)
						& (~TimePauseState.ChangingNextCycle)
						;
				}
				else
				{
					if (TimePauseState.JustResumed == (timePauseState & TimePauseState.JustResumed))
					{
						timePauseState = timePauseState & (~TimePauseState.JustChanged);
					}
					UpdateInternalTime();
				}
			}
			else
			{
				if (TimePauseState.ResumingNextCycle == (timePauseState & TimePauseState.ResumingNextCycle))
				{
					timePauseState =
						(timePauseState | TimePauseState.Flowing | TimePauseState.JustChanged)
						& (~TimePauseState.Paused)
						& (~TimePauseState.ChangingNextCycle)
						;
					StopRewind();
					StopReplay();
					UpdateInternalTime();
				}
				else
				{
					if (TimePauseState.JustPaused == (timePauseState & TimePauseState.JustPaused))
					{
						timePauseState = timePauseState & (~TimePauseState.JustChanged);
					}
					if (TimePauseState.Rewinding == (timePauseState & TimePauseState.Rewinding))
					{
						if (cycleNumber > oldestCycleWithinRewindLimit)
						{
							SetCurrentCycle(cycleNumber - 1);
						}
					}
					else if (TimePauseState.Replaying == (timePauseState & TimePauseState.Replaying))
					{
						if (cycleNumber < newestRecordedCycle)
						{
							SetCurrentCycle(cycleNumber + 1);
						}
					}
				}
			}
			if (!timeline.HasRecord(ManipulableTime.cycleNumber))
			{
				Debug.LogWarning("manipulable time and timeline cycle number are not equal");
			}
		}

		private void FixedUpdate()
		{
			if (IsTimeOrGamePaused)
			{
				return;
			}
			fixedDeltaTime = Time.fixedDeltaTime;
			fixedTime += fixedDeltaTime;
		}

		private void UpdateInternalTime()
		{
			Timeline.ShiftCurrentCycle(shiftTimelineAmount);
			shiftTimelineAmount = 0;
			if (cycleNumber < 0)
			{
				cycleNumber = 0;
				oldestRecordedCycle = cycleNumber;
				oldestCycleWithinRewindLimit = cycleNumber;
			}
			else
			{
				deltaTime = Time.deltaTime;
				time += deltaTime;
				cycleNumber++;
			}
			newestRecordedCycle = cycleNumber;
			TimelineRecord_ManipulableTime rec =
				(TimelineRecord_ManipulableTime)timeline.GetRecordForCurrentCycle();
			rec.time = time;
			rec.deltaTime = deltaTime;
			rec.fixedTime = fixedTime;
			rec.fixedDeltaTime = fixedDeltaTime;
			if (cycleNumber >= Timeline.timelineRecordCapacity)
			{
				oldestRecordedCycle++;
			}
			for (int cn = oldestRecordedCycle; cn <= newestRecordedCycle; cn++)
			{
				oldestCycleWithinRewindLimit = cn;
				if (time - ((TimelineRecord_ManipulableTime)timeline.GetRecord(cn)).time <= rewindTimeLimit)
				{
					break;
				}
			}
		}

		private void SetCurrentCycle(int newCycleNumber)
		{
			TimelineRecord_ManipulableTime rec =
				(TimelineRecord_ManipulableTime)timeline.GetRecord(newCycleNumber);
			time = rec.time;
			deltaTime = rec.deltaTime;
			fixedTime = rec.fixedTime;
			fixedDeltaTime = rec.fixedDeltaTime;
			shiftTimelineAmount += newCycleNumber - cycleNumber;
			cycleNumber = newCycleNumber;
		}

		public enum TimePauseState : byte
		{
			Flowing = 1,
			Paused = 2,
			JustChanged = 4,
			ChangingNextCycle = 8,
			RewindRequest = 16,
			ReplayRequest = 32,

			JustResumed = Flowing | JustChanged,
			JustPaused = Paused | JustChanged,
			ResumingNextCycle = Paused | ChangingNextCycle,
			PausingNextCycle = Flowing | ChangingNextCycle,
			Rewinding = RewindRequest | Paused,
			Replaying = ReplayRequest | Paused
		}

		private class TimelineRecord_ManipulableTime : TimelineRecord<ManipulableTime>
		{
			public float time;
			public float deltaTime;
			public float fixedTime;
			public float fixedDeltaTime;
		}
	}
}
