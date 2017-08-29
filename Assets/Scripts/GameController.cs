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
	/**<summary>The panel to be displayed (set active) when
	 * victory conditions are met.</summary>
	 */
	public GameObject victoryPanel;
	/**<summary>The panel to be displayed when the pause menu is
	 * open.</summary>
	 */
	public GameObject pauseMenuPanel;
	/**<summary>Set active when displaying any menus that pause
	 * the game.</summary>
	 */
	public GameObject menuBackgroundPanel;
	/**<summary>The GUI element to select first in the game over panel (used for
	 * gamepad menu navigation.)</summary>
	 */
	public GameObject gameOverPanelFirstSelected;
	/**<summary>The GUI element to select first in the pause menu panel (used for
	 * gamepad menu navigation.)</summary>
	 */
	public GameObject pauseMenuPanelFirstSelected;

	private void Update()
	{
		// Temporary gamepad mode toggle
		if (Input.GetKeyDown(KeyCode.G))
		{
			DynamicInput.GamepadModeEnabled = !DynamicInput.GamepadModeEnabled;
		}
		if (GetComponent<CharacterTracker>().LivePlayerCount <= 0)
		{
			gameOverPanel.SetActive(true);
		}
		else if (GetComponent<CharacterTracker>().LiveEnemyCount <= 0)
		{
			victoryPanel.SetActive(true);
		}
		else if (DynamicInput.GetButtonDown("Toggle Pause Menu"))
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

	/**<summary>Restart the current level.</summary>*/
	public void RestartGame()
	{
		ClosePauseMenu();
		gameOverPanel.SetActive(false);
		victoryPanel.SetActive(false);
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(gameObject.scene.buildIndex);
	}

	/**<summary>Quit to the main menu.</summary>*/
	public void QuitToMainMenu()
	{
		ClosePauseMenu();
		gameOverPanel.SetActive(false);
		victoryPanel.SetActive(false);
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
	}

	/**<summary>Open the pause menu.</summary>*/
	public void OpenPauseMenu()
	{
		if (menuBackgroundPanel.activeSelf)
		{
			return;
		}
		ManipulableTime.IsGameFrozen = true;
		pauseMenuPanel.SetActive(true);
		menuBackgroundPanel.SetActive(true);
		UnityEngine.EventSystems.EventSystem.current.firstSelectedGameObject = pauseMenuPanelFirstSelected;
	}

	/**<summary>Close the pause menu.</summary>*/
	public void ClosePauseMenu()
	{
		if (!pauseMenuPanel.activeSelf)
		{
			return;
		}
		ManipulableTime.IsGameFrozen = false;
		pauseMenuPanel.SetActive(false);
		menuBackgroundPanel.SetActive(false);
		UnityEngine.EventSystems.EventSystem.current.firstSelectedGameObject = null;
	}
}
