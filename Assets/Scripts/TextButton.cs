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

	Text textComponent;
	bool hover = false;
	bool clicked = false;

	public float hoverScale = 1.05f;
	public float clickScale = 1.08f;

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
			if (clicked)
			{
				color = ClickColor;
				transform.localScale = new Vector3(clickScale, clickScale, clickScale);
			}
			else
			{
				color = HoverColor;
				transform.localScale = new Vector3(hoverScale, hoverScale, hoverScale);
			}
		}
		else
		{
			color = IdleColor;
			transform.localScale = Vector3.one;
		}
		textComponent.color = color;
	}

}
