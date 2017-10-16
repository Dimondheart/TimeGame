using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Defines the direction something is facing without
	 * applying a transform rotation.</summary>
	 */
	public class DirectionLooking : RecordableMonoBehaviour
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

		public sealed class TimelineRecord_DirectionLooking : TimelineRecordForBehaviour<DirectionLooking>
		{
			public Vector2 direction;

			protected override void RecordState(DirectionLooking dl)
			{
				base.RecordState(dl);
				direction = dl.direction;
			}

			protected override void ApplyRecord(DirectionLooking dl)
			{
				base.ApplyRecord(dl);
				dl.direction = direction;
			}
		}
	}
}
