using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.Project1
{
	/**<summary>Holds info about damage-related hits.</summary>*/
	public class HitInfo
	{
		public float temperatureAlignment;
		public float moistureAlignment;
		public Collider2D hitBy;
		public Collider2D hitCollider;
		public float damage;
		public float permanentDamage;
	}
}
