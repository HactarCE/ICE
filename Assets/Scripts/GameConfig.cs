using System;
using AssemblyCSharp.Assets.Scripts;
using UnityEngine;

public class GameConfig : MonoBehaviour
{

	public static Team[] Teams;

	public static Int32 TeamID_A = 0;
	public static Int32 TeamID_B = 1;

	public static int ThrowCount = 16;
	public static int EndCount = 10;

	// Use this for initialization
	void Awake()
	{
		Teams = new Team[]{
			new Team(new Color(0.2f, 0.5f, 1f), "Team1"),
			new Team(Color.yellow, "Team2"),
			new Team(Color.green, "Team3"),
			new Team(Color.red, "Team4")
		};
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SelectTeamA(Int32 teamID)
	{
		TeamID_A = teamID;
	}

	public void SelectTeamB(Int32 teamID)
	{
		TeamID_B = teamID;
	}

}
