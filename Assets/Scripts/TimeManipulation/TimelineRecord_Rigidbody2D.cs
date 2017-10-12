using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	public class TimelineRecord_Rigidbody2D : TimelineRecordForComponent<Rigidbody2D>
	{
		public PhysicsMaterial2D sharedMaterial;
		public Vector2 velocity;
		public float angularVelocity;
	}
}
