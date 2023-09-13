using Assets.Script.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarZombieBehavior : MonoBehaviour
{
    [SerializeField] EnumZombieActionState zombieActionState = EnumZombieActionState.Patrol;
    //Patrol
    [Header("Patrulha")]
    [SerializeField] float waitingTime;
    float currentWaitingTime = 0;
    [SerializeField] Transform[] waypoint;
    int index;
    [Header("Perseguir jogador")]
    //ChasePlayer
    GameObject player;
    [SerializeField] float chaseStartDistance;
    [Header("Atacar Jogador")]
    [SerializeField] float attackDistance;



    NavMeshAgent navMeshAgent;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        index = Random.Range(0, waypoint.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null){ Debug.LogError("Player Nulo!"); return; }
        float _distance = Vector3.Distance(transform.position, player.transform.position);
        StateMannager(_distance);
        if (zombieActionState == EnumZombieActionState.Patrol)
        {
            navMeshAgent.speed = 1.0f;
            animator.SetFloat("Move", Mathf.Clamp(navMeshAgent.velocity.sqrMagnitude, 0, 0.5f), 0.06f, Time.deltaTime);
            animator.speed = 1.0f;
        }

        switch (zombieActionState)
        {
            case EnumZombieActionState.Patrol:
                PatrolArea();
                break;
            case EnumZombieActionState.ChasingPlayer:
                ChasePlayer();
                break;
            case EnumZombieActionState.AttackPlayer:
                AttackPlayer();
                break;
        }


    }


    void ChasePlayer()
    {

            animator.SetFloat("Move", 1.0f, 0.06f, Time.deltaTime);
            animator.speed = 1.5f;
            navMeshAgent.speed = 2.5f;
            navMeshAgent.SetDestination(player.transform.position);
        
    }
    void AttackPlayer()
    {
            animator.SetTrigger("Attack");           
            navMeshAgent.speed = 0.0f;
    }
    void PatrolArea()
    {
        currentWaitingTime += Time.deltaTime;
        if(currentWaitingTime>= waitingTime)
        {
            currentWaitingTime = 0;
            navMeshAgent.SetDestination(waypoint[index].position);
            index = index == waypoint.Length - 1 ? 0 : index + 1;
        }

        navMeshAgent.speed = 1.0f;
        animator.SetFloat("Move", Mathf.Clamp(navMeshAgent.velocity.sqrMagnitude, 0, 0.5f), 0.06f, Time.deltaTime);
        animator.speed = 1.0f;
    }
    void StateMannager(float distance)
    {
        if (distance > chaseStartDistance)
        {
            zombieActionState = EnumZombieActionState.Patrol;
        }else if (distance <= chaseStartDistance && distance > attackDistance)
        {
            zombieActionState = EnumZombieActionState.ChasingPlayer;
        }else if (distance<=attackDistance)
        {
            zombieActionState = EnumZombieActionState.AttackPlayer;
        }
    }
}


