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

		TimelineRecord ITimelineRecordable.MakeTimelineRecord()
		{
			TimelineRecord_DirectionLooking record = new TimelineRecord_DirectionLooking();
			record.direction = direction;
			return record;
		}

		void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
		{
			TimelineRecord_DirectionLooking r = (TimelineRecord_DirectionLooking)record;
			direction = r.direction;
		}

		public class TimelineRecord_DirectionLooking : TimelineRecordForComponent
		{
			public Vector2 direction;
		}
	}
}
