using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : MonoBehaviour
{

	public GameObject GameManagerObject;
	public GameObject Player1Score;
	public GameObject Player2Score;
	public GameObject ThrowCounter;
	public GameObject EndCounter;
	public GameObject StatusIndicator;

	GameManager gameManager;
	Text p1ScoreText;
	Text p2ScoreText;
	Text throwCountText;
	Text endCountText;
	Text statusText;

	// Use this for initialization
	void Start()
	{
		gameManager = GameManagerObject.GetComponent<GameManager>();
		p1ScoreText = Player1Score.GetComponent<Text>();
		p2ScoreText = Player2Score.GetComponent<Text>();
		throwCountText = ThrowCounter.GetComponent<Text>();
		endCountText = EndCounter.GetComponent<Text>();
		statusText = StatusIndicator.GetComponent<Text>();
		p1ScoreText.color = GameConfig.Teams[GameConfig.TeamID_1].Color;
		p2ScoreText.color = GameConfig.Teams[GameConfig.TeamID_2].Color;
	}

	// Update is called once per frame
	void Update()
	{
		p1ScoreText.text = string.Format("P1 Score: {0} ({1})", GameManager.P1Score + gameManager.P1ScoreThisEnd, gameManager.P1ScoreThisEnd);
		p2ScoreText.text = string.Format("P2 Score: {0} ({1})", GameManager.P2Score + gameManager.P2ScoreThisEnd, gameManager.P2ScoreThisEnd);
		throwCountText.text = string.Format("Throw: {0}/{1}", gameManager.Throw, GameConfig.ThrowCount);
		endCountText.text = string.Format("End: {0}/{1}", gameManager.End, GameConfig.EndCount);
		string playerName = null;
		string stage = null;
		switch (gameManager.CurrentTurn)
		{
			case GameManager.Turn.P1:
				playerName = "Player 1";
				break;
			case GameManager.Turn.P2:
				playerName = "Player 2";
				break;
		}
		switch (gameManager.CurrentGameState)
		{
			case GameManager.GameState.AIMING:
				stage = "Aim";
				break;
			case GameManager.GameState.THROWING:
				stage = "Throw";
				break;
			case GameManager.GameState.SWEEPING:
				stage = "Sweep";
				break;
		}
		if (playerName == null || stage == null)
			statusText.text = "";
		else
			statusText.text = playerName + " " + stage;
		statusText.color = gameManager.GetCurrentTeam().Color;
	}
}
