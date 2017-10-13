using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Base class for components that handle controlled motion
	 * from the player or an AI.</summary>
	 */
	public abstract class ControlledMovement<T> : RecordableMonoBehaviour<T>
		where T : TimelineRecord_ControlledMovement, new()
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

		protected override void WriteCurrentState(T record)
		{
			record.isApplyingMotion = isApplyingMotion;
		}

		protected override void ApplyRecordedState(T record)
		{
			isApplyingMotion = record.isApplyingMotion;
		}
	}

	public abstract class TimelineRecord_ControlledMovement : TimelineRecordForBehaviour
	{
		public bool isApplyingMotion;
	}
}
