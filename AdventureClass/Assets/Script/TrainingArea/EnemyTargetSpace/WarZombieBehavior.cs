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
    [SerializeField] Transform[] waypoint;
    WaitForSeconds waitinOnWayPoint;
    int index;
    bool isAlive = true;
    [Header("Perseguir jogador")]
    //ChasePlayer
    GameObject player;
    [SerializeField] float chaseStartDistance;



    NavMeshAgent navMeshAgent;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(player.name);
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        waitinOnWayPoint = new WaitForSeconds(waitingTime);
        index = Random.Range(0, waypoint.Length);
        StartCoroutine(Patrol());
    }

    // Update is called once per frame
    void Update()
    {
        if (zombieActionState == EnumZombieActionState.Patrol)
        {
            navMeshAgent.speed = 1.0f;
            animator.SetFloat("Move", Mathf.Clamp(navMeshAgent.velocity.sqrMagnitude, 0, 0.5f), 0.06f, Time.deltaTime);
        }
        ChasePlayer();
    }

    IEnumerator Patrol()
    {
        while (isAlive)
        {
            if (zombieActionState == EnumZombieActionState.Patrol)
            {
                navMeshAgent.SetDestination(waypoint[index].position);
                index = index == waypoint.Length - 1 ? 0 : index + 1;
            }
            yield return waitinOnWayPoint;
        }
       

    }
    void ChasePlayer()
    {
        if (player == null) { return; }

        if(Vector3.Distance(transform.position, player.transform.position) <= chaseStartDistance)
        {
            zombieActionState = EnumZombieActionState.ChasingPlayer;
            animator.SetFloat("Move", 1.0f, 0.06f, Time.deltaTime);
            navMeshAgent.speed = 2.0f;
            navMeshAgent.SetDestination(player.transform.position);
        }
        else
        {
            zombieActionState = EnumZombieActionState.Patrol;
        }

    }
}


