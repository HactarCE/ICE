using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp.Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour
{

	public GameObject GameManager;

	public GameObject TitleText;
	public GameObject BodyText;
	public GameObject Image;

	bool show;
	bool showing;
	HelpScreen helpScreen;

	float speed = 1f / 16f;

	// Use this for initialization
	void Awake()
	{
		gameObject.SetActive(false);
	}

	void SetChildrenActive(bool active)
	{
		for (int i = 0; i < transform.childCount; i++)
			transform.GetChild(i).gameObject.SetActive(active);
	}

	public void ShowHelp(HelpScreen newHelpScreen)
	{
		if (GameManager != null)
			GameManager.GetComponent<GameManager>().Pause();
		helpScreen = newHelpScreen;
		gameObject.SetActive(true);
		SetChildrenActive(false);
		show = true;
		showing = false;
		transform.localScale = new Vector2(speed, speed);
	}

	// Update is called once per frame
	void Update()
	{
		if (show)
		{
			if (transform.localScale.x < 0.99f)
				transform.localScale += new Vector3(speed, speed);
			else if (showing)
			{
				if (Input.anyKeyDown)
					show = false;
			}
			else
			{
				SetChildrenActive(true);
				showing = true;
				TitleText.GetComponent<Text>().text = helpScreen.Title;
				BodyText.SetActive(helpScreen.Text != null);
				if (helpScreen.Text != null)
					BodyText.GetComponent<Text>().text = helpScreen.Text;
				Image.SetActive(helpScreen.Image != null);
				if (helpScreen.Image != null)
				{
					Image.GetComponent<Image>().sprite = helpScreen.Image;
					//Image.GetComponent<Image>().preserveAspect = true; // TODO is this needed?
				}
			}
		}
		else
		{
			if (showing)
			{
				SetChildrenActive(false);
				showing = false;
			}
			if (transform.localScale.x > 0.01f)
				transform.localScale -= new Vector3(speed, speed);
			else
			{
				gameObject.SetActive(false);
				if (GameManager != null)
					GameManager.GetComponent<GameManager>().Resume();
			}
		}
	}
}
