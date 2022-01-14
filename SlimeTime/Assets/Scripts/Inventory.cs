using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IInvetory
{
	[SerializeField] private GameObject hud;
	[SerializeField] private GameObject itemUI;
	private bool hudActive = false;
    private Dictionary<string, int> items;
	[SerializeField] private int maxCapability = 12;
	[SerializeField] private int perItemMaxCapability = 100;
	private InteractableInventory interestPointRequest = null;
	private bool usable = true;

	void Awake() {
		items = new Dictionary<string, int>();
	}

	void Start() {
		hud.transform.Find("Close Button").GetComponent<Button>().onClick.AddListener(() => closeHUD());
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
		if(Input.GetKeyDown(KeyCode.Tab) && usable) {
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

		if(usable) {
			//Places item images inside each inventory cell
			List<string> keyList = new List<string>(this.items.Keys);
			for(int i = 0; i < keyList.Count; i++) {

				string name = keyList[i];
				int quantity = items[name];

				GameObject slot = hud.transform.Find("board").Find("inventory_parchment ("+i+")").gameObject;
				Transform itemTransform = slot.transform.Find("item (" + i + ")");
				GameObject item;
				
				if(itemTransform == null) {

					//Image and Text
					Sprite sprite = Resources.Load<Sprite>("Images/" + name);
					item = Instantiate(itemUI, itemUI.transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
					item.transform.Find("image").GetComponent<Image>().sprite = sprite;
					item.transform.Find("name").GetComponent<Text>().text = name + " ("+quantity+")";

					//Eventual callback for interactableInventory insterest point
					if(interestPointRequest != null) {
						Button button = item.GetComponent<Button>();
						button.onClick.AddListener(() => interestPointRequest.choose(name));
					}

					//Location inside inventory cell
					item.transform.SetParent(slot.transform);
					item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
					item.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
					item.transform.name = "item ("+i+")";
				}

				//Eventual callback for interactableInventory insterest point
				else if(interestPointRequest != null) {
					item = itemTransform.gameObject;
					Button button = item.GetComponent<Button>();
					button.onClick.AddListener(() => interestPointRequest.choose(name));
				}
			}

			hudActive = true;
			hud.SetActive(true);

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			Time.timeScale = 0;
		}
	}

	public void closeHUD() {

		//Deletes every item from the ui
		Transform board = hud.transform.Find("board");
		foreach (Transform parchment in board)
			foreach(Transform item in parchment)
				Destroy(item.gameObject);

		hudActive = false;
		hud.SetActive(false);

		Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
		Time.timeScale = 1;

		interestPointRequest = null;
	}

	public void setInterestPointRequest(InteractableInventory interestPoint) {
		interestPointRequest = interestPoint;
	}

	public bool isUsable() {
		return usable;
	}

	public void setUsable(bool usable) {
		this.usable = usable;
	}
}
