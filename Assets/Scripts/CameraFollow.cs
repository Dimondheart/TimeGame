using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public GameObject follow;

	private Vector3 offset;

	private void Start()
	{
		if (follow != null)
		{
			offset = transform.position - follow.transform.position;
		}
	}

	private void OnPreCull()
	{
		if (follow == null)
		{
			return;
		}
		transform.position = follow.transform.position + offset;
	}
}
