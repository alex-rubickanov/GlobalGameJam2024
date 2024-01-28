using UnityEngine;

public class GarbageCan : StunObject
{
    [Range(-9, 9)] 
    [SerializeField] private int points;

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponentInParent<NPlayerManager>() == null) return;

        playerInside = other.transform.GetComponentInParent<NPlayerManager>();

        Debug.Log(playerInside.GrabbablePlayer.wasThrown);
        if (playerInside && !isOccupied && playerInside.GrabbablePlayer.wasThrown)
        {
            TrapPlayer(playerInside);
            UpdatePlayerPoints(playerInside);
        }
    }

    private void UpdatePlayerPoints(NPlayerManager trappedPlayer)
    {
        PointsGainUIManager.instance.ShowUIPoints(
            trappedPlayer.GrabbablePlayer.lastGrabbedByPlayer.playerManager.playerPawn, points);
    }
}