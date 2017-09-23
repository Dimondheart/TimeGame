using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Base class for all timeline record classes created for
 * components.</summary>
 */
	public abstract class TimelineRecordForComponent : TimelineRecord<Component>
	{
		public bool enabled { get; private set; }
		public Component component { get; private set; }

		/**<summary>Check if the specified component has a record maker
		 * implemented by this class. Returns false for components that
		 * implement ITimelineRecordable.</summary>
		 */
		public static bool HasTimelineRecordMaker(Component component)
		{
			return component is Transform || component is SpriteRenderer || component is Rigidbody2D || component is Behaviour;
		}

		public static TimelineRecordForComponent MakeTimelineRecord(Component component)
		{
			TimelineRecordForComponent record = null;
			if (component is Transform)
			{
				record = new TimelineRecord_Transform();
			}
			else if (component is SpriteRenderer)
			{
				record = new TimelineRecord_SpriteRenderer();
			}
			else if (component is Rigidbody2D)
			{
				record = new TimelineRecord_Rigidbody2D();
			}
			else if (component is Behaviour)
			{
				record = new TimelineRecord_Behaviour();
			}
			return record;
		}

		public static void RecordCurrentState(Component component, TimelineRecordForComponent record)
		{
			if (component is Transform)
			{
				Transform t = (Transform)component;
				TimelineRecord_Transform rec = (TimelineRecord_Transform)record;
				rec.localPosition = t.localPosition;
				rec.localRotation = t.localRotation;
				rec.localScale = t.localScale;
			}
			else if (component is SpriteRenderer)
			{
				SpriteRenderer sr = (SpriteRenderer)component;
				TimelineRecord_SpriteRenderer rec = (TimelineRecord_SpriteRenderer)record;
				rec.sprite = sr.sprite;
				rec.color = sr.color;
			}
			else if (component is Rigidbody2D)
			{
				Rigidbody2D rb2d = (Rigidbody2D)component;
				TimelineRecord_Rigidbody2D rec = (TimelineRecord_Rigidbody2D)record;
				rec.sharedMaterial = rb2d.sharedMaterial;
				rec.velocity = rb2d.velocity;
				rec.angularVelocity = rb2d.angularVelocity;
			}
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
			else if (component is Rigidbody2D)
			{
				Rigidbody2D rb2d = (Rigidbody2D)component;
				TimelineRecord_Rigidbody2D rec = (TimelineRecord_Rigidbody2D)record;
				rb2d.sharedMaterial = rec.sharedMaterial;
				rb2d.velocity = rec.velocity;
				rb2d.angularVelocity = rec.angularVelocity;
				return;
			}
			else if (component is Behaviour)
			{
				// Generic Behaviour records only have common data
				return;
			}
			Debug.LogWarning(
				"Attempted to apply a timeline record to a " +
				"component that doesn't support it:" +
				component.GetType()
				);
		}

		public void SetupRecord(Component component)
		{
			this.component = component;
		}

		public void RecordState()
		{
			if (component is ITimelineRecordable)
			{
				((ITimelineRecordable)component).RecordCurrentState(this);
			}
			else if (HasTimelineRecordMaker(component))
			{
				RecordCurrentState(component, this);
			}
			AddCommonData(component);
		}

		public void ApplyRecord()
		{
			if (component is ITimelineRecordable)
			{
				((ITimelineRecordable)component).ApplyTimelineRecord(this);
			}
			else if (HasTimelineRecordMaker(component))
			{
				ApplyTimelineRecord(component, this);
			}
			ApplyCommonData(component);
		}

		public override void AddCommonData(Component component)
		{
			if (component is Collider)
			{
				enabled = ((Collider)component).enabled;
			}
			else if (component is LODGroup)
			{
				enabled = ((LODGroup)component).enabled;
			}
			else if (component is Cloth)
			{
				enabled = ((Cloth)component).enabled;
			}
			else if (component is Renderer)
			{
				enabled = ((Renderer)component).enabled;
			}
			else if (component is Behaviour)
			{
				enabled = ((Behaviour)component).enabled;
			}
			else
			{
				enabled = true;
			}
		}

		public override void ApplyCommonData(Component component)
		{
			if (component is Collider)
			{
				((Collider)component).enabled = enabled;
			}
			else if (component is LODGroup)
			{
				((LODGroup)component).enabled = enabled;
			}
			else if (component is Cloth)
			{
				((Cloth)component).enabled = enabled;
			}
			else if (component is Renderer)
			{
				((Renderer)component).enabled = enabled;
			}
			else if (component is Behaviour)
			{
				((Behaviour)component).enabled = enabled;
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

		public class TimelineRecord_Rigidbody2D : TimelineRecordForComponent
		{
			public PhysicsMaterial2D sharedMaterial;
			public Vector2 velocity;
			public float angularVelocity;
		}

		public class TimelineRecord_Behaviour : TimelineRecordForComponent
		{
		}
	}
}
