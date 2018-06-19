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

	// Use this for initialization
	void Start()
	{

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
		GameManager.GameState gameState = MyGameManager.GetComponent<GameManager>().CurrentGameState;
		if (gameState == GameManager.GameState.SWEEPING)
		{
			float maxSweepOffset = 0.4f;
			Vector3 offsetDelta = new Vector3(0f, 0.03f);
			if (Utils.GetKey_Up() && SweepOffset.y < maxSweepOffset)
				SweepOffset += offsetDelta;
			if (Utils.GetKey_Down() && SweepOffset.y > -maxSweepOffset)
				SweepOffset -= offsetDelta;
			if (Utils.GetKey_Confirm())
				broomAnimate += 0.4f;
			Broom.transform.localPosition = baseBroomOffset + new Vector3(0f, Mathf.Sin(broomAnimate) * 0.15f);
		}
		else if (gameState == GameManager.GameState.WATCHING)
		{
			if (Broom.GetComponent<SpriteRenderer>().color.a > 0)
			{
				Broom.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.03f);
			}
			else if (GetComponent<SpriteRenderer>().color.a > 0)
			{
				GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.03f);
			}
		}
	}

	protected override void LateUpdate()
	{
		rockOffset = baseRockOffset + SweepOffset;
		base.LateUpdate();
	}
}
