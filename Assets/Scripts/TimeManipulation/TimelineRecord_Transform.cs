using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	public class TimelineRecord_Transform : TimelineRecordForComponent, IWriteApplyTimelineRecord<Transform>
	{
		public Vector3 localPosition;
		public Quaternion localRotation;
		public Vector3 localScale;

		public void ApplyRecordedState(Transform transform)
		{
			base.ApplyRecordedState(transform);
			transform.localPosition = localPosition;
			transform.localRotation = localRotation;
			transform.localScale = localScale;
		}

		public void WriteCurrentState(Transform transform)
		{
			base.WriteCurrentState(transform);
			localPosition = transform.localPosition;
			localRotation = transform.localRotation;
			localScale = transform.localScale;
		}
	}
}
