using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
	/**
	*	AUDIO
	*/
	[SerializeField] private GameObject audioContext;
	private MenuAudioManager audioManager;
	private bool isAudioActive = true;
	[SerializeField] private Sprite audioEnabled;
	[SerializeField] private Sprite audioDisabled;
	AudioSource source;

	[SerializeField] private GameObject progressBar;
	private AsyncOperation sceneLoad;

	void Awake() {
		audioManager = audioContext.GetComponent<MenuAudioManager>();
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
		// Runtime code here
		#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
		#endif
		// Runtime code here
		//UnityEditor.EditorApplication.isPlaying = false;
	}

	public void startGame() {
		sceneLoad = SceneManager.LoadSceneAsync("IntroCutscene");
		StartCoroutine(Loading());
	}

	private IEnumerator Loading() {
		progressBar.SetActive(true);

		Image progressBarImage = progressBar.transform.Find("ProgressBar").GetComponent<Image>();
		while(!sceneLoad.isDone) {
			progressBarImage.fillAmount = sceneLoad.progress;
			yield return null;
		}
	}
}
