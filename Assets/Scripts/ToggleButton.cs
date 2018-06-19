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

	bool isChecked;

	// Use this for initialization
	void Start()
	{
		onClick.AddListener(ToggleState);
	}

	protected override void UpdateState()
	{
		IdleColor = isChecked ? IdleColor_Checked : IdleColor_Unchecked;
		HoverColor = isChecked ? HoverColor_Checked : HoverColor_Unchecked;
		ClickColor = isChecked ? ClickColor_Checked : ClickColor_Unchecked;
		base.UpdateState();
	}

	void ToggleState()
	{
		isChecked = !isChecked;
		textComponent.text = (isChecked ? "✔" : "✘") + textComponent.text.Substring(1);
		UpdateState();
		onToggle.Invoke(isChecked);
	}
}
