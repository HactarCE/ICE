using System;
using AssemblyCSharp.Assets.Scripts;
using UnityEngine;

public class GameConfig : MonoBehaviour
{

	public static Team[] Teams;

	public static int TeamID_1 = 0;
	public static int TeamID_2 = 1;

	public static int ThrowCount = 16;
	public static int EndCount = 10;

	public static bool P1ShowTutorial;
	public static bool P2ShowTutorial;

	// Use this for initialization
	void Awake()
	{
		Teams = new Team[]{
			new Team(Color.green, "Team1"),
			new Team(Color.yellow, "Team2"),
			new Team(new Color(0.2f, 0.5f, 1f), "Team3"),
			new Team(Color.red, "Team4")
		};
	}

	public void SetThrowCount(int throwCount)
	{
		ThrowCount = throwCount;
	}

	public void SetEndCount(int endCount)
	{
		EndCount = endCount;
	}

	public void SetP1ShowTutorial(bool showTutorial)
	{
		P1ShowTutorial = showTutorial;
	}

	public void SetP2ShowTutorial(bool showTutorial)
	{
		P2ShowTutorial = showTutorial;
	}
}
