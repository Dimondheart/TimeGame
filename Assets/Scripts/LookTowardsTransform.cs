using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

/**<summary>Look towards a specified transform.</summary>*/
public class LookTowardsTransform : MonoBehaviour, ITimelineRecordable
{
	/**<summary>The transform to look towards.</summary>*/
	public Transform lookTowards;
	/**<summary>Max distance to look towards the transform.</summary>*/
	public float maxDistance = 6.0f;

	TimelineRecord ITimelineRecordable.MakeTimelineRecord()
	{
		TimelineRecord_LookTowardsTransform record = new TimelineRecord_LookTowardsTransform();
		record.lookTowards = lookTowards;
		record.maxDistance = maxDistance;
		return record;
	}

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_LookTowardsTransform rec = (TimelineRecord_LookTowardsTransform)record;
		lookTowards = rec.lookTowards;
		maxDistance = rec.maxDistance;
	}

	private void Update()
	{
		if (ManipulableTime.ApplyingTimelineRecords || ManipulableTime.IsTimeFrozen || !GetComponent<Health>().IsAlive)
		{
			return;
		}
		Vector2 lookVector = lookTowards.position - transform.position;
		if (lookVector.magnitude <= maxDistance)
		{
			GetComponent<DirectionLooking>().Direction = lookVector;
		}
	}

	public class TimelineRecord_LookTowardsTransform : TimelineRecordForComponent
	{
		public Transform lookTowards;
		public float maxDistance;
	}
}
