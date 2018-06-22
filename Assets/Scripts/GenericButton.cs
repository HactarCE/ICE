using AssemblyCSharp.Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class GenericButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	protected bool disabled;
	protected bool hover;
	protected bool clicked;

	AudioSource hoverSource;
	AudioSource clickSource;

	// Use this for initialization
	protected virtual void Awake()
	{
		hoverSource = Utils.AddAudioSource(gameObject, "ButtonHover");
		clickSource = Utils.AddAudioSource(gameObject, "ButtonClick");
	}

	// Use this for initialization tooo
	protected virtual void Start()
	{
		UpdateState();
	}

	void OnEnable()
	{
		// It's ok if this errors, I guess?
		//UpdateState();
		hover = false;
		UpdateState();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0) && hover)
		{
			clicked = true;
			clickSource.Play();
			UpdateState();
		}
		else if (Input.GetMouseButtonUp(0) && clicked)
		{
			clicked = false;
			UpdateState();
			if (hover && !disabled)
				HandleClick();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		hover = true;
		hoverSource.Play();
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
