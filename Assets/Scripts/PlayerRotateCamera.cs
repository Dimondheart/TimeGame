using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.DynamicInputSystem;

namespace TechnoWolf.Project1
{
	/**<summary></summary>*/
	public class PlayerRotateCamera : MonoBehaviour
	{
		public float keyMouseSpeed = 1800.0f;
		public float gamepadSpeed = 90.0f;

		private Transform mainCamera;

		private void Awake()
		{
			mainCamera = transform.GetChild(0);
		}

		private void Update()
		{
			if (!mainCamera.GetComponent<CameraViewToggle>().view1Active)
			{
				return;
			}
			if (DynamicInput.GetButtonHeld("Rotate Camera"))
			{
				float rotate = DynamicInput.GetAxis("Horizontal Rotate Camera");
				if (Mathf.Abs(rotate) > 0.01f)
				{
					Vector3 newAngle = transform.localRotation.eulerAngles;
					if (DynamicInput.GamepadModeEnabled)
					{
						newAngle.y = newAngle.y + (rotate * gamepadSpeed * Time.deltaTime);
					}
					else
					{
						newAngle.y = newAngle.y + (rotate * keyMouseSpeed * Time.deltaTime);
					}
					transform.localRotation = Quaternion.Euler(newAngle);
				}
			}

		}
	}
}
