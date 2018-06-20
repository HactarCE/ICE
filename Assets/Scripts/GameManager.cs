using System;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp.Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	const float SWEEP_COEF = 3f / 10000f;

	public Camera Camera;
	public GameObject CameraIndicator;

	public GameObject UITop, UIBottom;
	float offsetX, offsetY, minX, minY, maxX, maxY;

	public GameObject RockPrefab, TrajectoryPrefab, TrajectoryIndicatorPrefab;
	public GameObject Minimap, Thrower, Sweeper;
	public GameObject Hogline1, Hogline2, House;

	List<GameObject> rocks = new List<GameObject>();
	public GameObject ActiveRock;

	GameObject trajectory, trajectoryIndicator;

	public GameState CurrentGameState = GameState.PRE_GAME;

	class RockComparer : IComparer<GameObject>
	{
		readonly Vector3 housePos;

		public RockComparer(GameObject house)
		{
			housePos = house.transform.position;
		}

		int IComparer<GameObject>.Compare(GameObject a, GameObject b)
		{
			float aDistance = (a.transform.position - housePos).magnitude;
			float bDistance = (b.transform.position - housePos).magnitude;
			return (int)Mathf.Sign(aDistance - bDistance);
		}
	}

	Team p1, p2;
	public static int P1Score, P2Score;
	public int P1ScoreThisEnd
	{
		get
		{
			int score = 0;
			foreach (GameObject rock in rocks)
			{
				if (rock.GetComponent<Rock>().Team.Color == p1.Color // kinda sloppy, but whatever
					&& IsRockValid(rock))
					score++;
				else break;
			}
			return score;
		}
	}
	public int P2ScoreThisEnd
	{
		get
		{
			int score = 0;
			foreach (GameObject rock in rocks)
			{
				if (rock.GetComponent<Rock>().Team.Color == p2.Color // again, kinda sloppy
					&& IsRockValid(rock))
					score++;
				else break;
			}
			return score;
		}
	}
	public int End = 1;
	public int Throw;
	public Turn CurrentTurn;

	public enum GameState { PRE_GAME, AIMING, THROWING, SWEEPING, WATCHING }

	HelpScreen.ID helpStage;
	public HelpPanel HelpPanel;
	public PausePanel PausePanel;
	public EndPopup EndPopup; // consistent naming FTW?

	public enum Turn { P1, P2 }

	int watchingFrameCounter;

	public bool Paused;
	float oldGameSpeed;

	// Use this for initialization
	void Start()
	{
		float cameraHeight = 2f * Camera.orthographicSize;
		float cameraWidth = cameraHeight * Camera.aspect;
		Bounds IceBounds = new Bounds(new Vector3(), new Vector3(96f, 10f)); // ouch
		offsetX = 0f;
		float uiHeightTop = 205f / 1080f * 10f; // kill me now
		float uiHeightBottom = 50f / 1080f * 10f; // please
		offsetY = (uiHeightTop - uiHeightBottom) / 2f;
		minX = IceBounds.min.x + cameraWidth / 2f;
		minY = IceBounds.min.y + cameraHeight / 2f - uiHeightBottom;
		maxX = IceBounds.max.x - cameraWidth / 2f;
		maxY = IceBounds.max.y - cameraHeight / 2f + uiHeightTop;
		p1 = GameConfig.Teams[GameConfig.TeamID_1];
		p2 = GameConfig.Teams[GameConfig.TeamID_2];
		P1Score = 0;
		P2Score = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (Paused) return;
		if (Utils.GetKeyDown_Escape())
		{
			PausePanel.gameObject.SetActive(true);
			PausePanel.ShowPauseMenu();
		}
		switch (helpStage)
		{
			case HelpScreen.ID.SCORING:
			case HelpScreen.ID.INTERFACE:
			case HelpScreen.ID.AIMING_AND_THROWING:
				ShowHelp();
				break;
			case HelpScreen.ID.SPIN:
				if (CurrentGameState == GameState.THROWING)
					ShowHelp();
				break;
			case HelpScreen.ID.SWEEPING:
				if (CurrentGameState == GameState.SWEEPING)
					ShowHelp();
				break;
			case HelpScreen.ID.FINAL:
				if (watchingFrameCounter == 1)
					ShowHelp();
				break;
		}
		if (Paused) return;
		switch (CurrentGameState)
		{
			case GameState.PRE_GAME:
				StartEnd();
				break;
			case GameState.AIMING:
				float currentRotation = trajectory.transform.rotation.eulerAngles.z;
				float rotateSpeed = 0.1f;
				float rotateLimit = 3f;
				if (Utils.GetKey_Up() && !(180 > currentRotation && currentRotation > rotateLimit))
				{
					trajectory.transform.Rotate(0, 0, rotateSpeed);
					trajectoryIndicator.transform.Rotate(0, 0, rotateSpeed);
				}
				if (Utils.GetKey_Down() && !(180 < currentRotation && currentRotation < 360 - rotateLimit))
				{
					trajectory.transform.Rotate(0, 0, -rotateSpeed);
					trajectoryIndicator.transform.Rotate(0, 0, -rotateSpeed);
				}
				trajectoryIndicator.transform.rotation = trajectory.transform.rotation;
				if (Utils.GetKeyDown_Confirm())
				{
					ActiveRock.transform.position += new Vector3(0.45f, 0); // ick magic constant
					Thrower.GetComponent<SpriteRenderer>().sprite = GetCurrentTeam().ThrowSprite;
					float rad = Mathf.Deg2Rad * trajectory.transform.rotation.eulerAngles.z;
					Destroy(trajectory);
					Destroy(trajectoryIndicator);
					ActiveRock.GetComponent<Rigidbody2D>().velocity = new Vector3(4.2f * Mathf.Cos(rad), 4.2f * Mathf.Sin(rad));
					CurrentGameState = GameState.THROWING;
				}

				break;
			case GameState.THROWING:
				if (Utils.GetKeyDown_Confirm() || ActiveRock.transform.position.x > Hogline1.transform.position.x)
				{
					ReleaseRock();
				}

				break;
			case GameState.SWEEPING:
				if (ActiveRock.transform.position.x > House.transform.position.x
					|| ActiveRock.GetComponent<Rigidbody2D>().velocity.magnitude < 0.2f
					|| ActiveRock.GetComponent<Rock>().OutOfBounds)
				{
					watchingFrameCounter = 0;
					CurrentGameState = GameState.WATCHING;
				}

				break;
			case GameState.WATCHING:
				if (watchingFrameCounter > 0)
				{
					if (watchingFrameCounter++ > 90) // 1.5 seconds
						EndThrow();
				}
				else
				{
					watchingFrameCounter = 1;
					foreach (GameObject rock in rocks)
					{
						Rigidbody2D rb = rock.GetComponent<Rigidbody2D>();
						if (!rock.GetComponent<Rock>().OutOfBounds
							&& (rb.velocity.magnitude > 0.01f
								|| Mathf.Abs(rb.angularVelocity) > 0.01f))
							watchingFrameCounter = 0;
					}
				}

				break;
		}
	}

	void LateUpdate()
	{
		if (Paused) return;
		if (ActiveRock != null)
		{
			Vector3 rockPos = ActiveRock.transform.position;
			float x = Mathf.Clamp(rockPos.x + offsetX, minX, maxX);
			float y = Mathf.Clamp(rockPos.y + offsetY, minY, maxY);
			float z = Camera.transform.position.z;
			Camera.transform.position = new Vector3(x, y, z);
			// once this works, DON'T EVER TOUCH IT AGAIN
			Vector3 p = ActiveRock.transform.localPosition * 20f; // eek magic constant
			CameraIndicator.transform.localPosition = new Vector3(
				Mathf.Clamp(p.x, -768, 768f), // AAAAAAAAAHHHHHHHHHHHHH
				Mathf.Clamp(p.y, -17.5f, 17.5f), // AAAAAAAHHHHHHHHHHHH
				CameraIndicator.transform.localPosition.z
			// I don't even know where those numbers came from
			);
		}
		rocks.Sort(new RockComparer(House));
		if (CurrentGameState == GameState.THROWING)
		{
			if (Utils.GetKey_Up())
				ActiveRock.GetComponent<Rigidbody2D>().angularVelocity += 3;
			if (Utils.GetKey_Down())
				ActiveRock.GetComponent<Rigidbody2D>().angularVelocity -= 3;
		}
	}

	// FixedUpdate is called once for physics frame
	void FixedUpdate()
	{
		if (CurrentGameState == GameState.SWEEPING)
		{
			ActiveRock.GetComponent<Rock>().UseReducedFriction = Utils.GetKey_Confirm();
			if (Utils.GetKey_Confirm())
			{
				Rigidbody2D rb = ActiveRock.GetComponent<Rigidbody2D>();
				rb.velocity += new Vector2(0f, Sweeper.GetComponent<Sweeper>().SweepOffset.y * rb.velocity.magnitude * SWEEP_COEF);
			}
		}
	}

	public void ShowHelp()
	{
		HelpPanel.gameObject.SetActive(true); // I should not have to do this?
		HelpPanel.ShowHelp(HelpScreen.HelpScreens[helpStage]);
		helpStage++;
	}

	public void Pause()
	{
		if (!Paused)
		{
			Paused = true;
			oldGameSpeed = Time.timeScale;
			Time.timeScale = 0;
		}
	}

	public void Resume()
	{
		if (Paused)
		{
			Paused = false;
			Time.timeScale = oldGameSpeed;
		}
	}

	public Team GetCurrentTeam()
	{
		return CurrentTurn == Turn.P1 ? p1 : p2;
	}

	public bool IsRockValid(GameObject rock)
	{
		return !rock.GetComponent<Rock>().OutOfBounds && rock.transform.position.x >= Hogline2.transform.position.x;
	}

	void StartEnd()
	{
		Throw = 1;
		rocks = new List<GameObject>();
		StartThrow();
	}

	void StartThrow()
	{
		ActiveRock = Instantiate(RockPrefab);
		rocks.Add(ActiveRock);
		ActiveRock.GetComponent<Rock>().Team = GetCurrentTeam();
		ActiveRock.GetComponent<Rock>().Minimap = Minimap;
		ActiveRock.GetComponent<Rock>().InstantiateIndicator();
		Thrower.GetComponent<SpriteRenderer>().sprite = GetCurrentTeam().AimSprite;
		Sweeper.GetComponent<SpriteRenderer>().sprite = GetCurrentTeam().SweepSprite;
		Sweeper.GetComponent<Sweeper>().ResetOffsets();
		trajectory = Instantiate(TrajectoryPrefab);
		trajectoryIndicator = Instantiate(TrajectoryIndicatorPrefab);
		trajectory.transform.SetParent(ActiveRock.transform);
		trajectoryIndicator.transform.SetParent(ActiveRock.GetComponent<Rock>().Indicator.transform);
		trajectory.transform.localPosition = new Vector3();
		trajectoryIndicator.transform.localPosition = new Vector3();
		CurrentGameState = GameState.AIMING;
		if (CurrentTurn == Turn.P1 ? GameConfig.P1ShowTutorial : GameConfig.P2ShowTutorial)
			helpStage = HelpScreen.ID.SCORING;
	}

	void ReleaseRock()
	{
		Thrower.GetComponent<Thrower>().Velocity = ActiveRock.GetComponent<Rigidbody2D>().velocity;
		CurrentGameState = GameState.SWEEPING;
	}

	void EndThrow()
	{
		if (CurrentTurn == Turn.P1)
			GameConfig.P1ShowTutorial = false;
		else
			GameConfig.P2ShowTutorial = false;
		foreach (GameObject rock in rocks)
		{
			if (!IsRockValid(rock))
			{
				Destroy(rock);
				rocks.Remove(rock);
			}
		}
		CurrentTurn = CurrentTurn == Turn.P1 ? Turn.P2 : Turn.P1;
		if (++Throw <= GameConfig.ThrowCount)
			StartThrow();
		else
			EndEnd();
	}

	void EndEnd()
	{
		EndPopup.gameObject.SetActive(true);
		EndPopup.ShowEndOver();
		P1Score += P1ScoreThisEnd;
		P2Score += P2ScoreThisEnd;
		if (++End <= GameConfig.EndCount)
		{
			if (P1ScoreThisEnd > 0)
				CurrentTurn = Turn.P1;
			else if (P2ScoreThisEnd > 0)
				CurrentTurn = Turn.P2;
			else
				CurrentTurn = CurrentTurn == Turn.P1 ? Turn.P2 : Turn.P1;
			foreach (GameObject rock in rocks)
				Destroy(rock);
			StartEnd();
		}
		else
			EndGame();
	}

	void EndGame()
	{
		SceneManager.LoadScene("EndOfGame");
	}

}
