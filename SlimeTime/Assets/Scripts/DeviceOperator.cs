using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceOperator : MonoBehaviour {
    public float maxRange = 1.5f;
	[SerializeField] private Camera camera;

	void Update() {
		if(Input.GetKeyDown(KeyCode.E)) {
			
        	RaycastHit raycastHit = new RaycastHit();
			bool hit = Physics.Raycast(
				camera.transform.position,
				camera.transform.forward,
				out raycastHit,
				maxRange
			);

			if(hit) {
				GameObject obj = raycastHit.collider.gameObject;
				if (obj.tag == "Interactable") obj.GetComponent<Interactable>().interact();
			}
		}
	}
}
