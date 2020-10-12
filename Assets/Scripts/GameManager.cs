using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] private bool _isGameOver;

	void Start()
	{
		_isGameOver = false;
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
		{
			// Reload scene to restart
			SceneManager.LoadScene("Game");
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			SceneManager.LoadScene("Main_Menu");
		}
	}

	public void SetGameOver()
	{
		_isGameOver = true;
	}
}
