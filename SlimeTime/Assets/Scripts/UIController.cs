using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private SettingsPopup settingsPopup;
    // Start is called before the first frame update
    void Start()
    {
        settingsPopup.Close();
        
    }

}
