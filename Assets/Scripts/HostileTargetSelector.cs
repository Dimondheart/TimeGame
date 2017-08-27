using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Selects a target for hostile actions.</summary>*/
[RequireComponent(typeof(Health))]
public class HostileTargetSelector : MonoBehaviour, ITimelineRecordable
{
	public GameObject target { get; private set; }
	public List<GameObject> hostilesInLineOfSight = new List<GameObject>();
	public List<GameObject> otherHostilesDetected = new List<GameObject>();
	public Vector3 targetLastSpotted { get; private set; }

	TimelineRecord ITimelineRecordable.MakeTimelineRecord()
	{
		TimelineRecord_HostileTargetSelector record = new TimelineRecord_HostileTargetSelector();
		record.target = target;
		record.hostilesInLineOfSight = hostilesInLineOfSight.ToArray();
		record.otherHostilesDetected = otherHostilesDetected.ToArray();
		record.targetLastSpotted = targetLastSpotted;
		return record;
	}

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_HostileTargetSelector rec = (TimelineRecord_HostileTargetSelector)record;
		target = rec.target;
		hostilesInLineOfSight.Clear();
		hostilesInLineOfSight.AddRange(rec.hostilesInLineOfSight);
		otherHostilesDetected.Clear();
		otherHostilesDetected.AddRange(rec.otherHostilesDetected);
		targetLastSpotted = rec.targetLastSpotted;
	}

	public void OnLineOfSightEnter(GameObject entered)
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		if (!hostilesInLineOfSight.Contains(entered) && IsHostile(entered))
		{
			hostilesInLineOfSight.Add(entered);
		}
	}

	public void OnLineOfSightExit(GameObject exited)
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		hostilesInLineOfSight.Remove(exited);
	}

	public void OnLineOfSightPersist(GameObject seen)
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		//Debug.Log("In line of sight:" + seen.name);
		if (!hostilesInLineOfSight.Contains(seen) && IsHostile(seen))
		{
			hostilesInLineOfSight.Add(seen);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		if (!otherHostilesDetected.Contains(collision.gameObject) && IsHostile(collision.gameObject))
		{
			otherHostilesDetected.Add(collision.gameObject);
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		otherHostilesDetected.Remove(collision.gameObject);
	}

	private void Start()
	{
		targetLastSpotted = transform.position;
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
		if (!hostilesInLineOfSight.Contains(target))
		{
			target = null;
		}
		if (target != null)
		{
			targetLastSpotted = target.transform.position;
		}
		target = ClosestDetectedHostile();
	}

	/**<summary>Checks if this thing considers the specified other thing
	 * hostile.</summary>
	 */
	public bool IsHostile(GameObject otherThing)
	{
		return otherThing.GetComponent<Health>() != null
			&& otherThing.GetComponent<Health>().isAlignedWithPlayer != GetComponent<Health>().isAlignedWithPlayer;
	}

	public GameObject ClosestDetectedHostile()
	{
		if (hostilesInLineOfSight.Count <= 0 && otherHostilesDetected.Count <= 0)
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
		foreach (GameObject go in otherHostilesDetected)
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

	public class TimelineRecord_HostileTargetSelector : TimelineRecordForComponent
	{
		public GameObject target;
		public GameObject[] hostilesInLineOfSight;
		public GameObject[] otherHostilesDetected;
		public Vector3 targetLastSpotted;
	}
}
