using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
	[SerializeField] private TMPro.TMP_Text _scoreText;
	[SerializeField] private TMPro.TMP_Text _ammoCount;
	[SerializeField] private Image _livesPlaceHolder;
	[SerializeField] private TMPro.TMP_Text _gameOver;
	[SerializeField] private TMPro.TMP_Text _restartText;
	[SerializeField] private Sprite[] _livesSprite;
	[SerializeField] private float _flickerDelay = 0.5f;


	// Managers
	[SerializeField] private GameManager _gameManager;

	private bool _outOfAmmoStatus = false;
	private bool _gameOverStatus = false;

	private void Start()
	{
		// Init start values
		_gameOver.gameObject.SetActive(false);
		_restartText.gameObject.SetActive(false);
	}

	private void GameOver()
	{
		_gameManager.SetGameOver();
		_gameOver.gameObject.SetActive(true);
		_restartText.gameObject.SetActive(true);
		StartCoroutine(GameOverFlicker());
	}

	public void UpdateAmmo(int score)
	{
		_ammoCount.text = score.ToString(); ;
	}

	public void OutOfAmmo()
	{
		StartCoroutine(Ammoflicker());
	}

	IEnumerator Ammoflicker()
	{
		while (_ammoCount.text == "0")
		{
			_outOfAmmoStatus = !_outOfAmmoStatus;
			_ammoCount.gameObject.SetActive(_outOfAmmoStatus);
			yield return new WaitForSeconds(_flickerDelay);
		}
	}

	public void UpdateScore(int score)
	{
		_scoreText.text = "Score : " + score;
	}

	public void UpdateLives(int currentLives)
	{
		_livesPlaceHolder.sprite = _livesSprite[currentLives];
		if (currentLives == 0)
		{
			GameOver();
		}
	}

	IEnumerator GameOverFlicker()
	{
		while (true)
		{
			_gameOverStatus = !_gameOverStatus;
			_gameOver.gameObject.SetActive(_gameOverStatus);
			yield return new WaitForSeconds(_flickerDelay);
		}
	}
}
