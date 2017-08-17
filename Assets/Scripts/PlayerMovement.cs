using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : ControlledMovement
{
	public float movementSpeed = 5.0f;
	public bool freezeMovement = false;

	private void Update()
	{
		if (freezeMovement)
		{
			IsApplyingMotion = false;
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		}
		else
		{
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			GetComponent<Rigidbody2D>().velocity =
				new Vector3(DynamicInput.GetAxis("Move Horizontal"), DynamicInput.GetAxis("Move Vertical"), 0.0f).normalized
				* movementSpeed;
			IsApplyingMotion = !Mathf.Approximately(0.0f, GetComponent<Rigidbody2D>().velocity.magnitude);
		}
	}
}
