using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary></summary>*/
	public class Timeline<T> where T : TimelineRecord, new()
	{
		public static readonly int timelineRecordCapacity =
			Mathf.CeilToInt(ManipulableTime.rewindTimeLimit / 0.008f);

		private static int currentCycleIndex = 0;
		private static int cycleNumberAtFirstIndex = 0;
		private static int cycleNumberAtLastIndex = -1;

		public readonly int timelineCreatedDuringCycle;

		private T[] recordLoop;

		public Timeline()
		{
			timelineCreatedDuringCycle = ManipulableTime.cycleNumber;
			recordLoop = new T[timelineRecordCapacity];
			for (int i = 0; i < recordLoop.Length; i++)
			{
				recordLoop[i] = new T();
			}
		}

		/**<summary>Update the shared/static timeline values so they line up with
		 * the next cycle.</summary>
		 */
		public static void MoveToNextCycle()
		{
			currentCycleIndex = (currentCycleIndex + 1) % timelineRecordCapacity;
			if (currentCycleIndex == 0)
			{
				cycleNumberAtFirstIndex = ManipulableTime.cycleNumber;
			}
			else if (currentCycleIndex == timelineRecordCapacity - 1)
			{
				cycleNumberAtLastIndex = ManipulableTime.cycleNumber;
			}
		}

		/**<summary>Get the TimelineRecord for the current cycle.</summary>*/
		public T GetRecordForCurrentCycle()
		{
			return recordLoop[currentCycleIndex];
		}

		public bool HasRecord(int cycle)
		{
			return GetIndexOfCycle(cycle) >= 0;
		}

		/**<summary>Get the record for the specified cycle. Does not verify that the
		 * specified cycle has been recorded into this timeline. Use HasRecord(int cycle)
		 * to check if this timeline has a record for the given cycle. An exception will occur
		 * if the record does not exist.</summary>
		 */
		public T GetRecord(int cycle)
		{
			return recordLoop[GetIndexOfCycle(cycle)];
		}

		private int GetIndexOfCycle(int cycle)
		{
			if (cycle < timelineCreatedDuringCycle)
			{
				return -1;
			}
			else if (cycle >= cycleNumberAtFirstIndex)
			{
				int index = cycle - cycleNumberAtFirstIndex;
				if (index <= currentCycleIndex)
				{
					return index;
				}
				// otherwise cycle has not been recorded yet
			}
			else if (cycle <= cycleNumberAtLastIndex)
			{
				int index = recordLoop.Length - 1 - (cycleNumberAtLastIndex - cycle);
				if (index > currentCycleIndex)
				{
					return index;
				}
				// Otherwise cycle record has aleady been recorded over by another cycle
			}
			return -1;
		}
	}
}
