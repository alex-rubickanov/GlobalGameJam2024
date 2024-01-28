using UnityEngine;

public class Alcohol : GrabbableObject
{
    private NPlayerManager hittedPlayer;
    [Range(-9, 9)]
    [SerializeField] private int points;
    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            wasThrown = false;
        } else if(other.gameObject.layer == LayerMask.NameToLayer("Player") && wasThrown)
        {
            hittedPlayer = other.gameObject.GetComponentInParent<NPlayerManager>();
            if (hittedPlayer.PlayerGrabbing == lastGrabbedByPlayer)
            {
                hittedPlayer = null;
                return;
            }
            UpdatePlayerPoints();
            MakeDrunk();
        }
    }

    private void MakeDrunk()
    {
        //maybe particle effect
        if (hittedPlayer.PlayerMovement.isDrunk)
        {
            hittedPlayer.RestartDrunk();
        }
        else
        {
            hittedPlayer.DrunkOn();
        }
        Destroy(gameObject);
    }
    
    private void UpdatePlayerPoints()
    {
        PointsGainUIManager.instance.ShowUIPoints(lastGrabbedByPlayer.playerManager.playerPawn.transform, points);
    }
}
