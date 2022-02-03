using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineLevelUIController : MonoBehaviour
{
    [SerializeField] private GameObject hud;
	private bool active = false;

    void Start() {
		//Disables cursor at starup
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Time.timeScale = 1;
	}

    void Update() {
        if(Input.GetKeyUp(KeyCode.H) || active && (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Tab)))
			toggle();
    }

	private void toggle() {
		active = !active;
		hud.SetActive(active);

		if(active) {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			Time.timeScale = 0;
		}
		else {
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			Time.timeScale = 1;
		}
	}
}
