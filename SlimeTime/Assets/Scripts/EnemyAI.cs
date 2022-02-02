using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using YounGenTech.HealthScript;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform[] movingPoints;
    [SerializeField] private Transform target;
    [SerializeField] private Collider weapon;
    [SerializeField] private float triggerDistance = 8.0f;
    private Health enemyHealth;
    private Attack attack;
    private float distance;
    private int destPoint = 0;
    private NavMeshAgent agent;

    [SerializeField] private Animator _animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = GetComponentInChildren<Health>();
        attack = GetComponent<Attack>();
        attack.CollisionAttack = weapon;
        attack.ToHit = "Player";
    }

    // Update is called once per frame
    void Update()
    {
        //By default is walking
        _animator.SetBool("walk", true);
        distance = Vector3.Distance(transform.position, target.position);
        if (distance <= triggerDistance)
        {
            //If chasing the enemy don't inspect weapon
            _animator.SetBool("chasing", true);
            //If enough distant chase the player
            huntPlayer(target);
            if (distance <= agent.stoppingDistance)
            {
                //If enough close, turn to face the player
                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Strike_2")) {
                    FaceTarget();
                }
                //if it is not, attack
                _animator.SetBool("Attack", true);
                _animator.SetBool("chasing", false);
                _animator.SetBool("walk", false);
                //If the animation says that he is attacking, attack
                if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Strike_1") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Strike_2"))
                {
                    attack.launchAttack();
                }
            }
            else
            {
                _animator.SetBool("Attack", false);
                _animator.SetBool("chasing", true);
                _animator.SetBool("walk", true);
            }
        }
        else
        {
            //If can't hear the player patrol
            _animator.SetBool("Attack", false);
            _animator.SetBool("chasing", false);
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Strike_1") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Strike_2") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Weapon_Inspection") &&
                !agent.pathPending && agent.remainingDistance < 2.5f)
            {
                goToPoint(movingPoints[destPoint++]);
                destPoint %= movingPoints.Length;
            }
        }
    }

    void goToPoint(Transform point)
    {
        agent.destination = point.position;
    }

    void huntPlayer(Transform player)
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Strike_1") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Strike_2") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Weapon_Inspection"))
        {
            agent.destination = player.position;
        }
        else
        {
            agent.destination = transform.position;
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

        private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, triggerDistance);
    }
}
