using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceOperator : MonoBehaviour {
    public float maxRange;
	[SerializeField] private Camera _camera;
	[SerializeField] private GameObject tooltip;
	[SerializeField] private int[] particleColor = {88, 217, 243};
	private ParticleSystem prevParticle;

	void Update() {
		RaycastHit raycastHit;
		bool hit;

		if(Input.GetKeyDown(KeyCode.E)) {
			
        	raycastHit = new RaycastHit();
			hit = Physics.Raycast(
				_camera.transform.position,
				_camera.transform.forward,
				out raycastHit,
				maxRange
			);

			if(hit) {
				GameObject obj = raycastHit.collider.gameObject;
				if (obj.tag == "Interactable") {

					//interact with object
					Interactable[] interactables = obj.GetComponents<Interactable>();
					foreach(Interactable i in interactables)
						i.interact();
				}
			}
		}


		//reset previous frame particle system color
		if(prevParticle != null) {
			ParticleSystem.MainModule psm = prevParticle.main;
			psm.startColor = new Color(1.0f, 1.0f, 1.0f);
		}

		//Search for an interaclable object
		bool interactableHit = false;
		raycastHit = new RaycastHit();
		hit = Physics.Raycast(
			_camera.transform.position,
			_camera.transform.forward,
			out raycastHit,
			maxRange
		);
		if(hit) {
			GameObject obj = raycastHit.collider.gameObject;
			if (obj.tag == "Interactable") {
				interactableHit = true;

				//updates the tooltip
				obj.GetComponent<Interactable>().tooltip(tooltip.GetComponent<Tootltip>());

				//change particle system color
				prevParticle = obj.GetComponent<ParticleSystem>();
				if(prevParticle != null) {
					ParticleSystem.MainModule psm = prevParticle.main;
					psm.startColor = new Color(particleColor[0]/255.0f, particleColor[1]/255.0f, particleColor[2]/255.0f);
				}
			}
		}
		
		if(!interactableHit) {
			//Hides out the tooltip
			tooltip.GetComponent<Tootltip>().HideTooltip();
		}
	}
}
