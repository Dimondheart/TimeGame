using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Base class for components that handle controlled motion
 * from the player or an AI.</summary>
 */
public abstract class ControlledMovement : MonoBehaviour, ITimelineRecordable
{
	public PhysicsMaterial2D stationaryMaterial;
	public PhysicsMaterial2D applyingMotionMaterial;

	private bool isApplyingMotion = false;
	
	public bool IsApplyingMotion
	{
		get
		{
			return isApplyingMotion;
		}
		protected set
		{
			if (ManipulableTime.ApplyingTimelineRecords)
			{
				return;
			}
			isApplyingMotion = value;
			if (value)
			{
				GetComponent<Rigidbody2D>().sharedMaterial = applyingMotionMaterial;
			}
			else
			{
				GetComponent<Rigidbody2D>().sharedMaterial = stationaryMaterial;
			}
		}
	}

	public abstract TimelineRecord MakeTimelineRecord();
	public abstract void ApplyTimelineRecord(TimelineRecord record);

	protected void AddTimelineRecordValues(TimelineRecord_ControlledMovement record)
	{
		record.isApplyingMotion = isApplyingMotion;
	}

	protected void ApplyTimelineRecordValues(TimelineRecord_ControlledMovement record)
	{
		isApplyingMotion = record.isApplyingMotion;
	}

	public abstract class TimelineRecord_ControlledMovement : TimelineRecordForComponent
	{
		public bool isApplyingMotion;
	}
}
