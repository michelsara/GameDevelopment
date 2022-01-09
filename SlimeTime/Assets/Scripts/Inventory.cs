using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	[SerializeField] private GameObject hud;
	[SerializeField] private GameObject itemUI;
	private bool hudActive = false;
    private Dictionary<string, int> items;
	[SerializeField] private int maxCapability = 12;
	[SerializeField] private int perItemMaxCapability = 100;

	void Awake() {
		items = new Dictionary<string, int>();
	}

    public void add(string name, int quantity) {
		
		//Item already in inventory and below the max capability
		if(items.ContainsKey(name)) {
		 	if(items[name] + quantity < perItemMaxCapability) 
				items[name] += quantity;
		}
		else {
			//Add new item in inventory if there is any empty slot
			if(items.Count < maxCapability)
				items.Add(name, quantity);
		}
	}

	public void remove(string name, int quantity) {

		if(items.ContainsKey(name)) {
			items[name] -= quantity;
			if(items[name] <= 0) 
				items.Remove(name);
		}
	}

	public int get(string name) {
		
		if(items.ContainsKey(name))
			return items[name];
		
		return 0;
	}

	void Update() {

		//Open closes the HUD via keyboard
		if(Input.GetKeyDown(KeyCode.Tab)) {
			hudActive = !hudActive;

			if(hudActive) openHUD();
			else closeHUD();
		}
		else if(Input.GetKeyDown(KeyCode.Escape) && hudActive) {
			closeHUD();
			hudActive = false;
		}
	}

	public void openHUD() {

		//Places item images inside each inventory cell
		List<string> keyList = new List<string>(this.items.Keys);
		for(int i = 0; i < keyList.Count; i++) {

			GameObject slot = hud.transform.Find("inventory_parchment ("+i+")").gameObject;
			if(slot.transform.Find("item (" + i + ")") == null) {
				string name = keyList[i];
				int quantity = items[name];

				Sprite sprite = Resources.Load<Sprite>("Images/" + name);

				GameObject item = Instantiate(itemUI, itemUI.transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
				item.transform.Find("image").GetComponent<Image>().sprite = sprite;
				item.transform.Find("name").GetComponent<Text>().text = name + " ("+quantity+")";

				item.transform.SetParent(slot.transform);
				item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
				item.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
				item.transform.name = "item ("+i+")";
			}
		}

		hudActive = true;
		hud.SetActive(true);
		Cursor.visible = true;
	}

	public void closeHUD() {
		hudActive = false;
		hud.SetActive(false);
		Cursor.visible = false;
	}
}
