using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp.Assets.Scripts;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

	AudioSource musicSource;

	// Use this for initialization
	void Start()
	{
		musicSource = Utils.AddAudioSource(gameObject, "DeliberateThought");
		musicSource.loop = true;
		musicSource.volume = 0.2f;
		SetEnabled(true);
		musicSource.Play();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SetEnabled(bool enabled)
	{
		musicSource.mute = !enabled;
	}
}
