using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>The attached trigger represents a region that has
 * surface friction.</summary>
 */
public class Surface : MonoBehaviour
{
	public SurfaceType surfaceType;

	public bool IsLiquid
	{
		get
		{
			return surfaceType == SurfaceType.ShallowWater || surfaceType == SurfaceType.Water;
		}
	}

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
					return 1.0f;
				case SurfaceType.Path:
					return 0.8f;
				default:
					Debug.Log("Unknown SurfaceWithFriction.SurfaceType:" + (int)surfaceType);
					return 1.0f;
			}
		}
	}

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
		if (collision.GetComponent<SurfaceInteraction>() != null)
		{
			collision.GetComponent<SurfaceInteraction>().AddSurface(this);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{

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
}
