using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLineEndPoint : MonoBehaviour
{

	private bool powered = false;
	private bool prevPowered = false;

	[SerializeField] private Transform door;
	[SerializeField] private Vector3 doorOpenPosition;
	[SerializeField] private Vector3 doorClosePosition;
	
	[SerializeField] private float openCloseTimeInSeconds = 1.0f;
	private float elapsedTime;

	void Awake() {
		elapsedTime = openCloseTimeInSeconds;
	}

    // Update is called once per frame
    void Update()
    {
		float lerpTime;

		//light and emission
		if(powered) {
			this.GetComponentInChildren<Renderer>().material.SetColor("_EMISSION", new Color(80, 154, 255));
			this.GetComponentInChildren<Renderer>().material.EnableKeyword("_EMISSION");
			lerpTime = 1f - elapsedTime / openCloseTimeInSeconds;
		}
		else {
			this.GetComponentInChildren<Renderer>().material.DisableKeyword("_EMISSION");
			lerpTime = elapsedTime / openCloseTimeInSeconds;
		}

		// open/close door animation
		if(elapsedTime < openCloseTimeInSeconds) {
			elapsedTime += Time.deltaTime;
			Vector3 newPosition = Vector3.Lerp(doorOpenPosition, doorClosePosition, lerpTime);
			door.localPosition = newPosition;
		}
		if(elapsedTime >= openCloseTimeInSeconds && powered != prevPowered) {
			elapsedTime = 0f;
		}
		
		//illumination
		this.GetComponentInChildren<Light>().enabled = powered;

		prevPowered = powered;

		//de-powers in case ray stops calling hit() method
		powered = false;
    }

	public void hit() {
		powered = true;
	}
}
