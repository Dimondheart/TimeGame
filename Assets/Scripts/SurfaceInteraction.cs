﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Surface interactions (like friction) and information
 * relating to touching surfaces.</summary>
 */
public class SurfaceInteraction : MonoBehaviour, ITimelineRecordable
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
				if (s.IsLiquid)
				{
					return true;
				}
			}
			return false;
		}
	}

	TimelineRecord ITimelineRecordable.MakeTimelineRecord()
	{
		TimelineRecord_SurfaceInteraction record = new TimelineRecord_SurfaceInteraction();
		record.frictionResistance = frictionResistance;
		record.touchingSurfaces = touchingSurfaces.ToArray();
		return record;
	}

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_SurfaceInteraction rec = (TimelineRecord_SurfaceInteraction)record;
		frictionResistance = rec.frictionResistance;
		touchingSurfaces.Clear();
		touchingSurfaces.AddRange(rec.touchingSurfaces);
	}

	private void Update()
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		if (ManipulableTime.IsTimeFrozen)
		{
			return;
		}
		if (GetComponent<Health>() == null
			|| GetComponent<Health>().health <= 0
			|| (GetComponent<ControlledMovement>() != null && !GetComponent<ControlledMovement>().IsApplyingMotion)
			)
		{
			return;
		}
		float multiplier = defaultVelocityMultiplier;
		if (touchingSurfaces.Count > 0)
		{
			multiplier = Surface.ResultingVelocityMultiplier(touchingSurfaces);
		}
		multiplier = Mathf.Clamp01(multiplier + frictionResistance * (1.0f - multiplier));
		GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * multiplier;
	}

	private void FixedUpdate()
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		if (ManipulableTime.IsTimeFrozen)
		{
			return;
		}
		if (GetComponent<ControlledMovement>() != null && GetComponent<ControlledMovement>().IsApplyingMotion)
		{
			return;
		}
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

	public class TimelineRecord_SurfaceInteraction : TimelineRecordForComponent
	{
		public float frictionResistance;
		public Surface[] touchingSurfaces;
	}
}
