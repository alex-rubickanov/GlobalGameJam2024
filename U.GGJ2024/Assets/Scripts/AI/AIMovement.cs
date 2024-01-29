using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    NavMeshAgent agent;
    AIAnimation anim;
    public Transform target;
    public bool targetReached;
    public float maxTimeToRoll = 5f;
    float maxTime;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<AIAnimation>();
        maxTime = maxTimeToRoll;
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        if (target != null)
        {
            float distanceFromTarget = Vector3.Distance(transform.position, target.position);
            if (distanceFromTarget > agent.stoppingDistance)
            {
                RandomRoll();
                anim.SetAnim(anim.AI_RUN);
                targetReached = false;
                agent.SetDestination(target.position);
            }
            else
            {
                anim.SetAnim(anim.AI_HIDE);
                targetReached = true;
                maxTimeToRoll = maxTime;
            }
        }
    }

    public bool TargetIsAvailable => target != null;
    public void SetDestination(Transform target)
    {
        this.target = target;
    }

    public void ResetNavPath()
    {
        agent.ResetPath();
    }

    //OPTION
    public void RandomRoll()
    {
        if(maxTimeToRoll > 0)
        {
            maxTimeToRoll -= Time.deltaTime;
        }
        else
        {
            int random = Random.Range(0, 5);
            if (random > 2)
            {
                anim.SetAnim(anim.AI_ROLL);
            }
            maxTimeToRoll = maxTime;
        }
    }
}
