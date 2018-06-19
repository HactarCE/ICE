using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamButton : GenericButton
{

	public Color IdleColor;
	public Color HoverColor;
	public Color ClickColor;
	public Color SelectedColor;

	public GameObject Pants;
	public GameObject Handle;
	public int TeamID;
	bool selected;

	// Use this for initialization
	void Start()
	{
		Pants.GetComponent<Image>().sprite = GameConfig.Teams[TeamID].PantsSprite;
		Handle.GetComponent<Image>().color = GameConfig.Teams[TeamID].Color;
		UpdateState();
	}

	public void SelectID(int teamID)
	{
		selected = teamID == TeamID;
		UpdateState();
	}

	protected override void UpdateState()
	{
		Color color;
		if (selected)
			color = SelectedColor;
		else if (hover)
			color = clicked ? ClickColor : HoverColor;
		else
			color = IdleColor;
		GetComponent<Image>().color = color;
	}

	protected override void HandleClick()
	{
		GetComponentInParent<TeamSelector>().SelectTeamID(TeamID);
	}
}
