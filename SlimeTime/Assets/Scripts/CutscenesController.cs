using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutscenesController : MonoBehaviour
{
	[SerializeField] private Canvas canvas;
	[SerializeField] private SceneLoader sceneLoader;
    private int index = 0;

	void Update() {

		if(Input.GetKeyUp(KeyCode.Space))
			next();
	}

	public void open() {
		canvas.gameObject.SetActive(true);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		Time.timeScale = 0;
	}

	public void close() {
		canvas.gameObject.SetActive(false);
		Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
		Time.timeScale = 1;
	}

	public void next() {
		canvas.transform.GetChild(index).gameObject.SetActive(false);
		index++;

		Transform child = canvas.transform.GetChild(index);
		if(child != null && child.GetComponent<Button>() == null)
			child.gameObject.SetActive(true);
		else if(sceneLoader != null)
			sceneLoader.loadScene();
		else
			close();
	}
}
