using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsSkip : MonoBehaviour
{
	[SerializeField] private SceneLoader sceneLoader;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Space) || Input.GetKey(KeyCode.Return))
			sceneLoader.loadScene();
    }
}
