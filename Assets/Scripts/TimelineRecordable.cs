using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Interface for all custom monobehaviors that support creating
 * and using timeline records.</summary>
 */
public interface ITimelineRecordable
{
	TimelineRecord MakeTimelineRecord();
	void ApplyTimelineRecord(TimelineRecord record);
}
