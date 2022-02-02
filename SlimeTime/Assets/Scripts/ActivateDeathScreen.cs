using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDeathScreen : MonoBehaviour
{
    [SerializeField] private GameObject HUD;

    public void open()
    {
        HUD.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
}
