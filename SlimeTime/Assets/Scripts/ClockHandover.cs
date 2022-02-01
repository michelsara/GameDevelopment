using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockHandover : MonoBehaviour
{
	[SerializeField] private Camera mainCamera;
	[SerializeField] private Camera handoverCamera;
	[SerializeField] private AnimationCurve curve;
	[SerializeField] private GameObject start;
	[SerializeField] private Transform end;
	[SerializeField] private float speed;
	[SerializeField] private GameObject clock;
	private bool animate = false;
	private float time = 0.0f;

	void Start() {
		mainCamera.enabled = true;
		handoverCamera.enabled = false;
	}

	void OnTriggerEnter(Collider other) {
		//Enables animation
		animate = true;

		//Enables slime model
		start.SetActive(true);

		//Switch camera
		mainCamera.enabled = false;
		handoverCamera.enabled = true;

		//Face clock in the same direction as camera
		clock.transform.forward = handoverCamera.transform.forward;
	}

    void Update() {

		//End of animation
		if(time >= 1.0f) {
			animate = false;
			start.SetActive(false);
			mainCamera.enabled = true;
			handoverCamera.enabled = false;
		}

		if(!animate || time >= 1.0f)
			return;

		//animates clock (parabola movement)
		time += Time.deltaTime * speed;
		Vector3 pos = Vector3.Lerp(start.transform.position, end.position, time);
		pos.y += curve.Evaluate(time);
		transform.position = pos;
    }
}
