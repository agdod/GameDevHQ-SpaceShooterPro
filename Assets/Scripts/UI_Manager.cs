using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
	[SerializeField] private TMPro.TMP_Text _scoreText;

	public void UpdateScore(int score)
	{
		_scoreText.text = "Score : " + score;
	}
}
