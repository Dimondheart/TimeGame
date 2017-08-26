using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
