using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunObject : MonoBehaviour
{
    [SerializeField] public Transform playerPawnHolder;
    [SerializeField] protected float sleepTime = 2.0f;
    
    protected NPlayerManager playerInside;
    protected bool isOccupied;
    
    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.GetComponentInParent<NPlayerManager>() == null) return;
        
        playerInside = other.transform.GetComponentInParent<NPlayerManager>();
        
        Debug.Log(playerInside.GrabbablePlayer.wasThrown);
        if (playerInside && !isOccupied && playerInside.GrabbablePlayer.wasThrown)
        {
            TrapPlayer(playerInside);
        }
    }

    protected virtual void TrapPlayer(NPlayerManager playerInside)
    {
        isOccupied = true;
        playerInside.Stun(this);

        
        playerInside.playerPawn.transform.position = playerPawnHolder.position;
        playerInside.playerPawn.transform.rotation = playerPawnHolder.rotation;   
    }

    public virtual void UnTrapPlayer()
    {
        playerInside = null;
        StartCoroutine(EnableCanWithDelay());
    }

    protected IEnumerator EnableCanWithDelay()
    {
        yield return new WaitForSeconds(sleepTime);
        isOccupied = false;
    }
}
