using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.DynamicInputSystem;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary></summary>*/
	public class CameraViewToggle : MonoBehaviour
	{
		public Vector3 view1Position;
		public Vector3 view1Rotation;
		public Vector3 view2Position;
		public Vector3 view2Rotation;

		public float transitionTime = 1.0f;
		public bool view1Active = true;

		public bool transitioning { get; private set; }

		private void Start()
		{
			transform.localPosition = view1Position;
			transform.localRotation = Quaternion.Euler(view1Rotation);
		}

		public void Update()
		{
			if (DynamicInput.GetButtonDown("Toggle Camera View"))
			{
				view1Active = !view1Active;
				transitioning = true;
			}
			if (transitioning)
			{
				if (view1Active)
				{
					if (Vector3.Distance(transform.localPosition, view1Position) <= 0.01f)
					{
						transform.localPosition = view1Position;
						transform.localRotation = Quaternion.Euler(view1Rotation);
						transitioning = false;
					}
					else
					{
						transform.localPosition =
							Vector3.MoveTowards(
								transform.localPosition,
								view1Position,
								Time.deltaTime / transitionTime * (view2Position - view1Position).magnitude
								);
						transform.localRotation =
							Quaternion.RotateTowards(
								transform.localRotation,
								Quaternion.Euler(view1Rotation),
								Time.deltaTime / transitionTime * (view2Rotation - view1Rotation).magnitude
								);
					}
				}
				else
				{
					if (Vector3.Distance(transform.localPosition, view2Position) <= 0.01f)
					{
						transform.localPosition = view2Position;
						transform.localRotation = Quaternion.Euler(view2Rotation);
						transitioning = false;
					}
					else
					{
						transform.localPosition =
							Vector3.MoveTowards(
								transform.localPosition,
								view2Position,
								Time.deltaTime / transitionTime * (view1Position - view2Position).magnitude
								);
						transform.localRotation =
							Quaternion.RotateTowards(
								transform.localRotation,
								Quaternion.Euler(view2Rotation),
								Time.deltaTime / transitionTime * (view1Rotation - view2Rotation).magnitude
								);
					}
				}
			}
		}
	}
}
