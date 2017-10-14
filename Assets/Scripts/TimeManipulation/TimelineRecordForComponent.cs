﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Timeline record base for component records.</summary>*/
	public abstract class TimelineRecordForComponent<T> : TimelineRecord<T>
		where T : Component
	{
		/**<summary>Check if the specified component has a timeline maker
		 * implemented by this class.</summary>
		 */
		public static bool HasTimelineMaker(Component component)
		{
			return component is Transform
				|| component is SpriteRenderer
				|| component is Rigidbody2D;
		}

		public static Timeline MakeTimeline(Component component)
		{
			if (component is Transform)
			{
				return new Timeline(typeof(TimelineRecord_Transform), true);
			}
			else if (component is SpriteRenderer)
			{
				return new Timeline(typeof(TimelineRecord_SpriteRenderer), true);
			}
			else if (component is Rigidbody2D)
			{
				return new Timeline(typeof(TimelineRecord_Rigidbody2D), true);
			}
			Debug.LogError(
				"Attempted to use TimelineRecordForComponent to make a" +
				"timeline for a component that does not support it."
				);
			return null;
		}

		public static bool IsComponentWithEnabled(Component component)
		{
			return component is Collider
				|| component is LODGroup
				|| component is Cloth
				|| component is Renderer;
		}

		public static void WriteRecord(Timeline timeline, Component component)
		{
			if (component is Transform)
			{
			}
			else if (component is SpriteRenderer)
			{
				SpriteRenderer sr = (SpriteRenderer)component;
				TimelineRecord_SpriteRenderer rec =
					((Timeline<TimelineRecord_SpriteRenderer>)timeline).GetRecordForCurrentCycle();
				rec.sprite = sr.sprite;
				rec.color = sr.color;
				rec.AddCommonData(sr);
			}
			else if (component is Rigidbody2D)
			{
				Rigidbody2D rb2d = (Rigidbody2D)component;
				TimelineRecord_Rigidbody2D rec =
					((Timeline<TimelineRecord_Rigidbody2D>)timeline).GetRecordForCurrentCycle();
				rec.sharedMaterial = rb2d.sharedMaterial;
				rec.velocity = rb2d.velocity;
				rec.angularVelocity = rb2d.angularVelocity;
				rec.AddCommonData(rb2d);
			}
		}

		public static void ApplyRecord(Timeline timeline, int cycle, Component component)
		{
			if (component is Transform)
			{
			}
			else if (component is SpriteRenderer)
			{
				SpriteRenderer sr = (SpriteRenderer)component;
				TimelineRecord_SpriteRenderer rec =
					((Timeline<TimelineRecord_SpriteRenderer>)timeline).GetRecord(cycle);
				sr.sprite = rec.sprite;
				sr.color = rec.color;
				rec.ApplyCommonData(sr);
			}
			else if (component is Rigidbody2D)
			{
				Rigidbody2D rb2d = (Rigidbody2D)component;
				TimelineRecord_Rigidbody2D rec =
					((Timeline<TimelineRecord_Rigidbody2D>)timeline).GetRecord(cycle);
				rb2d.sharedMaterial = rec.sharedMaterial;
				rb2d.velocity = rec.velocity;
				rb2d.angularVelocity = rec.angularVelocity;
				rec.ApplyCommonData(rb2d);
			}
		}
		public override void ApplyRecordedState<T>(T recordable)
		{
		}
	}
}
