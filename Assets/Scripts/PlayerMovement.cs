using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.DynamicInputSystem;

namespace TechnoWolf.Project1
{
	/**<summary></summary>*/
	public class PlayerMovement : MonoBehaviour
	{
		private Transform mainCameraContainerTransform;
		private Transform mainCamera;

		private void Awake()
		{
			mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
			mainCameraContainerTransform = mainCamera.parent;
		}

		private void Update()
		{
			transform.forward = mainCameraContainerTransform.forward;
			float ix = DynamicInput.GetAxisRaw("Move Horizontal");
			float iy = DynamicInput.GetAxisRaw("Move Vertical");
			Quaternion rotate =
				Quaternion.Euler(0.0f, mainCameraContainerTransform.localRotation.eulerAngles.y, 0.0f);
			Vector3 movement =
				rotate
				* new Vector3(
					DynamicInput.GetAxisRaw("Move Horizontal") * 4.0f,
					0.0f,
					DynamicInput.GetAxisRaw("Move Vertical") * 4.0f
					);
			if (movement.magnitude >= 0.01f)
			{
				GetComponent<CharacterController>().SimpleMove(movement);
			}
			Vector3 look =
				rotate *
				new Vector3(
					DynamicInput.GetAxis("Look Horizontal"),
					0.0f,
					DynamicInput.GetAxis("Look Vertical")
					);
			if (
				mainCamera.GetComponent<CameraViewToggle>().transitioning
				|| mainCamera.GetComponent<CameraViewToggle>().view1Active
				|| look.magnitude < 0.01f
				|| !DynamicInput.GetButtonHeld("Rotate Camera")
			)
			{
				if (movement.magnitude > 0.01f)
				{
					transform.forward = movement;
				}
			}
			else
			{
				transform.forward = look;
			}
		}
	}
}
