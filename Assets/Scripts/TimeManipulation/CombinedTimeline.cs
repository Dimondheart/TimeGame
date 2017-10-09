using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Contains all Recorded data for a single game object
 * to be used while rewinding.</summary>
 */
	public class CombinedTimeline
	{
		public readonly GameObject gameObject;

		private Dictionary<int, TimelineSnapshot> snapshots = new Dictionary<int, TimelineSnapshot>();
		private Stack<TimelineSnapshot> snapshotPool = new Stack<TimelineSnapshot>();

		public int oldestSnapshot { get; private set; }
		public int newestSnapshot { get; private set; }
		public bool HasAtLeastOneSnapshot
		{
			get
			{
				return oldestSnapshot != -1;
			}
		}

		public CombinedTimeline(GameObject gameObject)
		{
			oldestSnapshot = -1;
			newestSnapshot = -1;
			this.gameObject = gameObject;
		}

		public TimelineSnapshot GetSnapshotForRecording()
		{
			if (snapshotPool.Count > 0)
			{
				return snapshotPool.Pop();
			}
			return new TimelineSnapshot(gameObject);
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

		/**<summary>Return all snapshots outside the specified range to the pool. The
		 * two snapshots that fall on the specifed bounds will be kept.</summary>
		 */
		public void RemoveSnapshotsOutsideRange(int oldestInclusive, int newestInclusive)
		{
			for (int cn = oldestInclusive - 1; cn >= oldestSnapshot; cn--)
			{
				if (!snapshots.ContainsKey(cn))
				{
					continue;
				}
				MoveSnapshotToPool(cn);
			}
			oldestSnapshot = oldestInclusive;
			for (int cn = newestInclusive + 1; cn <= newestSnapshot; cn++)
			{
				if (!snapshots.ContainsKey(cn))
				{
					continue;
				}
				MoveSnapshotToPool(cn);
			}
			newestSnapshot = newestInclusive;
		}

		private void MoveSnapshotToPool(int cycleNumber)
		{
			snapshotPool.Push(snapshots[cycleNumber]);
			snapshots.Remove(cycleNumber);
		}
	}
}
