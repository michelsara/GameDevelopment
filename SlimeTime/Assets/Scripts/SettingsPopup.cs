using System.Collections;
using UnityEngine;

public class SettingsPopup : MonoBehaviour
{
    // Start is called before the first frame update
    public void Open()
    {
        gameObject.SetActive(true);
        
    }

    // Update is called once per frame
    public void Close() {
        gameObject.SetActive(false);
    }
}
