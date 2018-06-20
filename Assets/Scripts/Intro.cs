using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
	string[] dramaticTexts = {
		"Each winter...",
		"The world's best Curling Teams convene...",
		"At the greatest curling championship...",
		"The..."
	};

	int textIndex = -1;
	int subframe = 0;

	int wait;
	bool opacityDown;

	public Text Text;

	// Use this for initialization
	void Start()
	{
		nextText();
	}

	// Update is called once per frame
	void Update()
	{
		//if (Input.anyKeyDown || (textIndex >= dramaticTexts.Length - 1 && opacityDown))
		if (Input.anyKeyDown || textIndex >= dramaticTexts.Length)
			SceneManager.LoadScene("MainMenu");
	}

	// Update is called once per physics frame
	void FixedUpdate()
	{
		if (wait-- > 0)
			return;
		//if (++subframe >= 20)
		//{
		//	Text.fontSize += 1;
		//	subframe = 0;
		//}
		Text.transform.localScale += new Vector3(0.001f, 0.001f, 0.001f);
		if (opacityDown)
		{
			if (Text.color.a > 0.01f)
				Text.color = new Color(1f, 1f, 1f, Text.color.a - 0.005f);
			else
				nextText();
		}
		else
		{
			if (Text.color.a < 0.99f)
				Text.color = new Color(1f, 1f, 1f, Text.color.a + 0.005f);
			else
				opacityDown = true;
		}
	}

	void nextText()
	{
		Text.text = dramaticTexts[++textIndex];
		Text.color = Color.clear;
		float size = 0.5f + 0.1f * textIndex;
		Text.transform.localScale = new Vector3(size, size, size);
		opacityDown = false;
		wait = 30;
	}
}
