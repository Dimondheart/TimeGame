using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Surface interactions (like friction) and information
	 * relating to touching surfaces.</summary>
	 */
	public class SurfaceInteraction : RecordableMonoBehaviour
	{
		/**<summary>Velocity multiplier when not touching a specific surface
		 * (meaning outside the map/play area.)</summary>
		 */
		public static readonly float defaultVelocityMultiplier = 0.08f;

		/**<summary>Resistance to friction effects. 0 is no resistance, 1
		 * means friction has no effect.</summary>
		 */
		public float frictionResistance = 0.0f;

		private List<Surface> touchingSurfaces = new List<Surface>();

		public bool IsSwimming
		{
			get
			{
				if (touchingSurfaces.Count <= 0)
				{
					return true;
				}
				foreach (Surface s in touchingSurfaces)
				{
					if (!s.IsLiquid || s.surfaceType == Surface.SurfaceType.ShallowWater)
					{
						return false;
					}
				}
				return true;
			}
		}

		protected override void FlowingUpdate()
		{
			/*if (GetComponent<Health>() == null
				|| !GetComponent<Health>().IsAlive
				|| (GetComponent<ControlledMovement<TimelineRecord_ControlledMovement>>() != null && !GetComponent<ControlledMovement<TimelineRecord_ControlledMovement>>().IsApplyingMotion)
				|| (GetComponent<PlayerMovement>() != null && !GetComponent<PlayerMovement>().IsApplyingMotion)
				)
			{
				return;
			}*/
			float multiplier = defaultVelocityMultiplier;
			if (touchingSurfaces.Count > 0)
			{
				multiplier = Surface.ResultingVelocityMultiplier(touchingSurfaces);
			}
			multiplier = Mathf.Clamp01(multiplier + frictionResistance * (1.0f - multiplier));
			GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * multiplier;
		}

		protected override void FlowingFixedUpdate()
		{
			/*
			if ((GetComponent<ControlledMovement<TimelineRecord_ControlledMovement>>() != null && GetComponent<ControlledMovement<TimelineRecord_ControlledMovement>>().IsApplyingMotion)
				|| (GetComponent<PlayerMovement>() != null && GetComponent<PlayerMovement>().IsApplyingMotion)
				)
			{
				return;
			}
			*/
			float multiplier = defaultVelocityMultiplier;
			if (touchingSurfaces.Count > 0)
			{
				multiplier = Surface.ResultingVelocityMultiplier(touchingSurfaces);
			}
			multiplier = Mathf.Clamp01(multiplier + frictionResistance * (1.0f - multiplier));
			GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * (multiplier * ManipulableTime.fixedDeltaTime);
		}

		public void AddSurface(Surface surface)
		{
			if (!touchingSurfaces.Contains(surface))
			{
				touchingSurfaces.Add(surface);
			}
		}

		public void RemoveSurface(Surface surface)
		{
			touchingSurfaces.Remove(surface);
		}

		public sealed class TimelineRecord_SurfaceInteraction : TimelineRecordForBehaviour<SurfaceInteraction>
		{
			public float frictionResistance;
			public Surface[] touchingSurfaces;

			protected override void WriteCurrentState(SurfaceInteraction si)
			{
				base.WriteCurrentState(si);
				frictionResistance = si.frictionResistance;
				touchingSurfaces = si.touchingSurfaces.ToArray();
			}

			protected override void ApplyRecordedState(SurfaceInteraction si)
			{
				base.ApplyRecordedState(si);
				si.frictionResistance = frictionResistance;
				si.touchingSurfaces.Clear();
				si.touchingSurfaces.AddRange(touchingSurfaces);
			}
		}
	}
}
