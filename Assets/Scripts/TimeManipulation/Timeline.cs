using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Contains all Recorded data for a single game object
 * to be used while rewinding.</summary>
 */
	public class Timeline
	{
		private Dictionary<int, TimelineSnapshot> snapshots = new Dictionary<int, TimelineSnapshot>();

		public int oldestSnapshot { get; private set; }
		public int newestSnapshot { get; private set; }
		public bool HasAtLeastOneSnapshot
		{
			get
			{
				return oldestSnapshot != -1;
			}
		}

		public Timeline()
		{
			oldestSnapshot = -1;
			newestSnapshot = -1;
		}

		public bool HasSnapshot(int cycleNumber)
		{
			return snapshots.ContainsKey(cycleNumber);
		}

		public void AddSnapshot(int cycleNumber, TimelineSnapshot snapshot)
		{
			snapshots[cycleNumber] = snapshot;
			if (oldestSnapshot == -1)
			{
				oldestSnapshot = cycleNumber;
			}
			newestSnapshot = cycleNumber;
		}

		public void ApplySnapshot(int cycleNumber)
		{
			TimelineSnapshot snapshot = snapshots[cycleNumber];
			if (snapshot == null)
			{
				Debug.LogWarning(
					"Attempted to apply a snapshot number not found in" +
					"the timeline:" +
					cycleNumber
					);
			}
			else
			{
				snapshot.ApplyRecords();
			}
		}

		/**<summary>Clear/Remove all snapshots outside the specified range. The
		 * two snapshots that fall on the specifed bounds will be kept.</summary>
		 */
		public void ClearSnapshotsOutsideRange(int oldestInclusive, int newestInclusive)
		{
			/*
			if (debugThing != null && debugThing.name == "Player")
			{
				Debug.Log("Before2:" + snapshots.Count);
			}
			*/
			for (int cn = oldestInclusive - 1; cn >= oldestSnapshot; cn--)
			{
				if (!snapshots.ContainsKey(cn))
				{
					continue;
				}
				snapshots[cn].ClearRecords();
				snapshots.Remove(cn);
			}
			oldestSnapshot = oldestInclusive;
			for (int cn = newestInclusive + 1; cn <= newestSnapshot; cn++)
			{
				if (!snapshots.ContainsKey(cn))
				{
					continue;
				}
				snapshots[cn].ClearRecords();
				snapshots.Remove(cn);
			}
			newestSnapshot = newestInclusive;
			/*
			if (debugThing.name == "Player")
			{
				Debug.Log("After2:" + snapshots.Count);
			}
			*/
		}
	}
}
