using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPopup : PopupPanel
{

	public Text Title;
	public Text P1ScoreText;
	public Text P2ScoreText;

	// Use this for initialization
	protected override void Awake()
	{
		speed = 1f / 8f;
		base.Awake();
	}


	public void ShowEndOver()
	{
		Title.text = string.Format("End #{0} is over!", GameManager.End);
		P1ScoreText.text = string.Format("Player 1\n{0}", GameManager.P1ScoreThisEnd);
		P1ScoreText.color = GameConfig.Teams[GameConfig.TeamID_1].Color;
		P2ScoreText.text = string.Format("Player 2\n{0}", GameManager.P2ScoreThisEnd);
		P2ScoreText.color = GameConfig.Teams[GameConfig.TeamID_2].Color;
		Show();
	}

	protected override void FinishShow()
	{
	}

	protected override bool CheckShouldClose()
	{
		return Input.anyKeyDown;
	}
}
