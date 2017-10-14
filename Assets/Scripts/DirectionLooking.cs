using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Defines the direction something is facing without
	 * applying a transform rotation.</summary>
	 */
	public class DirectionLooking : RecordableMonoBehaviour<DirectionLooking.TimelineRecord_DirectionLooking>
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

		protected override void RecordCurrentState(TimelineRecord_DirectionLooking record)
		{
			System.Type[] ts = GetType().GetNestedTypes();
			if (ts == null)
			{
				Debug.Log("null");
			}
			else
			{
				Debug.Log("hello");

				foreach (System.Type t in ts)
				{
					Debug.Log(t.IsSubclassOf(typeof(TimelineRecord)));
				}

				//TimelineRecord re = (TimelineRecord)System.Activator.CreateInstance(t);
			}
			record.direction = direction;
		}

		protected override void ApplyRecordedState(TimelineRecord_DirectionLooking record)
		{
			direction = record.direction;
		}

		public class Test
		{

		}

		public class TimelineRecord_DirectionLooking : TimelineRecordForBehaviour
		{
			public Vector2 direction;
		}
	}
}
