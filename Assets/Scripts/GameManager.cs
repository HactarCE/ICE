using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp.Assets.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public Camera Camera;

	public GameObject UITop, UIBottom;
	float offsetX, offsetY, minX, minY, maxX, maxY;

	public GameObject RockPrefab, TrajectoryPrefab, TrajectoryIndicatorPrefab;
	public GameObject Minimap, Thrower, Sweeper;
	public GameObject Hogline1, Hogline2, House;

	GameObject[] rocks = { };
	public GameObject ActiveRock;

	GameObject trajectory, trajectoryIndicator;

	public GameState CurrentGameState = GameState.PRE_GAME;

	Team p1, p2;
	public static int P1Score, P2Score;
	public int P1ScoreThisEnd = 0, P2ScoreThisEnd = 0;
	public int End = 1;
	public int Throw;
	public Turn CurrentTurn;

	public enum GameState { PRE_GAME, AIMING, THROWING, SWEEPING, WATCHING }

	public enum Turn { P1, P2 }

	float watchingFrameCounter;

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

	//float GetRealUIHeight(GameObject uiElement)
	//{
	//	if (uiElement == null) return 0;
	//	Vector3 vector = new Vector3(0, uiElement.GetComponent<RectTransform>().rect.height, 0);
	//	Debug.Log(vector);
	//	vector = Camera.cameraToWorldMatrix.MultiplyVector(vector);
	//	Debug.Log(vector);
	//	Debug.Log(Camera.cameraToWorldMatrix);
	//	return vector.y;
	//}

	// Update is called once per frame
	void Update()
	{
		if (CurrentGameState == GameState.PRE_GAME)
		{
			StartEnd();
		}
		else if (CurrentGameState == GameState.AIMING)
		{
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
		}
		else if (CurrentGameState == GameState.THROWING)
		{
			if (Utils.GetKey_Up())
			{
				ActiveRock.GetComponent<Rigidbody2D>().angularVelocity += 3;
			}
			if (Utils.GetKey_Down())
			{
				ActiveRock.GetComponent<Rigidbody2D>().angularVelocity -= 3;
			}
			if (Utils.GetKeyDown_Confirm() || ActiveRock.transform.position.x > Hogline1.transform.position.x)
			{
				ReleaseRock();
			}
		}
		else if (CurrentGameState == GameState.SWEEPING)
		{
			if (ActiveRock.transform.position.x > House.transform.position.x
				|| ActiveRock.GetComponent<Rigidbody2D>().velocity.magnitude < 0.2f
				|| ActiveRock.GetComponent<Rock>().OutOfBounds)
			{
				watchingFrameCounter = 0;
				CurrentGameState = GameState.WATCHING;
			}
		}
		else if (CurrentGameState == GameState.WATCHING)
		{
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
					if (rb.velocity.magnitude < 0.01f || Mathf.Abs(rb.angularVelocity) < 0.01f)
						watchingFrameCounter = 0;
				}
			}
		}
	}

	void LateUpdate()
	{
		if (ActiveRock != null)
		{
			Vector3 rockPos = ActiveRock.transform.position;
			float x = Mathf.Clamp(rockPos.x + offsetX, minX, maxX);
			float y = Mathf.Clamp(rockPos.y + offsetY, minY, maxY);
			float z = Camera.transform.position.z;
			Camera.transform.position = new Vector3(x, y, z);
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
				rb.velocity += new Vector2(0f, Sweeper.GetComponent<Sweeper>().SweepOffset.y * rb.velocity.magnitude / 10000);
			}
		}
	}

	public Team GetCurrentTeam()
	{
		return CurrentTurn == Turn.P1 ? p1 : p2;
	}

	void KeepScores()
	{
		// TODO: add scores from current end to scores for whole game
		// also probably refactor this?
	}

	void StartEnd()
	{
		Throw = 1;
		// TODO update bar at bottom
		StartThrow();
	}

	void StartThrow()
	{
		ActiveRock = Instantiate(RockPrefab);
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
	}

	void ReleaseRock()
	{
		Thrower.GetComponent<Thrower>().Velocity = ActiveRock.GetComponent<Rigidbody2D>().velocity;
		CurrentGameState = GameState.SWEEPING;
	}

	void EndThrow()
	{
		CurrentTurn = CurrentTurn == Turn.P1 ? Turn.P2 : Turn.P1;
		if (++Throw <= GameConfig.ThrowCount)
			StartThrow();
	}

	void EndEnd()
	{
		P1Score += P1ScoreThisEnd;
		P2Score += P2ScoreThisEnd;
		if (++End >= GameConfig.EndCount)
			EndGame();
		else
		{
			P1ScoreThisEnd = 0;
			P2ScoreThisEnd = 0;
			// TODO player that scored points goes first in next round
			foreach (GameObject rock in rocks)
			{
				Destroy(rock);
			}
			StartEnd();
		}
	}

	void EndGame()
	{

	}

}
