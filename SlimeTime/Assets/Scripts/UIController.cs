using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	/**
	*	AUDIO
	*/
	[SerializeField] private GameObject audioContext;
	private AudioManager audioManager;
	private bool isAudioActive = true;
	[SerializeField] private Sprite audioEnabled;
	[SerializeField] private Sprite audioDisabled;
	AudioSource source;

	void Awake() {
		audioManager = audioContext.GetComponent<AudioManager>();
		source = this.GetComponent<AudioSource>();
	}

	void Start() {
		audioManager.PlayIntroMusic();
	}

    public void switchAudio(GameObject button) {
		
		Image image = button.GetComponent<Image>();

		if(isAudioActive) {
			audioManager.StopMusic();
			image.sprite = audioDisabled;
		}
		else {
			audioManager.PlayIntroMusic();
			image.sprite = audioEnabled;
		}

		isAudioActive = !isAudioActive;
	}

	public void playButtonHoverSould() {
		source.clip = Resources.Load("Sound/button_hover") as AudioClip;
		source.Play();
	}

	public void playButtonClickSould() {
		source.clip = Resources.Load("Sound/button_click") as AudioClip;
		source.Play();
	}

	public void exit() {
		//Build version exit
		Application.Quit();

		//Unity editor exit
		UnityEditor.EditorApplication.isPlaying = false;
	}

	public void start() {
		
	}
}
