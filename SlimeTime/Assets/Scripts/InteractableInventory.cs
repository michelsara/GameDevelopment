using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableInventory : InteractablePoint
{
    [SerializeField] private string requestedItem;
	[SerializeField] private int quantity;

	public override void interact() {

		//Shows inventory HUD
		inventory.openHUD();

		base.interact();
	}

	public void choose(string item) {
		//Check if that selected item is compatible with the requested Item and remove it from the inventory
		if(item == requestedItem)
			inventory.remove(requestedItem, quantity);
	}
}
