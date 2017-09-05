using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuard : MonoBehaviour, ITimelineRecordable
{
	/**<summary>Color to make the player sprite while guarding, a placeholder
	 * for animations.</summary>
	 */
	public Color colorDuringAction;

	TimelineRecord ITimelineRecordable.MakeTimelineRecord()
	{
		TimelineRecord_PlayerGuard record = new TimelineRecord_PlayerGuard();
		record.colorDuringAction = colorDuringAction;
		return record;
	}

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_PlayerGuard rec = (TimelineRecord_PlayerGuard)record;
		colorDuringAction = rec.colorDuringAction;
	}

	private void Update()
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		if (ManipulableTime.IsTimeFrozen)
		{
			return;
		}
		if (DynamicInput.GetButton("Guard"))
		{
			//GetComponent<SpriteColorChanger>().SpriteColor = colorDuringAction;
			//GetComponent<Health>().takeDamage = false;
		}
		else
		{
			GetComponent<Health>().takeDamage = true;
			if (!DynamicInput.GetButton("Melee"))
			{
				GetComponent<SpriteColorChanger>().SpriteColor = Color.white;
			}
		}
	}

	public class TimelineRecord_PlayerGuard : TimelineRecordForComponent
	{
		public Color colorDuringAction;
	}
}
