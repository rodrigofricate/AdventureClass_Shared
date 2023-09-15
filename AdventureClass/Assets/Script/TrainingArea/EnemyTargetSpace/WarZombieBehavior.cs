using Assets.Script.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class WarZombieBehavior : MonoBehaviour
{
    [SerializeField] EnumZombieActionState zombieActionState = EnumZombieActionState.Patrol;
    [SerializeField] float maximumHealthPoints;
    float currentHealthPoint;
    [SerializeField] GameObject deadBody;
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
    [Header("Body Parts")]
    [SerializeField] Transform headOfModel;
    [SerializeField] Transform headCollider;


    NavMeshAgent navMeshAgent;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(10, 11);

        currentHealthPoint = maximumHealthPoints;
        index = Random.Range(0, waypoint.Length);

        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        BodyColliderUpdater();

        if (player == null) { Debug.LogError("Player Nulo!"); return; }
        float _distance = Vector3.Distance(transform.position, player.transform.position);
       
        StateMannager(_distance);
        switch (zombieActionState)
        {
            case EnumZombieActionState.Death:
                Death();
                break;
            case EnumZombieActionState.Patrol:
                PatrolArea();
                break;
            case EnumZombieActionState.ChasingPlayer:
                ChasePlayer();
                break;
            case EnumZombieActionState.AngryChasingPlayer:
                ChasePlayer();
                break;
            case EnumZombieActionState.AttackPlayer:
                AttackPlayer();
                break;
        }

    }

    void StateMannager(float distance)
    {
        if (currentHealthPoint <= 0)
        {
            zombieActionState = EnumZombieActionState.Death;
            return;
        }

        if ( currentHealthPoint < maximumHealthPoints || zombieActionState == EnumZombieActionState.AngryChasingPlayer)
        {
            zombieActionState = EnumZombieActionState.AngryChasingPlayer;
        }
        else if (distance > chaseStartDistance)
        {
            zombieActionState = EnumZombieActionState.Patrol;
        }
        else if (distance <= chaseStartDistance && distance > attackDistance)
        {
            zombieActionState = EnumZombieActionState.ChasingPlayer;
        }

        if (distance <= attackDistance)
        {
            zombieActionState = EnumZombieActionState.AttackPlayer;
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

        Vector3 _direction = player.transform.position - transform.position;
        Vector3 _rotationTowards = Vector3.RotateTowards(transform.forward, _direction, 1.0f * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(_rotationTowards);

        animator.SetTrigger("Attack");
        navMeshAgent.speed = 0.0f;
    }
    void PatrolArea()
    {
        currentWaitingTime += Time.deltaTime;
        if (currentWaitingTime >= waitingTime)
        {
            currentWaitingTime = 0;
            navMeshAgent.SetDestination(waypoint[index].position);
            index = index == waypoint.Length - 1 ? 0 : index + 1;
        }

        navMeshAgent.speed = 1.0f;
        animator.SetFloat("Move", Mathf.Clamp(navMeshAgent.velocity.sqrMagnitude, 0, 0.5f), 0.06f, Time.deltaTime);
        animator.speed = 1.0f;
    }
   
    void BodyColliderUpdater()
    {
        headCollider.transform.position = headOfModel.transform.position;

    }
    void Death()
    {
        GameObject _dead = Instantiate(deadBody, transform.position, Quaternion.identity);        
        Destroy(this.gameObject);
    }
    //Gizmo
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseStartDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
    //Public
    public void TakeDamange(float damange) {currentHealthPoint -= damange;}
    public void Alert()
    {
        zombieActionState = EnumZombieActionState.AngryChasingPlayer;

    }
}


