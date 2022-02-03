using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
	[SerializeField] private CutscenesController cutscenesController;

    void OnTriggerEnter(Collider other) {
		cutscenesController.open();
		this.gameObject.SetActive(false);
	}
}
