using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableCrafting : InteractablePoint, IInvetory
{
    private Dictionary<string, List<string>> recipes;
	[SerializeField] private GameObject hud;
	private bool hudActive = false;

	void Awake() {
		base.Awake();
		recipes = new Dictionary<string, List<string>>();
		recipes.Add("Acid", new List<string> {"Oil", "Salt", "Toxic Roots"});
		recipes.Add("Torch", new List<string> {"Wood", "Cloth", "Oil"});
		recipes.Add("Explosive", new List<string> {"Sulfur", "Acid", "Gunpowder"});
	}

	void Start() {
		hud.transform.Find("Close Button").GetComponent<Button>().onClick.AddListener(() => closeHUD());
	}

	void Update() {
		//closes the HUD via keyboard
		if(Input.GetKeyDown(KeyCode.Escape) && hudActive) {
			closeHUD();
		}
	}

	public override void interact() {

		//Shows crafting HUD
		openHUD();

		base.interact();
	}

	public void openHUD() {
		hudActive = true;
		hud.SetActive(true);

		Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
		Time.timeScale = 0;

		inventory.setUsable(false);

		//Updates resources quantity
		updateHUD();
	}

	public void closeHUD() {
		hudActive = false;
		hud.SetActive(false);

		Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
		Time.timeScale = 1;

		inventory.setUsable(true);
	}

	private void updateHUD() {
		List<string> keyList = new List<string>(this.recipes.Keys);
		Transform board = hud.transform.Find("board");
		for(int i = 0; i < keyList.Count; i++) {

			string recipe = keyList[i];
			List<string> ingredients = recipes[recipe];

			for(int j = 0; j < ingredients.Count; j++) {
				string ingredient = ingredients[j];
				Text quantity = board.Find(recipe).Find(ingredient).GetComponent<Text>();
				quantity.text = inventory.get(ingredient) + "";
			}

			//clickable area
			Button button = board.Find(recipe).GetComponent<Button>();
			button.onClick.AddListener(() => craft(recipe));
		}
	}

	public void craft(string item) {
		
		//Item name check
		if(recipes.ContainsKey(item)) {

			List<string> resources = recipes[item];

			//Check if that selected item is craftable
			bool craftable = true;
			foreach(string resource in resources) {
				craftable = craftable && (inventory.get(resource) > 0);
			}

			//Remove components from the inventory and add the crafted one
			if(craftable) {
				foreach(string resource in resources)
					inventory.remove(resource, 1);

				inventory.add(item, 1);
				updateHUD();
				playSound();
			}
		}
	}
}
