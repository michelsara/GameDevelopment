using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarSoundManager : MonoBehaviour
{
	private AudioSource soundSource; 
 	[SerializeField] private AudioClip moveSound; 
 	[SerializeField] private AudioClip stopSound;

	void Start() {
		soundSource = this.GetComponent<AudioSource>();
	}

    private void OnTriggerEnter(Collider other)
    {
		soundSource.loop = true;
		soundSource.clip = moveSound;
        soundSource.Play();
    }
	
	private void OnTriggerExit(Collider other)
	{
		soundSource.loop = false;
        soundSource.PlayOneShot(stopSound);
    }
}
