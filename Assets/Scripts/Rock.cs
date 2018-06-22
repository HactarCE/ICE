using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp.Assets.Scripts;
using UnityEngine;

public class Rock : MonoBehaviour
{

	public static float CURL_COEF = 3f / 1000000f;
	// 0.3 millionths
	// higher number -> more curling

	public static float LIN_FRIC_COEF = 0.0130f;
	public static float ANG_FRIC_COEF = 2;
	// higher number -> more friction

	public bool UseReducedFriction = false;

	public GameObject Handle;

	public GameObject IndicatorPrefab;
	public GameObject Minimap;

	public GameObject Indicator;

	AudioSource slidingSource;
	AudioSource collisionSource;

	Team team;

	public Team Team
	{
		get
		{
			return team;
		}
		set
		{
			team = value;
			Handle.GetComponent<SpriteRenderer>().color = Team.Color;
		}
	}

	public bool OutOfBounds
	{
		get
		{
			return Mathf.Abs(transform.position.x) > 76.8 || Mathf.Abs(transform.position.y) > 5;
		}
	}

	// Use this for initialization
	void Start()
	{
		if (Indicator == null) InstantiateIndicator();
		slidingSource = Utils.AddAudioSource(gameObject, "RockSlide");
		collisionSource = Utils.AddAudioSource(gameObject, "RockHit");
		slidingSource.Play();
		slidingSource.loop = true;
		MuteTemporarily();
	}

	// FixedUpdate is called once per physics frame
	void FixedUpdate()
	{
		float linFric = LIN_FRIC_COEF, angFric = ANG_FRIC_COEF;
		if (UseReducedFriction)
		{
			linFric /= 1.5f;
			angFric /= 1.5f;
		}
		Rigidbody2D rb = GetComponent<Rigidbody2D>();

		// LINEAR VELOCITY
		rb.velocity = Quaternion.Euler(0f, 0f, rb.angularVelocity * rb.velocity.magnitude * CURL_COEF) * rb.velocity;
		// f=μN; N = normal force and f = frictional force
		// using gravity*mass for N, mass cancels out on both sides, resulting in:
		// a=μg
		// 0.17 = (16 ft/s^2) * (0.64 units/ft) / (60 frame/s) = acceleration due to gravity (units/s/frame)
		float reduction = linFric * 0.17f;
		if (rb.velocity.magnitude < reduction) rb.velocity = new Vector2();
		else rb.velocity -= rb.velocity / rb.velocity.magnitude * reduction;

		// ANGULAR VELOCITY
		// I guess angular friction is inversely proportional to linear speed? This makes no sense, but it works.
		float dr = angFric / Mathf.Max(0.01f, rb.velocity.magnitude) / 60;
		if (Mathf.Abs(rb.angularVelocity) < dr) rb.angularVelocity = 0;
		else rb.angularVelocity -= Mathf.Sign(rb.angularVelocity) * dr;
		slidingSource.volume = Mathf.Sqrt(rb.velocity.magnitude) / 30f;
	}

	public void MuteTemporarily()
	{
		slidingSource.volume = 0f;
	}

	void OnDestroy()
	{
		Destroy(Indicator);
	}

	public void InstantiateIndicator()
	{
		Indicator = Instantiate(IndicatorPrefab);
		Indicator.GetComponent<RockIndicator>().RealRock = gameObject;
		Indicator.transform.SetParent(Minimap.transform);
	}

	void OnCollisionEnter2D(Collision2D hit)
	{
		Debug.Log("collide!");
		collisionSource.volume = Mathf.Sqrt(hit.relativeVelocity.magnitude) / 5f;
		collisionSource.Play();
	}
}
