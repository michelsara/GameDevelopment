using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerParticles : MonoBehaviour {
     void OnParticleCollision(GameObject other) {
        Debug.Log("trigger");
        Debug.Log(other.name);
    }
}
