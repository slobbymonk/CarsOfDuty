using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;

    public GameObject player;

    public LayerMask whatsGround, whatsPlayer;

    //Roaming
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    private Vector3 guntip;
    public Transform guntipT;
    public ParticleSystem muzzle;
    public LayerMask layer;
    public float dist;
    public AudioSource shot;


    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        guntip = guntipT.transform.position;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatsPlayer);

        if(!playerInAttackRange && !playerInSightRange)
        {
            Roaming();
        }
        if (!playerInAttackRange && playerInSightRange)
        {
            ChasePlayer();
        }
        if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }
    }

    void Roaming()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if(distanceToWalkPoint.magnitude < 1f)
            {
                walkPointSet = false;
            }
        }
    }
    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomx = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomx, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatsGround))
        {
            walkPointSet = true;
        }
    }
    void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }
    void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        gameObject.transform.LookAt(player.transform);

        if (!alreadyAttacked)
        {
            Shooting();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    void Shooting()
    {
        shot.Play();
        muzzle.Play();
        RaycastHit hit;
        if (Physics.Raycast(guntip, guntipT.forward, out hit, dist))
        {
            if(hit.transform.CompareTag("Player"))
            {
                var playerHealth = hit.transform.GetComponent<Health>();
                var enemyDamage = gameObject.GetComponent<Health>();
                playerHealth.health -= enemyDamage.damage;
                if (playerHealth.health <= 0)
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

}
