using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCrafting : InteractablePoint
{
    private Dictionary<string, List<string>> recipes;
	[SerializeField] private GameObject hud;

	void Awake() {
		recipes.Add("Acid", new List<string> {"Oil", "Salt", "Toxic Roots"});
		recipes.Add("Torch", new List<string> {"Wood", "Cloth", "Oil"});
		recipes.Add("Explosive", new List<string> {"Sulfur", "Acid", "Gunpowder"});
	}

	public override void interact() {

		//Shows crafting HUD

		base.interact();
	}

	public void craft(string item) {
		
		//Item name check
		if(recipes.ContainsKey(item)) {

			List<string> resources = recipes[item];

			//Check if that selected item is craftable
			bool craftable = false;
			foreach(string resource in resources)
				craftable = craftable && (inventory.get(resource) > 0);

			//Remove components from the inventory and add the crafted one
			if(craftable) {
				foreach(string resource in resources)
					inventory.remove(resource, 1);

				inventory.add(item, 1);
			}
		}
	}
}
