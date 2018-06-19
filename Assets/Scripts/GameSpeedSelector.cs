using UnityEngine;

public class GameSpeedSelector : MonoBehaviour
{

	public GameObject[] textButtons;

	// Use this for initialization
	void Start()
	{
		SelectGameSpeed(1f);
	}

	public void SelectGameSpeed(float speed)
	{
		Time.timeScale = speed;
		foreach (GameObject tb in textButtons)
			tb.GetComponent<GameSpeedButton>().SelectGameSpeed(speed);
	}
}
