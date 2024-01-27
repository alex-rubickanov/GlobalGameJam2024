using UnityEngine;

public class GarbageCan : StunObject
{
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
}