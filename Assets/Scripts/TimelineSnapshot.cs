using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Contains all TimelineRecord objects for a single game object
 * at a single instance in time.</summary>
 */
public class TimelineSnapshot
{
	private Dictionary<Component, TimelineRecord> records = new Dictionary<Component, TimelineRecord>();

	public void AddRecord(Component component, TimelineRecord record)
	{
		records[component] = record;
	}

	public void ApplyRecords()
	{
		foreach (KeyValuePair<Component, TimelineRecord> kvp in records)
		{
			if (kvp.Key is ITimelineRecordable)
			{
				((ITimelineRecordable)kvp.Key).ApplyTimelineRecord(kvp.Value);
			}
			else
			{
				TimelineRecord.ApplyTimelineRecord(kvp.Key, kvp.Value);
			}
		}
	}

	public void ClearRecords()
	{
		records.Clear();
	}
}
