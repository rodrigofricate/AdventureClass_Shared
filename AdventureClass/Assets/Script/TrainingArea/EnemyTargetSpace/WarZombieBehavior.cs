using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarZombieBehavior : MonoBehaviour
{
    [SerializeField] float waitingTime;
    [SerializeField] Transform[] waypoint;
    WaitForSeconds waitinOnWayPoint;
    int index;
    bool isOnPatrol = true;
    NavMeshAgent navMeshAgent;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
         waitinOnWayPoint = new WaitForSeconds(waitingTime);
        index = Random.Range(0, waypoint.Length);
        StartCoroutine(Patrol());
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Move", navMeshAgent.velocity.sqrMagnitude, 0.06f, Time.deltaTime);
    }

    IEnumerator Patrol()
    {
        while (isOnPatrol)
        {
            navMeshAgent.SetDestination(waypoint[index].position);
            index = index == waypoint.Length - 1 ? 0 : index + 1;
            yield return waitinOnWayPoint;
          
        }
    }

}
