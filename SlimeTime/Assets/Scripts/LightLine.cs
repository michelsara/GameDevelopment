using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LightLine : MonoBehaviour
{
	public float pulseEffectCycleInSeconds = 1.0f;
	private float elapsedTime = 0.0f;
	private Material material;

	public bool isMirror = false;
	private bool powered = false;

	[Range(0.0f, 360.0f)]
	public float angleY = 0.0f;
	[Range(0.0f, 360.0f)]
	public float angleX = 0.0f;
    public float range = 1000;
    private LineRenderer line;

	[SerializeField]
	private GameObject endPoint;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
		material = line.GetComponent<Renderer>().material;

		powered = !isMirror;
    }

    void Update()
    {
        RaycastHit raycastHit = new RaycastHit();
        bool hit = false;
		
		if(powered) {
			hit = Physics.Raycast(
				transform.position, // transform.position + (transform.forward * (float)offset) can be used for casting not from center.
				Quaternion.AngleAxis(angleY, Vector3.up) * Quaternion.AngleAxis(angleX, Vector3.right) * Vector3.forward,
				out raycastHit, 
				range,
				~(1 << LayerMask.NameToLayer("NoPhysicsRaycast"))
			);

            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position + (transform.forward * range)); // transform.position + (transform.forward * ((float)offset + range)) can be used for casting not from center.
		
			if(isMirror)
				powered = false;
		}

		if (hit) {
            Collider collider = raycastHit.collider;

            line.SetPosition(0, transform.position);
            line.SetPosition(1, raycastHit.point);

            if (collider.gameObject.tag == "CrystalMirror")
				collider.gameObject.GetComponentInChildren<LightLine>().powered = true;
			
			else if (collider.gameObject.tag == "EndPoint")
				collider.gameObject.GetComponentInParent<LightLineEndPoint>().hit();

        }
		else {
			line.SetPosition(0, Vector3.zero);
			line.SetPosition(1, Vector3.zero);
		}

		//Glowing cycle
		elapsedTime += Time.deltaTime;
		Color color = material.color;

		if(elapsedTime <= pulseEffectCycleInSeconds / 2)
			color.a = 1f - elapsedTime / pulseEffectCycleInSeconds;
		else
			color.a = elapsedTime / pulseEffectCycleInSeconds;

		material.SetColor("_Color", color);

		if(elapsedTime >= pulseEffectCycleInSeconds) elapsedTime = 0f;
		line.GetComponent<Renderer>().material = material;
    }
}
