using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHallPuzzleRune : Interactable
{
	[SerializeField] private bool enabled = false;
	[SerializeField] private Material enabledMaterial;
	[SerializeField] private Material disabledMaterial;
	[SerializeField] private GameObject[] neighboors = new GameObject[2];
	private Texture _texture;
	private GameObject _light;
	private Renderer _renderer;

	void Awake() {
		_light = transform.Find("Point Light").gameObject;
		_renderer = this.GetComponent<Renderer>();
		_texture = _renderer.material.GetTexture("_MainTex");
	}

	void Start() {
		_light.SetActive(enabled);
	}

	private void interact(bool interactNeighboors) {

		enabled = !enabled;

		Material usedMaterial = new Material(enabled ? enabledMaterial : disabledMaterial);
		usedMaterial.SetTexture("_MainTex", _texture);
		_renderer.material = usedMaterial;
		_light.SetActive(enabled);

		if(interactNeighboors) {
			foreach (GameObject neighboor in neighboors)
				neighboor.GetComponent<MainHallPuzzleRune>().interact(false);
		} 
		else {
			transform.parent.GetComponent<MainHallPuzzle>().checkIfCompleted();
		}
	}

	public override void interact() {
		interact(true);
	}

	public bool isEnabled() {
		return enabled;
	}
}
