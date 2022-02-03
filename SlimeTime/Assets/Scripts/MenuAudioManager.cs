using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    private AudioSource source;
	[SerializeField] private string introBGMusic;
	
	void Awake() {
		source = this.GetComponent<AudioSource>();
	}
	
	public void PlayIntroMusic() {
		PlayMusic(Resources.Load("Music/" + introBGMusic) as AudioClip);
	}

	private void PlayMusic(AudioClip clip) {
		source.clip = clip;
		source.Play();
	}

	public void StopMusic() {
		source.Stop();
	}
}
