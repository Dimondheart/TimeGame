using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Information about a ground/etc. surface and detecting
	 * touching game objects that have SurfaceIntaraction.</summary>
	 */
	public class Surface : RecordableMonoBehaviour
	{
		/**<summary>What type of ground/etc. covers this surface.</summary>*/
		public SurfaceType surfaceType;

		/**<summary>True if this surface is a liquid such as water.</summary>*/
		public bool IsLiquid
		{
			get
			{
				return surfaceType == SurfaceType.ShallowWater || surfaceType == SurfaceType.Water;
			}
		}

		/**<summary>Velocity multiplier on this surface due to effects like
		 * friction.</summary>
		 */
		public float velocityMultiplier
		{
			get
			{
				switch (surfaceType)
				{
					case SurfaceType.LowGrass:
						return 0.5f;
					case SurfaceType.Sand:
						return 0.4f;
					case SurfaceType.ShallowWater:
						return 0.35f;
					case SurfaceType.Water:
						return 0.2f;
					case SurfaceType.Floor:
						return 0.95f;
					case SurfaceType.Path:
						return 0.8f;
					default:
						Debug.Log("Unknown SurfaceWithFriction.SurfaceType:" + (int)surfaceType);
						return 0.95f;
				}
			}
		}

		/**<summary>Resulting velocity multiplier when something is equally
		 * touching all of the specified surfaces.</summary>
		 */
		public static float ResultingVelocityMultiplier(List<Surface> surfaces)
		{
			if (surfaces.Count <= 0)
			{
				return 1.0f;
			}
			float sum = 0.0f;
			foreach (Surface s in surfaces)
			{
				sum += s.velocityMultiplier;
			}
			return sum / surfaces.Count;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (ManipulableTime.IsApplyingRecords)
			{
				return;
			}
			if (collision.GetComponent<SurfaceInteraction>() != null)
			{
				collision.GetComponent<SurfaceInteraction>().AddSurface(this);
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (ManipulableTime.IsApplyingRecords)
			{
				return;
			}
			if (collision.GetComponent<SurfaceInteraction>() != null)
			{
				collision.GetComponent<SurfaceInteraction>().RemoveSurface(this);
			}
		}

		public enum SurfaceType
		{
			LowGrass = 0,
			Sand,
			ShallowWater,
			Water,
			Floor,
			Path
		}

		public sealed class TimelineRecord_Surface : TimelineRecordForBehaviour<Surface>
		{
			public Surface.SurfaceType surfaceType;

			protected override void WriteCurrentState(Surface surface)
			{
				base.WriteCurrentState(surface);
				surfaceType = surface.surfaceType;
			}

			protected override void ApplyRecordedState(Surface surface)
			{
				base.ApplyRecordedState(surface);
				surface.surfaceType = surfaceType;
			}
		}
	}
}
