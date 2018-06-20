using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp.Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopupPanel : MonoBehaviour
{

	public GameObject GameManager;

	bool show;
	bool showing;

	protected float speed = 1f / 16f;

	// Use this for initialization
	virtual protected void Awake()
	{
		gameObject.SetActive(false);
	}

	void SetChildrenActive(bool active)
	{
		for (int i = 0; i < transform.childCount; i++)
			transform.GetChild(i).gameObject.SetActive(active);
	}

	protected void Show()
	{
		if (GameManager != null)
			GameManager.GetComponent<GameManager>().Pause();
		gameObject.SetActive(true);
		SetChildrenActive(false);
		show = true;
		showing = false;
		transform.localScale = new Vector2(speed, speed);
	}

	abstract protected void FinishShow();

	abstract protected bool CheckShouldClose();

	// Update is called once per frame
	void Update()
	{
		if (show)
		{
			if (transform.localScale.x < 0.99f)
				transform.localScale += new Vector3(speed, speed);
			else if (showing)
			{
				if (CheckShouldClose())
					show = false;
			}
			else
			{
				showing = true;
				SetChildrenActive(true);
				FinishShow();
			}
		}
		else
		{
			if (showing)
			{
				showing = false;
				SetChildrenActive(false);
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
