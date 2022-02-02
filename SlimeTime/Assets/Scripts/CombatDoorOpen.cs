using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDoorOpen : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject doors;

    void Start()
    {
        animator.ResetTrigger("Open");
    }

    public void open() {
		animator.SetTrigger("Open");
	}
}
