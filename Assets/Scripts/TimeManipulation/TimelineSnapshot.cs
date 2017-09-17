using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
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
					TimelineRecordForComponent.ApplyTimelineRecord(kvp.Key, (TimelineRecordForComponent)kvp.Value);
				}
				kvp.Value.ApplyCommonData(kvp.Key);
			}
		}
	}
}
