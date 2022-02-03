using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDeathScreen : MonoBehaviour
{
    [SerializeField] private GameObject HUD;

    public void open()
    {
        StartCoroutine(Execute());
    }

    private IEnumerator Execute()
    {
        yield return new WaitForSeconds(2);

        HUD.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
}
