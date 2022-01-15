using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePoint : Interactable
{
	[SerializeField] private bool destroyParticle = false;
	[SerializeField] private bool destroyRelated = false;
	[SerializeField] protected GameObject relatedObject;
	[SerializeField] private GameObject player;
	[SerializeField] private string tooltipString;
	protected Inventory inventory;

	public void Awake() {
		inventory = player.GetComponent<Inventory>();
	}

	public override void interact() {
		
		//Remove this interactable point
		if(destroyParticle) Destroy(this.gameObject);

		//Remove the related object from scene
		if(destroyRelated) Destroy(relatedObject);

		//Play sound if immediatly requested
		if(playSoundImmediatly) playSound();
	}

	public override void tooltip(Tootltip tooltip) {
		tooltip.ShowTooltip(tooltipString);
	}
	
}
