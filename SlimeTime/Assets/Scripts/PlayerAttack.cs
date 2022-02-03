using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YounGenTech.HealthScript;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider collisionAttack;
    [SerializeField] private int damage = 5;
    private Attack attack;

    /// <summary>
    /// The layer mask to use when firing the raycast.
    /// </summary>
    public Animator Animator
    {
        get { return _animator; }
        set { _animator = value; }
    }
    /// <summary>
    /// The layer mask to use when firing the raycast.
    /// </summary>
    public Collider CollisionAttack
    {
        get { return collisionAttack; }
        set { collisionAttack = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        attack = GetComponent<Attack>();
        attack.ToHit = "Enemy";
        attack.CollisionAttack = collisionAttack;
        attack.Damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            attack.launchAttack();
            _animator.SetBool("attack", true);
        }
        //If the animation is running, change to stop it 
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
        {
            _animator.SetBool("attack", false);
        }
    }
}
