using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rune : MonoBehaviour
{
	[SerializeField] private Texture2D texture;

    void Awake()
    {
        Renderer renderer = this.GetComponent<Renderer>();
		Material material = renderer.material;
		material.SetTexture("_MainTex", texture);

		renderer.material = material;
    }
}
