using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private bool attack = false;
    public Animator _animator;
    public Collider collisionAttack;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            _animator.SetBool("attack", true);
            launchAttack(collisionAttack);
        }

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
            _animator.SetBool("attack", false);
    }

    private void launchAttack(Collider col)
    {
        Debug.Log("LOG");
        Collider[] colliders = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));
        foreach (Collider c in colliders)
            Debug.Log(c.name);
    }
}
