using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Applies surface friction to a game object.</summary>*/
public class SurfaceFriction : MonoBehaviour
{
	/**<summary>Velocity multiplier when not touching a specific surface
	 * (meaning outside the map/play area.)</summary>
	 */
	public static float defaultVelocityMultiplier = 0.08f;

	/**<summary>Resistance to friction effects. 0 is no resistance, 1
	 * means friction has no effect.</summary>
	 */
	public float frictionResistance = 0.0f;

	private List<SurfaceWithFriction> touchingSurfaces = new List<SurfaceWithFriction>();

	public bool IsSwimming
	{
		get
		{
			if (touchingSurfaces.Count <= 0)
			{
				return true;
			}
			foreach (SurfaceWithFriction s in touchingSurfaces)
			{
				if (s.IsLiquid)
				{
					return true;
				}
			}
			return false;
		}
	}

	private void Update()
	{
		float multiplier = defaultVelocityMultiplier;
		if (touchingSurfaces.Count > 0)
		{
			multiplier = SurfaceWithFriction.ResultingVelocityMultiplier(touchingSurfaces);
		}
		multiplier = Mathf.Clamp01(multiplier + frictionResistance * (1.0f - multiplier));
		GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * multiplier;
		Debug.Log(GetComponent<Rigidbody2D>().velocity);
	}

	public void AddSurface(SurfaceWithFriction surface)
	{
		if (!touchingSurfaces.Contains(surface))
		{
			touchingSurfaces.Add(surface);
		}
	}

	public void RemoveSurface(SurfaceWithFriction surface)
	{
		touchingSurfaces.Remove(surface);
	}
}
