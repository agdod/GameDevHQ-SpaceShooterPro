using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
	[SerializeField] private TMPro.TMP_Text _scoreText;
	[SerializeField] private Image _livesPlaceHolder;
	[SerializeField] private Sprite[] _livesSprite;

	private void Start()
	{
		// Init start values
		UpdateScore(0);
		UpdateLives(3);
	}

	public void UpdateScore(int score)
	{
		_scoreText.text = "Score : " + score;
	}

	public void UpdateLives(int currentLives)
	{
		_livesPlaceHolder.sprite = _livesSprite[currentLives];
	}
}
