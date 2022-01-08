using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHallPuzzle : MonoBehaviour
{
	[SerializeField] private GameObject goalObject;

    public void checkIfCompleted() {

		bool isCompleted = true;
		MainHallPuzzleRune[] allChildren = GetComponentsInChildren<MainHallPuzzleRune>();
		foreach (MainHallPuzzleRune child in allChildren) {
			isCompleted = isCompleted && child.isEnabled();
		}
		
		Transform[] goalChildren = goalObject.GetComponentsInChildren<Transform>(true);
		if(isCompleted) {
			foreach (Transform child in goalChildren) {
				if(child.gameObject.name == "Chest_Closed (1)") child.gameObject.SetActive(false);
				else if(child.gameObject.name == "Chest_Open (3)") child.gameObject.SetActive(true);
				else if(child.gameObject.name == "InterestPoint_ParticleSystem") child.gameObject.SetActive(false);
				else if(child.gameObject.name == "InterestPoint_ParticleSystem (1)") child.gameObject.SetActive(true);
			}
		}
	}
}
