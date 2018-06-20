using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{

	public Text TitleText;
	public Text FinalScoreText;
	public Text Player1ScoreText;
	public Text Player2ScoreText;

	// Use this for initialization
	void Start()
	{
		int score1 = GameManager.P1Score, score2 = GameManager.P2Score;
		if (score1 == score2)
			TitleText.text = "It's a tie!";
		else if (score1 > score2)
			TitleText.text = "Player 1 wins!";
		else
			TitleText.text = "Player 2 wins!";
		Player1ScoreText.text += score1.ToString();
		Player1ScoreText.color = GameConfig.Teams[GameConfig.TeamID_1].Color;
		Player2ScoreText.text += score2.ToString();
		Player2ScoreText.color = GameConfig.Teams[GameConfig.TeamID_2].Color;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.anyKeyDown)
			SceneManager.LoadScene("MainMenu");
	}
}
