using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetection_demo : MonoBehaviour {
    private ParticleSystem part;
    private Transform parent;
    private Transform hitParent;
    private GameObject[] steps;
    void Start() {
        part = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other) {
        if (other.tag != part.gameObject.tag && part.gameObject.layer == LayerMask.NameToLayer("Line") && other.layer == LayerMask.NameToLayer("Player") && !(other.tag == "Untagged") && !(part.gameObject.tag == "Untagged")){
            findParent(other.tag);
        }
    }


    void findParent(string tag) {
        steps = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject step in steps) {
            if (step.name == "GreenLine" && tag == "stepGreen" && !drawLine_demo.completedGreen) {
                parent = step.transform;
                drawLine_demo.completedGreen = false;
                break;
            } else if (step.name == "YellowLine" && tag == "stepYellow" && !drawLine_demo.completedYellow) {
                parent = step.transform;
                drawLine_demo.completedYellow = false;
                break;
            }
        }
        drawLine_demo.destroyLine(parent);
        
    }
}
