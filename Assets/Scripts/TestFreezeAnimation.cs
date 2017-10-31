using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary></summary>*/
	public class TestFreezeAnimation : PausableMonoBehaviour
	{
		public Animation anim;

		protected override void FirstPausedUpdate()
		{
			//anim.enabled = false;
			base.FirstPausedUpdate();
		}

		protected override void FirstResumedUpdate()
		{
			//anim.enabled = true;
			base.FirstResumedUpdate();
		}

		protected override void PausedUpdate()
		{
			base.FlowingUpdate();
		}

		protected override void FlowingUpdate()
		{

			base.FlowingUpdate();
		}
	}
}
