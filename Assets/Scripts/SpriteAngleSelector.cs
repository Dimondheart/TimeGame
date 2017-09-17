using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Selcts which sprite to display based on which direction this thing
 * is currently facing, and other factors. ITimelineRecordable implementation
 * note; the sprite variables are not accounted for in the record, so if in
 * the future the sprites are changed then this will need to be added.</summary>
 */
public class SpriteAngleSelector : MonoBehaviour, ITimelineRecordable
{
	private static readonly Quaternion upRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
	private static readonly Quaternion downRotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
	private static readonly Quaternion rightRotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
	private static readonly Quaternion leftRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
	private static readonly Quaternion upRightRotation = Quaternion.Euler(0.0f, 0.0f, -45.0f);
	private static readonly Quaternion upLeftRotation = Quaternion.Euler(0.0f, 0.0f, 45.0f);
	private static readonly Quaternion downRightRotation = Quaternion.Euler(0.0f, 0.0f, -135.0f);
	private static readonly Quaternion downLeftRotation = Quaternion.Euler(0.0f, 0.0f, 135.0f);
	private static readonly float angleBuffer = 2.0f;
	/**<summary>Angle guarenteed to not be used as a sprite angle, for doing things like
	 * forcing the selector to update.</summary>
	 */
	private static readonly int unusedAngle = 1000;
	/**<summary>Sprite facing the camera.</summary>*/
	public Sprite frontSprite;
	/**<summary>Sprite facing away from the camera.</summary>*/
	public Sprite backSprite;
	/**<summary>Sprite facing to the left.</summary>*/
	public Sprite leftSprite;
	/**<summary>Sprite facing to the right.</summary>*/
	public Sprite rightSprite;
	/**<summary>Dead version of frontSprite.</summary>*/
	public Sprite frontDeadSprite;
	/**<summary>Dead version of backSprite.</summary>*/
	public Sprite backDeadSprite;
	/**<summary>Dead version of leftSprite.</summary>*/
	public Sprite leftDeadSprite;
	/**<summary>Dead version of rightSprite.</summary>*/
	public Sprite rightDeadSprite;
	/**<summary>Front left sprite.</summary>*/
	public Sprite frontLeftSprite;
	/**<summary>Front right sprite.</summary>*/
	public Sprite frontRightSprite;
	/**<summary>Back left sprite.</summary>*/
	public Sprite backLeftSprite;
	/**<summary>Back right sprite.</summary>*/
	public Sprite backRightSprite;
	/**<summary>Dead version of frontLeftSprite.</summary>*/
	public Sprite frontLeftDeadSprite;
	/**<summary>Dead version of frontRightSprite.</summary>*/
	public Sprite frontRightDeadSprite;
	/**<summary>Dead version of backLeftSprite.</summary>*/
	public Sprite backLeftDeadSprite;
	/**<summary>Dead version of backRightSprite.</summary>*/
	public Sprite backRightDeadSprite;
	/**<summary>Transforms that should be rotated to match
	 * the current sprite direction. A rotation of 0 corresponds to when the
	 * back sprite is shown. Do not directly modify this list after script start,
	 * use one of the ___SyncTransformRuntime(...) methods instead.</summary>
	 */
	public List<Transform> syncronizeRotations = new List<Transform>();
	private int currentAngle;
	private bool usingDeadVersion;

