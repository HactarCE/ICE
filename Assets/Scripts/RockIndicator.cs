using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RockIndicator : MonoBehaviour
{

	public GameObject Handle;

	public GameObject RealRock;

	// Use this for initialization
	void Start()
	{
		Handle.GetComponent<Image>().color = RealRock.GetComponent<Rock>().Team.Color;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 rockPos = RealRock.transform.localPosition;
		transform.localPosition = rockPos * 20f; // eek magic constant
	}
}
