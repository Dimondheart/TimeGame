using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Base class for components that handle controlled motion
	 * from the player or an AI.</summary>
	 */
	public abstract class ControlledMovement : RecordableMonoBehaviour
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

		public abstract class TimelineRecord_ControlledMovement<T> : TimelineRecordForBehaviour<T>
			where T : ControlledMovement
		{
			public bool isApplyingMotion;

			protected override void WriteCurrentState(T cm)
			{
				isApplyingMotion = cm.isApplyingMotion;
			}

			protected override void ApplyRecordedState(T cm)
			{
				cm.isApplyingMotion = isApplyingMotion;
			}
		}
	}
}
