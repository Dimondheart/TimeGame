﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
	public GameObject mainMenuPanel;

	private void Awake()
	{
		Time.timeScale = 1.0f;
	}

	public void StartGame()
	{
		mainMenuPanel.SetActive(false);
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
	}

	public void QuitToDesktop()
	{
		mainMenuPanel.SetActive(false);
		Application.Quit();
	}
}
