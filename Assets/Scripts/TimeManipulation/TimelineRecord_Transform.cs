﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	public class TimelineRecord_Transform : TimelineRecordForComponent<Transform>
	{
		public Transform parent;
		public Vector3 localPosition;
		public Quaternion localRotation;
		public Vector3 localScale;

		protected override void ApplyRecord(Transform transform)
		{
			base.ApplyRecord(transform);
			if (!ReferenceEquals(parent, transform.parent))
			{
				Debug.Log("Shouldn't be here");
				transform.SetParent(parent, false);
			}
			transform.localPosition = localPosition;
			transform.localRotation = localRotation;
			transform.localScale = localScale;
		}

		protected override void RecordState(Transform transform)
		{
			base.RecordState(transform);
			parent = transform.parent;
			localPosition = transform.localPosition;
			localRotation = transform.localRotation;
			localScale = transform.localScale;
		}
	}
}
