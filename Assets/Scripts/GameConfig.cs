using System;
using AssemblyCSharp.Assets.Scripts;
using UnityEngine;

public class GameConfig : MonoBehaviour
{

    public static Team[] Teams;

    public Int32 TeamID_A;
    public Int32 TeamID_B;

    // Use this for initialization
    void Start()
    {
        Teams = new Team[]{
            new Team(Color.yellow, "Team2")
        };
        Debug.Log(Teams[0].AimSprite);
    }

    // Update is called once per frame
    void Update()
    {

    }

	public void SelectTeamA(Int32 teamID) {
		TeamID_A = teamID;
	}

	public void SelectTeamB(Int32 teamID) {
		TeamID_B = teamID;
	}

}
