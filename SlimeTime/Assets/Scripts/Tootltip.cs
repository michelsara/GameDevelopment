using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tootltip : MonoBehaviour
{

	[SerializeField] private int padding = 5;
	private Text tooltipText;
	private RectTransform rectTransform;

	void Awake() {
		rectTransform = GetComponent<RectTransform>();
		tooltipText = GetComponent<Text>();
	}

    public void ShowTooltip(string tooltipString) {
		gameObject.SetActive(true);
		tooltipText.text = tooltipString;

		Vector2 tooltipSize = new Vector2(tooltipText.preferredWidth, tooltipText.preferredHeight + padding);
		rectTransform.sizeDelta = tooltipSize;
	}

	public void HideTooltip() {
		gameObject.SetActive(false);
	}
}
