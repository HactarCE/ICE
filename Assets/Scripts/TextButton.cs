using UnityEngine;
using UnityEngine.Events;
using AssemblyCSharp.Assets.Scripts;
using UnityEngine.UI;

public class TextButton : GenericButton
{

	public Color IdleColor;
	public Color HoverColor;
	public Color ClickColor;
	public Color DisabledColor;

	[System.Serializable]
	public class ClickEvent : UnityEvent { }

	public ClickEvent onClick = new ClickEvent();

	protected Text textComponent;

	public float hoverScale = 1.05f;
	public float clickScale = 1.08f;

	// Use this for initialization
	void Awake()
	{
		textComponent = GetComponent<Text>();
		Rect box = textComponent.rectTransform.rect;
		BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
		boxCollider.size = new Vector2(box.width, box.height);
		UpdateState();
	}

	protected override void UpdateState()
	{
		Color color;
		if (disabled)
		{
			color = DisabledColor;
			transform.localScale = Vector3.one;
		}
		else if (hover)
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

	protected override void HandleClick()
	{
		onClick.Invoke();
	}
}
