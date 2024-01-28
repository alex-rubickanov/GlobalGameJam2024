using System.Collections;
using UnityEngine;

public class Fridge : StunObject
{
    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.GetComponentInParent<NPlayerManager>() == null) return;
        
        playerInside = other.transform.GetComponentInParent<NPlayerManager>();
        
        Debug.Log(playerInside.GrabbablePlayer.wasThrown);
        if (playerInside && !isOccupied && playerInside.GrabbablePlayer.wasThrown)
        {
            TrapPlayer(playerInside);
            anim.PlayAnim(anim.PlayerEntered);
        }
    }
    
    protected override void TrapPlayer(NPlayerManager playerInside)
    {
        isOccupied = true;
        playerInside.Stun(this);
        playerInside.playerModel.gameObject.SetActive(false);
        
        playerInside.playerPawn.transform.position = playerPawnHolder.position;
        playerInside.playerPawn.transform.rotation = playerPawnHolder.rotation;     
    }
    
    public override void UnTrapPlayer()
    {
        playerInside.playerModel.gameObject.SetActive(true);
        playerInside = null;
        StartCoroutine(EnableCanWithDelay());
    }

    public FridgeAnimator anim => GetComponent<FridgeAnimator>();
}
