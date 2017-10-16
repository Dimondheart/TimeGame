using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	public class TimelineRecord_SpriteRenderer : TimelineRecord_ComponentWithEnabled<SpriteRenderer>
	{
		public Sprite sprite;
		public Color color;

		protected override void ApplyRecord(SpriteRenderer spriteRenderer)
		{
			base.ApplyRecord(spriteRenderer);
			spriteRenderer.sprite = sprite;
			spriteRenderer.color = color;
		}

		protected override void RecordState(SpriteRenderer spriteRenderer)
		{
			base.RecordState(spriteRenderer);
			sprite = spriteRenderer.sprite;
			color = spriteRenderer.color;
		}
	}
}
