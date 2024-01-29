using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    [SerializeField] public bool isHidingHere;
    [SerializeField] public bool hit;
    [SerializeField] public GameObject hidingObject;

    //For debugging
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float rad = .5f;

    private void Update()
    {
        ShowVirus();

        if (Physics.CheckSphere(transform.position, rad, playerLayer))
        {
            hit = true;
        }
        else
        {
            hit = false;
        }
    }

    //private void CalculateDistanceFromPlayers()
    //{
    //    float totalDistance = 0f;
    //    foreach (var player in players)
    //    {
    //        Transform playerPos = player.transform;
    //        float distanceToPlayer = Vector3.Distance(transform.position, playerPos.position);
    //        totalDistance += distanceToPlayer;
    //    }

    //    // Calculate average distance
    //    averageDistance = players.Length > 0 ? totalDistance / players.Length : 0f;
    //}

    /// <summary>
    /// When this hiding spot is being hit, it will release the bug
    /// </summary>
    private void ShowVirus()
    {
        if (hidingObject != null && hit)
        {
            hidingObject.GetComponent<AIMovement>().targetReached = false;
            // hidingObject.GetComponent<Hide>().SetActive3D(true);
            hidingObject.GetComponent<Hide>().isHiding = false;
            hidingObject.GetComponent<GoToHidingSpot>().RandomizeHidingSpot();
            isHidingHere = false;
            StartCoroutine(ResetHit());
        }
    }

    IEnumerator ResetHit()
    {
        yield return new WaitForSeconds(.5f);
        hidingObject = null;
        hit = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rad);
    }
}
