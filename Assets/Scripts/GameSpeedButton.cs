using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpeedButton : TextButton
{

	public float GameSpeed;

	bool selected;

	protected override void UpdateState()
	{
		if (selected)
		{
			textComponent.color = ClickColor;
			transform.localScale = Vector3.one;
		}
		else
			base.UpdateState();
	}

	protected override void HandleClick()
	{
		GetComponentInParent<GameSpeedSelector>().SelectGameSpeed(GameSpeed);
	}

	public void SelectGameSpeed(float speed)
	{
		selected = speed == GameSpeed;
		UpdateState();
	}
}
