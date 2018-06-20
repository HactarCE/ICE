using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp.Assets.Scripts;
using UnityEngine;

public class PausePanel : PopupPanel
{

	bool shouldClose;

	// Use this for initialization
	protected override void Awake()
	{
		speed = 1f / 8f;
		base.Awake();
	}

	public void ShowPauseMenu()
	{
		shouldClose = false;
		Show();
	}

	protected override void FinishShow()
	{
	}

	protected override bool CheckShouldClose()
	{
		return (Utils.GetKeyDown_Escape() || Utils.GetKeyDown_Confirm() || shouldClose);
	}

	public void Close()
	{
		shouldClose = true;
	}
}
