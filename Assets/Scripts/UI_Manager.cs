using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
	[SerializeField] private TMPro.TMP_Text _scoreText;
	[SerializeField] private Image _livesPlaceHolder;
	[SerializeField] private TMPro.TMP_Text _gameOver;
	[SerializeField] private Sprite[] _livesSprite;
	[SerializeField] private float _flickerDelay = 0.5f;
	private bool _gameOverStatus = false;

	private void Start()
	{
		// Init start values
		UpdateScore(0);
		UpdateLives(3);
		_gameOver.gameObject.SetActive(false);
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
			_gameOver.gameObject.SetActive(true);
			StartCoroutine(GameOverFlicker());
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
