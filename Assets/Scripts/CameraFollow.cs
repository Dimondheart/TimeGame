using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Script for making a camera hold a set position over
 * a specified game object.</summary>
 */
public class CameraFollow : MonoBehaviour
{
	/**<summary>The Transform to follow.</summary>*/
	public Transform follow;

	/**<summary>The offset of the camera from the followed object.</summary>*/
	private Vector3 offset = new Vector3(0.0f, 0.0f, -10.0f);

	private void Start()
	{
		if (follow == null)
		{
			return;
		}
		offset = transform.position - follow.position;
	}

	private void LateUpdate()
	{
		if (follow == null)
		{
			return;
		}
		transform.position = follow.position + offset;
	}
}
