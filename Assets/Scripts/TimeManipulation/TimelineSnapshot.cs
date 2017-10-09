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
		public readonly GameObject gameObject;
		public TimelineRecordForGameObject gameObjectRecord;
		private Dictionary<Component, TimelineRecordForBehaviour> records =
			new Dictionary<Component, TimelineRecordForBehaviour>();

		public TimelineSnapshot(GameObject gameObject)
		{
			this.gameObject = gameObject;
		}

		public bool HasRecord(Component component)
		{
			return records.ContainsKey(component);
		}

		public TimelineRecordForBehaviour GetRecord(Component component)
		{
			return records[component];
		}

		public void AddRecord(Component component, TimelineRecordForBehaviour record)
		{
			records[component] = record;
		}

		public void ApplyRecords()
		{
			gameObjectRecord.ApplyCommonData(gameObject);
			foreach (KeyValuePair<Component, TimelineRecordForBehaviour> kvp in records)
			{
				kvp.Value.ApplyRecord();
			}
		}
	}
}
