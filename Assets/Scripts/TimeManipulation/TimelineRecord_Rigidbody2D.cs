﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	public class TimelineRecord_Rigidbody2D : TimelineRecordForComponent<Rigidbody2D>
	{
		public PhysicsMaterial2D sharedMaterial;
		public Vector2 velocity;
		public float angularVelocity;

		protected override void ApplyRecord(Rigidbody2D rigidbody2D)
		{
			base.ApplyRecord(rigidbody2D);
			rigidbody2D.sharedMaterial = sharedMaterial;
			rigidbody2D.velocity = velocity;
			rigidbody2D.angularVelocity = angularVelocity;
		}

		protected override void RecordState(Rigidbody2D rigidbody2D)
		{
			base.RecordState(rigidbody2D);
			sharedMaterial = rigidbody2D.sharedMaterial;
			velocity = rigidbody2D.velocity;
			angularVelocity = rigidbody2D.angularVelocity;
		}
	}
}
