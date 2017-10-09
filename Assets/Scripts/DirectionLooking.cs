using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Defines the direction something is facing without
	 * applying a transform rotation.</summary>
	 */
	public class DirectionLooking : MonoBehaviour, ITimelineRecordable
	{
		private Vector2 direction = Vector2.down;

		public Vector2 Direction
		{
			get
			{
				return direction;
			}
			set
			{
				direction = value;
				direction.Normalize();
			}
		}

		public float Angle
		{
			get
			{
				float angle = Vector2.Angle(Direction, Vector2.up);
				return Direction.x < 0 ? angle : -angle;
			}
		}

		TimelineRecordForBehaviour ITimelineRecordable.MakeTimelineRecord()
		{
			return new TimelineRecord_DirectionLooking();
		}

		void ITimelineRecordable.RecordCurrentState(TimelineRecordForBehaviour record)
		{
			TimelineRecord_DirectionLooking rec = (TimelineRecord_DirectionLooking)record;
			rec.direction = direction;
		}

		void ITimelineRecordable.ApplyTimelineRecord(TimelineRecordForBehaviour record)
		{
			TimelineRecord_DirectionLooking r = (TimelineRecord_DirectionLooking)record;
			direction = r.direction;
		}

		public class TimelineRecord_DirectionLooking : TimelineRecordForBehaviour
		{
			public Vector2 direction;
		}
	}
}
