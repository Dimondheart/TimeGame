using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Records all timeline data for the game object, and
 * also handles playback of recorded data.</summary>
 */
public class TimelineRecorder : MonoBehaviour
{
	public Timeline timeline { get; private set; }

	private void Awake()
	{
		timeline = new Timeline();
	}

	private void Update()
	{
		
	}
}
