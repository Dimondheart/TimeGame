using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Indicates to the game controller that the current level
 * is finished loading.</summary>
 */
public class LevelLoadFinisher : MonoBehaviour
{
	private void Awake()
	{
		GetComponent<GameController>().isLevelLoaded = true;
	}
}
