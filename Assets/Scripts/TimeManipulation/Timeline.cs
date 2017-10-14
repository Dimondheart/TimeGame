using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>A timeline contains all records for a single GameObject,
	 * Component, etc. and also synchronizes all timelines so the cycles
	 * corresponding to the record at each position are the same.</summary>
	 */
	public class Timeline
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

		/**<summary>What ManipulableTime cycle this timeline was created during.
		 * A value less than 0 indicates the timeline was created before the first
		 * cycle (such as in a constructor.)</summary>
		 */
		public readonly int timelineCreatedDuringCycle;
		/**<summary>The Type of the record this timeline contains. Used to
		 * populate the internal storage of records when the timeline is first
		 * created, or when the timeline is resized.</summary>
		 */
		public readonly Type recordType;
		/**<summary>The array of records.</summary>*/
		private TimelineRecord[] recordLoop;

		static Timeline()
		{
			timelineRecordCapacity = Mathf.CeilToInt(ManipulableTime.rewindTimeLimit / 0.008f);
			Reset();
		}

		/**<summary>The type parameter is either the type of the timeline record,
		 * or the type of the class containing the definition for the timeline record.
		 * The typeParamIsRecordType indicates if the type parameter is the record
		 * type itself or the record definition is nested in the given type.</summary>
		 * <param name="type">Either the type of the timeline record, or the type of
		 * the class containing the definition for the timeline record.</param>
		 * <param name="typeParamIsRecordType">True if the type param is the type of
		 * the record, false if it is the type of the class containing the timeline
		 * record declaration.</param>
		 */
		public Timeline(System.Type type, bool typeParamIsRecordType)
		{
			timelineCreatedDuringCycle = ManipulableTime.cycleNumber;
			recordLoop = new TimelineRecord[timelineRecordCapacity];
			if (typeParamIsRecordType)
			{
				recordType = type;
			}
			else
			{
				Type[] nestedTypes = type.GetNestedTypes();
				foreach (Type nestedType in nestedTypes)
				{
					if (nestedType.IsSubclassOf(typeof(TimelineRecord)) && nestedType.IsSealed)
					{
						recordType = nestedType;
						break;
					}
				}
			}
			PopulateRecordArray(0, recordLoop.Length - 1);
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

		/**<summary>Get the TimelineRecord for the current cycle.</summary>*/
		public TimelineRecord GetRecordForCurrentCycle()
		{
			return recordLoop[currentCycleIndex];
		}

		/**<summary>Check if the timeline contains the specified record.</summary>*/
		public bool HasRecord(int cycle)
		{
			return GetIndexOfCycle(cycle) >= 0;
		}

		/**<summary>Get the record for the specified cycle. Does not verify that the
		 * specified cycle has been recorded into this timeline. Use HasRecord(int cycle)
		 * to check if this timeline has a record for the given cycle. An exception will occur
		 * if the record does not exist.</summary>
		 */
		public TimelineRecord GetRecord(int cycle)
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
				recordLoop[i] = (TimelineRecord)Activator.CreateInstance(recordType);
			}
		}
	}
}
