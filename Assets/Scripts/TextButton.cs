using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	public Color IdleColor;
	public Color HoverColor;
	public Color ClickColor;

	[System.Serializable]
	public class ClickEvent : UnityEvent { }

	public ClickEvent onClick = new ClickEvent();

	private Text textComponent;
	private bool hover = false;
	private bool clicked = false;

	private float hoverScale = 1.05f;

	// Use this for initialization
	void Start()
	{
		textComponent = GetComponent<Text>();
		Rect box = textComponent.rectTransform.rect;
		BoxCollider boxCollider = (BoxCollider)gameObject.AddComponent(typeof(BoxCollider));
		boxCollider.size = new Vector3(box.width, box.height, 1);
		updateColor();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0) && hover)
		{
			clicked = true;
			updateColor();
		}
		if (Input.GetMouseButtonUp(0) && clicked)
		{
			clicked = false;
			updateColor();
			if (hover) onClick.Invoke();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		hover = true;
		updateColor();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		hover = false;
		updateColor();
	}

	private void updateColor()
	{
		Color color;
		if (hover)
		{
			if (clicked) color = ClickColor;
			else color = HoverColor;
			transform.localScale = new Vector3(hoverScale, hoverScale, hoverScale);
		}
		else
		{
			color = IdleColor;
			transform.localScale = Vector3.one;
		}
		textComponent.color = color;
	}

}
