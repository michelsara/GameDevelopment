using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rune : MonoBehaviour
{
	[SerializeField] private Texture2D texture;

	private Renderer _renderer;

    void Awake()
    {
        _renderer = this.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
		Material material = _renderer.material;
		material.SetTexture("_MainTex", texture);

		_renderer.material = material;
    }
}
