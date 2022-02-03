using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float lifeTime = 5.0f;
    private Attack attack;
    [SerializeField] private int damage = 1;

    private Rigidbody _rigidbody;

    public Attack Attack { get => attack; set => attack = value; }
    public float Speed { get => speed; set => speed = value; }
    public Vector3 Direction { get => direction; set => direction = value; }

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // start with explosive velocity, also called impulse
        _rigidbody.AddForce(Direction * Speed, ForceMode.VelocityChange);
        attack = GetComponent<Attack>();
        attack.CollisionAttack = gameObject.GetComponent<Collider>();
        attack.ToHit = "Enemy";
        attack.Damage = damage;
    }

    void Update()
    {
        // handle life of the shot
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
        {
            // Destroy whole gameobject, if "this" is being used instead of gameObject -> then only this script (MonoBehaviour) will be destroyed
            StartCoroutine(DestroyRoutine(0.1f));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //OnCollision attack
        Attack.launchAttack();
        StartCoroutine(DestroyRoutine(0.1f));
    }

    private IEnumerator DestroyRoutine(float time)
    {
        // handle how the shot is destroyed (briefly alive to show particles (from particle lesson))
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.detectCollisions = false;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        _rigidbody.isKinematic = true;
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
