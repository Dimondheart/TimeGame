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
	public GameObject pauseMenuPanel;
	public GameObject gameOverPanelFirstSelected;
	public GameObject pauseMenuPanelFirstSelected;

	private void Update()
	{
		if (player.GetComponent<Health>().currentHealth <= 0)
		{
			gameOverPanel.SetActive(true);
			player.GetComponent<PlayerMovement>().freezeMovement = true;
		}
		else if (Input.GetButtonDown("TogglePauseMenu"))
		{
			if (pauseMenuPanel.activeSelf)
			{
				ClosePauseMenu();
			}
			else
			{
				OpenPauseMenu();
			}
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			OpenPauseMenu();
		}
	}

	/**<summary>Restart the game.</summary>*/
	public void RestartGame()
	{
		gameOverPanel.SetActive(false);
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(gameObject.scene.buildIndex);
	}

	public void QuitToMainMenu()
	{
		pauseMenuPanel.SetActive(false);
		gameOverPanel.SetActive(false);
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
	}

	public void OpenPauseMenu()
	{
		if (pauseMenuPanel.activeSelf || gameOverPanel.activeSelf)
		{
			return;
		}
		Time.timeScale = 0.0f;
		pauseMenuPanel.SetActive(true);
		UnityEngine.EventSystems.EventSystem.current.firstSelectedGameObject = pauseMenuPanelFirstSelected;
	}

	public void ClosePauseMenu()
	{
		if (!pauseMenuPanel.activeSelf)
		{
			return;
		}
		Time.timeScale = 1.0f;
		pauseMenuPanel.SetActive(false);
		UnityEngine.EventSystems.EventSystem.current.firstSelectedGameObject = null;
	}
}
