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

		protected override void ApplyRecordedState(SpriteRenderer spriteRenderer)
		{
			base.ApplyRecordedState(spriteRenderer);
			spriteRenderer.sprite = sprite;
			spriteRenderer.color = color;
		}

		protected override void WriteCurrentState(SpriteRenderer spriteRenderer)
		{
			base.WriteCurrentState(spriteRenderer);
			sprite = spriteRenderer.sprite;
			color = spriteRenderer.color;
		}
	}
}
