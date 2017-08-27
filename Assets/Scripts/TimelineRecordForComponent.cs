using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Base class for all timeline record classes created for
 * components.</summary>
 */
public abstract class TimelineRecordForComponent : TimelineRecord
{
	public bool enabled { get; private set; }

	/**<summary>Check if the specified component has a record maker
	 * implemented by this class. Returns false for components that
	 * implement ITimelineRecordable.</summary>
	 */
	public static bool HasTimelineRecordMaker(Component component)
	{
		return component is Transform || component is SpriteRenderer;
	}

	public static TimelineRecordForComponent MakeTimelineRecord(Component component)
	{
		if (component is Transform)
		{
			Transform t = (Transform)component;
			TimelineRecord_Transform record = new TimelineRecord_Transform();
			record.localPosition = t.localPosition;
			record.localRotation = t.localRotation;
			record.localScale = t.localScale;
			return record;
		}
		else if (component is SpriteRenderer)
		{
			SpriteRenderer sr = (SpriteRenderer)component;
			TimelineRecord_SpriteRenderer record = new TimelineRecord_SpriteRenderer();
			record.sprite = sr.sprite;
			record.color = sr.color;
			return record;
		}
		Debug.LogWarning(
			"Attempted to make a timeline record for a " +
			"component that doesn't support it:" +
			component.GetType()
			);
		return null;
	}

	public static void ApplyTimelineRecord(Component component, TimelineRecordForComponent record)
	{
		if (component is Transform)
		{
			Transform t = (Transform)component;
			TimelineRecord_Transform rec = (TimelineRecord_Transform)record;
			t.localPosition = rec.localPosition;
			t.localRotation = rec.localRotation;
			t.localScale = rec.localScale;
			return;
		}
		else if (component is SpriteRenderer)
		{
			SpriteRenderer sr = (SpriteRenderer)component;
			TimelineRecord_SpriteRenderer rec = (TimelineRecord_SpriteRenderer)record;
			sr.sprite = rec.sprite;
			sr.color = rec.color;
			return;
		}
		Debug.LogWarning(
			"Attempted to apply a timeline record to a " +
			"component that doesn't support it:" +
			component.GetType()
			);
	}

	public override void AddCommonData(Component component)
	{
		enabled = true;
		System.Reflection.PropertyInfo pInfo = component.GetType().GetProperty("enabled");
		if (pInfo != null)
		{
			enabled = (bool)pInfo.GetValue(component, null);
		}
	}

	public override void ApplyCommonData(Component component)
	{
		System.Reflection.PropertyInfo pInfo = component.GetType().GetProperty("enabled");
		if (pInfo != null)
		{
			pInfo.SetValue(component, enabled, null);
		}
	}

	public class TimelineRecord_Transform : TimelineRecordForComponent
	{
		public Vector3 localPosition;
		public Quaternion localRotation;
		public Vector3 localScale;
	}

	public class TimelineRecord_SpriteRenderer : TimelineRecordForComponent
	{
		public Sprite sprite;
		public Color color;
	}
}
