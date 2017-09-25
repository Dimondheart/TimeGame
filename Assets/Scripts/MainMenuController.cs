using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>High level tasks and functionality for the main menu.</summary>*/
	public class MainMenuController : MonoBehaviour
	{
		public GameObject mainMenuPanel;
		public GameObject creditsPanel;
		public GameObject activateOnQuit;
		public GameObject activateWhileLoading;

		private void Awake()
		{
			ManipulableTime.IsGamePaused = false;
			ManipulableTime.IsTimePaused = false;
		}

		public void StartGame()
		{
			mainMenuPanel.SetActive(false);
			activateWhileLoading.SetActive(true);
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
		}

		public void QuitToDesktop()
		{
			mainMenuPanel.SetActive(false);
			creditsPanel.SetActive(false);
			activateOnQuit.SetActive(true);
			Application.Quit();
		}

		public void OpenCreditsMenu()
		{
			if (creditsPanel.activeSelf)
			{
				return;
			}
			mainMenuPanel.SetActive(false);
			creditsPanel.SetActive(true);
		}

		public void ReturnToMainMenuPanel()
		{
			if (mainMenuPanel.activeSelf)
			{
				return;
			}
			mainMenuPanel.SetActive(true);
			creditsPanel.SetActive(false);
		}
	}
}
