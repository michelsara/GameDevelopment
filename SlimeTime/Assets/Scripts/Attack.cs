using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YounGenTech.HealthScript;

public class Attack : MonoBehaviour
{
    private int damage = 5;
    private string toHit = "";
    private Collider collisionAttack;

    public int Damage { get => damage; set => damage = value; }
    public string ToHit { get => toHit; set => toHit = value; }
    public Collider CollisionAttack { get => collisionAttack; set => collisionAttack = value; }

    public void launchAttack()
    {
        Collider[] colliders = Physics.OverlapBox(CollisionAttack.bounds.center, CollisionAttack.bounds.extents, CollisionAttack.transform.rotation, LayerMask.GetMask("Hitbox"));
        foreach (Collider c in colliders)
        {
            if (c.name.Equals(ToHit))
            {
                Health health = c.GetComponentInChildren<Health>();
                health.Damage(new HealthEvent(gameObject, Damage));
            }
        }
    }
}
