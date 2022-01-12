using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	[SerializeField] protected GameObject audioSource;
	[SerializeField] private AudioClip sound;
	[SerializeField] protected bool playSoundImmediatly;

    public abstract void interact();
    public void playSound() {
		if(sound != null) {
			audioSource.GetComponent<AudioSource>().clip = sound;
			audioSource.GetComponent<AudioSource>().Play();
		}
	}

	public abstract void tooltip(Tootltip tooltip);
}
