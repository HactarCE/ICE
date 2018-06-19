using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp.Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class HelpButton : GenericButton
{

	public Color IdleColor;
	public Color HoverColor;
	public Color ClickColor;

	public HelpPanel HelpPanel;
	public HelpScreen.ID HelpScreenID;

	protected override void UpdateState()
	{
		Color color;
		if (hover)
			color = clicked ? ClickColor : HoverColor;
		else
			color = IdleColor;
		GetComponent<Image>().color = color;
	}

	protected override void HandleClick()
	{
		HelpPanel.gameObject.SetActive(true);
		HelpPanel.ShowHelp(HelpScreen.HelpScreens[HelpScreenID]);
	}
}
