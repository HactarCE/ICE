using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class GenericButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	protected bool disabled;
	protected bool hover;
	protected bool clicked;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0) && hover)
		{
			clicked = true;
			UpdateState();
		}
		else if (Input.GetMouseButtonUp(0) && clicked)
		{
			clicked = false;
			UpdateState();
			if (hover && !disabled) HandleClick();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		hover = true;
		UpdateState();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		hover = false;
		UpdateState();
	}

	public void SetDisabled(bool disabled)
	{
		this.disabled = disabled;
		UpdateState();
	}

	abstract protected void UpdateState();

	abstract protected void HandleClick();

}
