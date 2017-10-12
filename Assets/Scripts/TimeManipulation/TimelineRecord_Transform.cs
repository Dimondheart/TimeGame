using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	public class TimelineRecord_Transform : TimelineRecordForComponent<Transform>
	{
		public Vector3 localPosition;
		public Quaternion localRotation;
		public Vector3 localScale;
	}
}
