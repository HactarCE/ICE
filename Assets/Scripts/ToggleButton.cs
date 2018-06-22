using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleButton : TextButton
{

	public Color IdleColor_Checked;
	public Color HoverColor_Checked;
	public Color ClickColor_Checked;
	public Color IdleColor_Unchecked;
	public Color HoverColor_Unchecked;
	public Color ClickColor_Unchecked;

	[System.Serializable]
	public class ToggleEvent : UnityEvent<bool> { }

	public ToggleEvent onToggle = new ToggleEvent();

	public bool IsChecked;

	// Use this for initialization
	protected override void Start()
	{
		onClick.AddListener(ToggleState);
		base.Start();
	}

	protected override void UpdateState()
	{
		textComponent.text = (IsChecked ? "✔" : "✘") + textComponent.text.Substring(1);
		IdleColor = IsChecked ? IdleColor_Checked : IdleColor_Unchecked;
		HoverColor = IsChecked ? HoverColor_Checked : HoverColor_Unchecked;
		ClickColor = IsChecked ? ClickColor_Checked : ClickColor_Unchecked;
		base.UpdateState();
	}

	void ToggleState()
	{
		IsChecked = !IsChecked;
		UpdateState();
		onToggle.Invoke(IsChecked);
	}
}
