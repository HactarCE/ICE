using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp.Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : PopupPanel
{

	public GameObject TitleText;
	public GameObject BodyText;
	public GameObject Image;

	HelpScreen helpScreen;

	void SetChildrenActive(bool active)
	{
		for (int i = 0; i < transform.childCount; i++)
			transform.GetChild(i).gameObject.SetActive(active);
	}

	public void ShowHelp(HelpScreen newHelpScreen)
	{
		helpScreen = newHelpScreen;
		Show();
	}

	protected override void FinishShow()
	{
		TitleText.GetComponent<Text>().text = helpScreen.Title;
		BodyText.SetActive(helpScreen.Text != null);
		if (helpScreen.Text != null)
			BodyText.GetComponent<Text>().text = helpScreen.Text;
		Image.SetActive(helpScreen.Image != null);
		if (helpScreen.Image != null)
			Image.GetComponent<Image>().sprite = helpScreen.Image;
	}

	protected override bool CheckShouldClose()
	{
		return Input.anyKeyDown;
	}
}
