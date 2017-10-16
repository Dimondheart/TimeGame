using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.Project1
{
	/**<summary></summary>*/
	public class RigidFollowTransform : MonoBehaviour
	{
		public Transform follow;

		private void LateUpdate()
		{
			UpdatePosition();
		}

		private void UpdatePosition()
		{
			if (follow == null)
			{
				return;
			}
			transform.position = follow.position;
		}
	}
}
