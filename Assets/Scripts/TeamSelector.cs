using UnityEngine;

public class TeamSelector : MonoBehaviour
{

	public GameObject TeamButtonPrefab;

	public GameObject ConfirmButton;

	TeamButton[] teamButtons;

	public GameManager.Turn Player;

	// Use this for initialization
	void Start()
	{
		int teamCount = GameConfig.Teams.Length;
		teamButtons = new TeamButton[teamCount];
		for (int i = 0; i < teamCount; i++)
		{
			GameObject teamButton = Instantiate(TeamButtonPrefab, transform, false);
			float min = 0.8f / teamCount * i + 0.1f;
			float max = 0.8f / teamCount * (i + 1) + 0.1f;
			teamButton.GetComponent<RectTransform>().anchorMin = new Vector2(min, 0.5f);
			teamButton.GetComponent<RectTransform>().anchorMax = new Vector2(max, 0.5f);
			teamButtons[i] = teamButton.GetComponent<TeamButton>();
			teamButtons[i].TeamID = i;
		}
		SelectTeamID(Player == GameManager.Turn.P1 ? GameConfig.TeamID_1 : GameConfig.TeamID_2);
	}

	public void SelectTeamID(int TeamID)
	{
		switch (Player)
		{
			case GameManager.Turn.P1:
				GameConfig.TeamID_1 = TeamID;
				break;
			case GameManager.Turn.P2:
				GameConfig.TeamID_2 = TeamID;
				break;
		}
		ConfirmButton.GetComponent<TextButton>().SetDisabled(GameConfig.TeamID_1 == GameConfig.TeamID_2);
		foreach (TeamButton tb in teamButtons)
			tb.SelectID(TeamID);
	}
}
