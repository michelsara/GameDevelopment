using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetection : MonoBehaviour {
    private ParticleSystem part;
    private Transform parent;
    private Transform hitParent;
    private GameObject[] steps;
    void Start() {
        part = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other) {  
        if (other.name != "Player" && other.tag != part.gameObject.tag && part.gameObject.layer == 10 && other.layer == 3 && !(other.tag == "Untagged") && !(part.gameObject.tag == "Untagged")){
            Debug.Log ("HIT DIFFERENT");
            Debug.Log(other.tag);
            findParent(other.tag);
            
        }
    }


    void findParent(string tag) {
        steps = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject step in steps) {
            if(step.name == "CyanLine" && tag == "stepCyan" && !drawLine.completedCyan) {
                parent = step.transform;
                drawLine.completedCyan = false;
                break;
            } else if (step.name == "BlueLine" && tag == "stepBlue" && !drawLine.completedBlue) {
                parent = step.transform;
                drawLine.completedBlue = false;
                break;
            } else if (step.name == "GreenLine" && tag == "stepGreen" && !drawLine.completedGreen) {
                parent = step.transform;
                drawLine.completedGreen = false;
                break;
            } else if (step.name == "RedLine" && tag == "stepRed" && !drawLine.completedRed) {
                parent = step.transform;
                drawLine.completedRed = false;
                break;
            } else if (step.name == "MagentaLine" && tag == "stepMagenta" && !drawLine.completedMagenta) {
                parent = step.transform;
                drawLine.completedMagenta = false;
                break;
            } else if (step.name == "YellowLine" && tag == "stepYellow" && !drawLine.completedYellow) {
                parent = step.transform;
                drawLine.completedYellow = false;
                break;
            }
        }
        drawLine.destroyLine(parent);
        
    }
}
