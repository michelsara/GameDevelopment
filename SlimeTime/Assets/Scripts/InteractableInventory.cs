using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableInventory : InteractablePoint
{
    [SerializeField] private string requestedItem;
	[SerializeField] private int quantity;
	[SerializeField] private bool door;

	public override void interact() {

		//Shows inventory HUD
		inventory.setInterestPointRequest(this);
		inventory.openHUD();

		base.interact();
	}

	public void choose(string item) {
		//Check if that selected item is compatible with the requested Item and remove it from the inventory
		if(item == requestedItem) {
			inventory.remove(requestedItem, quantity);
			inventory.closeHUD();

			//Enables the door
			if(door) {
				relatedObject.GetComponent<HingeJoint>().useSpring = false;
				relatedObject.transform.parent.GetComponent<BoxCollider>().enabled = false;
			}
			//Wall explosion
			else {
				GameObject explosion = relatedObject.transform.Find("Explosion").gameObject;
				ParticleSystem particleSystem = explosion.GetComponent<ParticleSystem>();
				explosion.SetActive(true);
				relatedObject.GetComponent<MeshRenderer>().enabled = false;
				Destroy(relatedObject, particleSystem.main.duration);
			}

			playSound();
			
			Destroy(this.gameObject);
		}
	}
}
