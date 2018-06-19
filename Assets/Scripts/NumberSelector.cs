using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NumberSelector : MonoBehaviour
{

	public int[] Options;
	public int SelectedIndex;

	public GameObject Label;

	[System.Serializable]
	public class SelectEvent : UnityEvent<int> { }

	public SelectEvent onSelect = new SelectEvent();

	// Use this for initialization
	void Start()
	{
		SelectionChanged();
	}

	public void Increase()
	{
		if (SelectedIndex < Options.Length - 1)
		{
			SelectedIndex++;
			SelectionChanged();
		}
	}

	public void Decrease()
	{
		if (SelectedIndex > 0)
		{
			SelectedIndex--;
			SelectionChanged();
		}
	}

	void SelectionChanged()
	{
		int selected = Options[SelectedIndex];
		onSelect.Invoke(selected);
		Label.GetComponent<Text>().text = selected.ToString();
	}
}
