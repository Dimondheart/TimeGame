using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Manages high-level game operations.</summary>*/
public class GameController : MonoBehaviour
{
	/**<summary>The player character.</summary>*/
	public GameObject player;
	/**<summary>The panel to be displayed (set active) when
	 * game over conditions are met.</summary>
	 */
	public GameObject gameOverPanel;

	private void Update()
	{
		if (player.GetComponent<Health>().currentHealth <= 0)
		{
			gameOverPanel.SetActive(true);
			player.GetComponent<PlayerMovement>().freezeMovement = true;
		}
	}

	/**<summary>Restart the game.</summary>*/
	public void Restart()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(gameObject.scene.buildIndex);
		gameOverPanel.SetActive(false);
	}
}