	TimelineRecord ITimelineRecordable.MakeTimelineRecord()
	{
		TimelineRecord_SpriteAngleSelector record = new TimelineRecord_SpriteAngleSelector();
		record.syncronizeRotations = syncronizeRotations.ToArray();
		record.currentAngle = currentAngle;
		record.usingDeadVersion = usingDeadVersion;
		return record;
	}

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_SpriteAngleSelector rec = (TimelineRecord_SpriteAngleSelector)record;
		syncronizeRotations.Clear();
		syncronizeRotations.AddRange(rec.syncronizeRotations);
		currentAngle = rec.currentAngle;
		usingDeadVersion = rec.usingDeadVersion;
	}

	private void Update()
	{
		if (ManipulableTime.ApplyingTimelineRecords || ManipulableTime.IsTimeFrozen)
		{
			return;
		}
		float angle = GetComponent<DirectionLooking>().Angle;
		float absAngle = Mathf.Abs(angle);
		// Back
		if (absAngle < 22.5f - angleBuffer)
		{
			SetSelectedRotation(0, !GetComponent<Health>().IsAlive);
		}
		// Front
		else if (absAngle > 157.5f + angleBuffer)
		{
			SetSelectedRotation(180, !GetComponent<Health>().IsAlive);
		}
		// One of the left directions
		else if (angle >= angleBuffer)
		{
			// Front left
			if (absAngle > 112.5f + angleBuffer && absAngle < 157.5f - angleBuffer)
			{
				SetSelectedRotation(135, !GetComponent<Health>().IsAlive);
			}
			// Left
			else if (absAngle > 67.5f + angleBuffer && absAngle < 122.5f - angleBuffer)
			{
				SetSelectedRotation(90, !GetComponent<Health>().IsAlive);
			}
			// Back left
			else if (absAngle > 22.5f + angleBuffer && absAngle < 67.5f - angleBuffer)
			{
				SetSelectedRotation(45, !GetComponent<Health>().IsAlive);
			}
		}
		// One of the right directions
		else if (angle <= -angleBuffer)
		{
			// Front right
			if (absAngle > 112.5f + angleBuffer && absAngle < 157.5f - angleBuffer)
			{
				SetSelectedRotation(-135, !GetComponent<Health>().IsAlive);
			}
			// Right
			else if (absAngle > 67.5f + angleBuffer && absAngle < 122.5f - angleBuffer)
			{
				SetSelectedRotation(-90, !GetComponent<Health>().IsAlive);
			}
			// Back right
			else if (absAngle > 22.5f + angleBuffer && absAngle < 67.5f - angleBuffer)
			{
				SetSelectedRotation(-45, !GetComponent<Health>().IsAlive);
			}
		}
		// Within a buffer arc, make sure to change to the dead version if needed
		else if (!GetComponent<Health>().IsAlive)
		{
			SetSelectedRotation(currentAngle, true);
			return;
		}
	}

	public void AddSyncTransformRuntime(Transform toSync)
	{
		if (!syncronizeRotations.Contains(toSync))
		{
			syncronizeRotations.Add(toSync);
			currentAngle = unusedAngle;
		}
	}

	public void RemoveSyncTransformRuntime(Transform toDesync)
	{
		if (syncronizeRotations.Remove(toDesync))
		{
			currentAngle = unusedAngle;
		}
	}

	private void SetSelectedRotation(int angle, bool useDeadVersion)
	{
		if (angle == currentAngle && useDeadVersion == usingDeadVersion)
		{
			return;
		}
		currentAngle = angle;
		usingDeadVersion = useDeadVersion;
		Quaternion synchRotation;
		Sprite selSprite;
		switch (angle)
		{
			// Back sprite
			case 0:
				synchRotation = upRotation;
				selSprite = useDeadVersion ? backDeadSprite : backSprite;
				break;
			// Back left sprite
			case 45:
				synchRotation = upLeftRotation;
				selSprite = useDeadVersion ? backLeftDeadSprite : backLeftSprite;
				break;
			// Left sprite
			case 90:
				synchRotation = leftRotation;
				selSprite = useDeadVersion ? leftDeadSprite : leftSprite;
				break;
			// Front left sprite
			case 135:
				synchRotation = downLeftRotation;
				selSprite = useDeadVersion ? frontLeftDeadSprite : frontLeftSprite;
				break;
			// Back right sprite
			case -45:
				synchRotation = upRightRotation;
				selSprite = useDeadVersion ? backRightDeadSprite : backRightSprite;
				break;
			// Right sprite
			case -90:
				synchRotation = rightRotation;
				selSprite = useDeadVersion ? rightDeadSprite : rightSprite;
				break;
			// Front right sprite
			case -135:
				synchRotation = downRightRotation;
				selSprite = useDeadVersion ? frontRightDeadSprite : frontRightSprite;
				break;
			// Front sprite
			case 180:
				synchRotation = downRotation;
				selSprite = useDeadVersion ? frontDeadSprite : frontSprite;
				break;
			default:
				Debug.LogWarning("Attempted to select a sprite with an unsupported angle:" + angle);
				return;
		}
		GetComponent<SpriteRenderer>().sprite = selSprite;
		foreach (Transform t in syncronizeRotations)
		{
			t.localRotation = synchRotation;
		}
	}

	public class TimelineRecord_SpriteAngleSelector : TimelineRecordForComponent
	{
		public Transform[] syncronizeRotations;
		public int currentAngle;
		public bool usingDeadVersion;
	}
}
