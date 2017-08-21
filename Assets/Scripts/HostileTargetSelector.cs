using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Selects a target for hostile actions.</summary>*/
[RequireComponent(typeof(Health))]
public class HostileTargetSelector : MonoBehaviour
{
	public GameObject target { get; private set; }
	public List<GameObject> hostilesInLineOfSight = new List<GameObject>();
	public Vector3 targetLastSpotted { get; private set; }

	public void OnLineOfSightEnter(GameObject entered)
	{
		//Debug.Log("Entered line of sight:" + entered.name);
		if (IsHostile(entered))
		{
			hostilesInLineOfSight.Add(entered);
		}
	}

	public void OnLineOfSightExit(GameObject exited)
	{
		//Debug.Log("Left line of sight:" + exited.name);
		hostilesInLineOfSight.Remove(exited);
	}

	public void OnLineOfSightPersist(GameObject seen)
	{
		//Debug.Log("In line of sight:" + seen.name);
		if (!hostilesInLineOfSight.Contains(seen) && IsHostile(seen))
		{
			hostilesInLineOfSight.Add(seen);
		}
	}

	private void Start()
	{
		targetLastSpotted = transform.position;
	}

	private void Update()
	{
		GameObject newTarget = ClosestVisibleHostile();
		if (newTarget == null && target != null)
		{
			targetLastSpotted = target.transform.position;
		}
		target = newTarget;
	}

	/**<summary>Checks if this thing considers the specified other thing
	 * hostile.</summary>
	 */
	public bool IsHostile(GameObject otherThing)
	{
		return otherThing.GetComponent<Health>() != null
			&& otherThing.GetComponent<Health>().isAlignedWithPlayer != GetComponent<Health>().isAlignedWithPlayer;
	}

	public GameObject ClosestVisibleHostile()
	{
		if (hostilesInLineOfSight.Count <= 0)
		{
			return null;
		}
		GameObject closest = null;
		float closestDist = float.PositiveInfinity;
		float dist;
		foreach (GameObject go in hostilesInLineOfSight)
		{
			dist = Vector3.Distance(go.transform.position, transform.position);
			if (dist < closestDist)
			{
				closestDist = dist;
				closest = go;
			}
		}
		return closest;
	}
}
