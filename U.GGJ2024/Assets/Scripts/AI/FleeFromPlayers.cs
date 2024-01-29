using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FleeFromPlayers : MonoBehaviour
{
    [SerializeField] float sphereRad = 3f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Collider[] players;
    [SerializeField] float moveDistance;

    Vector3 direction;
    Vector3 velocity;
    Vector3 fleeDestination;

    bool randomized;
    public AIMovement aiMovement => GetComponent<AIMovement>();
    public NavMeshAgent agent => GetComponent<NavMeshAgent>();
    public Hide hide => GetComponent<Hide>();
    public GoToHidingSpot goToHidingSpot => GetComponent<GoToHidingSpot>();

    void Update()
    {
        players = Physics.OverlapSphere(transform.position, sphereRad, playerLayer);
        if (hide.IsHiding == false && players.Length > 0)
        {
            FleeMovement(transform, players, moveDistance);
            goToHidingSpot.RandomizeHidingSpot();
        }
        else if (players.Length <= 0) { aiMovement.enabled = true; }
    }

    void FleeMovement(Transform currentPos, Collider[] players, float moveDistance)
    {
        Vector3 cumulativeFleeDirection = new Vector3();

        foreach (Collider obstacle in players)
        {
            Vector3 directionToObstacle = currentPos.position - obstacle.transform.position;
            directionToObstacle.Normalize();
            cumulativeFleeDirection += directionToObstacle;
        }
        cumulativeFleeDirection.Normalize();

        Vector3 fleeDestination = currentPos.position + cumulativeFleeDirection * moveDistance;
        agent.SetDestination(fleeDestination);
        aiMovement.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereRad);
    }
}
