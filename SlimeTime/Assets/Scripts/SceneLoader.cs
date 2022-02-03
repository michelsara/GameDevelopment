using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
	[SerializeField] private string sceneName;
	private AsyncOperation sceneToLoad;
	[SerializeField] private GameObject canvas;
	[SerializeField] private bool onTriggerEnter = false;

	void OnTriggerEnter(Collider other) {
		if(onTriggerEnter)
			loadScene();
	}

	public void loadScene() {
		sceneToLoad = SceneManager.LoadSceneAsync(sceneName);
		StartCoroutine(Loading());
	}

	private IEnumerator Loading() {
		canvas.SetActive(true);
		Image progressBarImage = canvas.transform.Find("ProgressBar").Find("ProgressBar").GetComponent<Image>();

		while(!sceneToLoad.isDone) {
			progressBarImage.fillAmount = sceneToLoad.progress;
			yield return null;
		}
	}
}
