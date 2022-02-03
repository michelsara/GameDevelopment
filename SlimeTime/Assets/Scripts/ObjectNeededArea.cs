using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectNeededArea : MonoBehaviour
{
	[SerializeField] private string objectNeeded;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject relatedObject;
	private Inventory inventory;

	void Awake() {
		inventory = player.GetComponent<Inventory>();
	}

    // Update is called once per frame
    void Update() {
		if(inventory.get(objectNeeded) > 0) {
			relatedObject.SetActive(true);
			Destroy(this.gameObject);
		}
    }
}
