using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp.Assets.Scripts;
using UnityEngine;

public class Sweeper : Person
{

	public Vector3 SweepOffset;
	Vector3 baseRockOffset = new Vector3(1.2f, -1f);
	float broomAnimate;
	Vector3 baseBroomOffset = new Vector3(-0.2f, 0.35f);

	public GameObject Broom;

	bool paused;

	AudioSource singleSweepSource;
	AudioSource multiSweepSource;

	// Use this for initialization
	void Start()
	{
		singleSweepSource = Utils.AddAudioSource(gameObject, "SingleSweep");
		multiSweepSource = Utils.AddAudioSource(gameObject, "MultiSweep");
	}

	public void ResetOffsets()
	{
		SweepOffset = new Vector3();
		broomAnimate = 0;
		GetComponent<SpriteRenderer>().color = Color.white;
		Broom.GetComponent<SpriteRenderer>().color = Color.white;
	}

	// Update is called once per frame
	void Update()
	{
		if (paused) return;
		GameManager.GameState gameState = MyGameManager.GetComponent<GameManager>().CurrentGameState;
		if (gameState == GameManager.GameState.SWEEPING)
		{
			float maxSweepOffset = 0.4f;
			Vector3 offsetDelta = new Vector3(0f, 0.03f);
			if (Utils.GetKey_Up() && SweepOffset.y < maxSweepOffset)
				SweepOffset += offsetDelta;
			if (Utils.GetKey_Down() && SweepOffset.y > -maxSweepOffset)
				SweepOffset -= offsetDelta;
			if (Utils.GetKey_Confirm() || broomAnimate > 0)
			{
				if (broomAnimate == 0f)
				{
					if (Utils.GetKeyDown_Confirm())
						singleSweepSource.Play();
					else
						multiSweepSource.Play();
				}
				broomAnimate += 0.6f;
			}
			if (broomAnimate >= 2 * Mathf.PI)
				broomAnimate = 0;
			Broom.transform.localPosition = baseBroomOffset + new Vector3(0f, Mathf.Sin(broomAnimate) * 0.15f);
		}
		else if (gameState == GameManager.GameState.WATCHING)
		{
			if (Broom.GetComponent<SpriteRenderer>().color.a > 0)
			{
				Broom.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.05f);
			}
			else if (GetComponent<SpriteRenderer>().color.a > 0)
			{
				GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.05f);
			}
		}
	}

	protected override void LateUpdate()
	{
		rockOffset = baseRockOffset + SweepOffset;
		base.LateUpdate();
	}

	void OnPauseGame()
	{
		paused = true;
	}

	void OnResumeGame()
	{
		paused = false;
	}
}
