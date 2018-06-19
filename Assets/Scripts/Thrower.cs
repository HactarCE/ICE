using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : Person
{

	public Vector3 Velocity;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	protected override void LateUpdate()
	{
		GameManager.GameState gameState = MyGameManager.GetComponent<GameManager>().CurrentGameState;
		// I hate C#
		// I know I'm doing this wrong
		// But I still hate C#
		if (gameState == GameManager.GameState.AIMING || gameState == GameManager.GameState.THROWING)
		{
			rockOffset = new Vector3(-0.7f, -0.05f);
			base.LateUpdate();
		}
	}

	void FixedUpdate()
	{
		GameManager.GameState gameState = MyGameManager.GetComponent<GameManager>().CurrentGameState;
		if (gameState != GameManager.GameState.AIMING && gameState != GameManager.GameState.THROWING)
		{
			Velocity.Scale(new Vector3(0.98f, 0.98f));
			transform.position += Velocity / 60;
		}
	}
}
