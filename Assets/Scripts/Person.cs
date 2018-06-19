using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{

	public GameObject MyGameManager;

	protected Vector3 rockOffset;

	// LateUpdate is called once per frame, after all transforms have completed their Update()
	protected virtual void LateUpdate()
	{
		transform.position = MyGameManager.GetComponent<GameManager>().ActiveRock.transform.position + rockOffset;
	}
}
