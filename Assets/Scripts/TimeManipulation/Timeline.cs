using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Base timeline class, implements all instance-independent behaviour
	 * and properties for easy access.</summary>
	 */
	public abstract class Timeline
	{
		/**<summary>The number of records that can be contained in a timeline.</summary>*/
		public static readonly int timelineRecordCapacity;

		/**<summary>The index of the current cycle in the array/list of records.</summary>*/
		public static int currentCycleIndex;
		/**<summary>The cycle number corresponding to index 0 in the array/list
		 * of records.</summary>
		 */
		protected static int cycleNumberAtFirstIndex;
		/**<summary>The cycle number corresponding to the last index in the array/list
		 * of records.</summary>
		 */
		protected static int cycleNumberAtLastIndex;

		static Timeline()
		{
			timelineRecordCapacity = Mathf.CeilToInt(ManipulableTime.rewindTimeLimit / 0.008f);
			Reset();
		}

		/**<summary>Update the shared/static timeline values so they line up with
		 * the next cycle. The timeline values may not be properly configured until
		 * the first time this is called, so do not attempt to read or manipulate
		 * a timeline instance until this has been called at least once.</summary>
		 */
		public static void AdvanceToNextCycle()
		{
			if (currentCycleIndex < 0)
			{
				currentCycleIndex = 0;
				return;
			}
			currentCycleIndex = (currentCycleIndex + 1) % timelineRecordCapacity;
			if (currentCycleIndex == 0)
			{
				cycleNumberAtFirstIndex = ManipulableTime.cycleNumber + 1;
			}
			else if (currentCycleIndex == timelineRecordCapacity - 1)
			{
				cycleNumberAtLastIndex = ManipulableTime.cycleNumber + 1;
			}
		}

		public static void Reset()
		{
			currentCycleIndex = -1;
			cycleNumberAtFirstIndex = 0;
			cycleNumberAtLastIndex = -2;
		}

		public static void ShiftCurrentCycle(int deltaCycles)
		{
			if (deltaCycles > 0)
			{
				currentCycleIndex = (currentCycleIndex + deltaCycles) % timelineRecordCapacity;
			}
			else if (deltaCycles < 0)
			{
				if (currentCycleIndex >= Mathf.Abs(deltaCycles))
				{
					currentCycleIndex += deltaCycles;
				}
				else
				{
					currentCycleIndex = timelineRecordCapacity + currentCycleIndex + deltaCycles;
					while (currentCycleIndex < 0)
					{
						currentCycleIndex = timelineRecordCapacity - currentCycleIndex;
					}
				}
			}
		}
	}

	/**<summary></summary>*/
	public class Timeline<T> : Timeline where T : TimelineRecord, new()
	{
		public readonly int timelineCreatedDuringCycle;

		/**<summary>The array of records.</summary>*/
		private T[] recordLoop;

		public Timeline()
		{
			timelineCreatedDuringCycle = ManipulableTime.cycleNumber;
			recordLoop = new T[timelineRecordCapacity];
			PopulateRecordArray(0, recordLoop.Length - 1);
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

		private void PopulateRecordArray(int startIndex, int endIndex)
		{
			for (int i = startIndex; i <= endIndex; i++)
			{
				recordLoop[i] = new T();
			}
		}
	}
}
