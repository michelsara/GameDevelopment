using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : InteractablePoint
{
    [SerializeField] private string gatherableItem;
	[SerializeField] private int quantity;

	public override void interact() {

		//Add item(s) to the player inventory
		inventory.add(gatherableItem, quantity);

		base.interact();
	}
}
