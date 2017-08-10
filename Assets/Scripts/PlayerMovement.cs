using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float movementSpeed = 5.0f;
	public bool freezeMovement = false;

	private void Update()
	{
		if (freezeMovement)
		{
			return;
		}
		GetComponent<Rigidbody2D>().velocity =
			new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f).normalized
			* movementSpeed;
	}
}
