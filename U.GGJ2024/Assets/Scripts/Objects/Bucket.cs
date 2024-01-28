using UnityEngine;

public class Bucket : GrabbableObject
{
    [Range(-9, 9)]
    [SerializeField] private int points;
    private NPlayerManager bucketedPlayer;

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            wasThrown = false;
        } else if(other.gameObject.layer == LayerMask.NameToLayer("CharacterBones") && wasThrown)
        {
            bucketedPlayer = other.gameObject.GetComponentInParent<NPlayerManager>();
            if (bucketedPlayer.PlayerGrabbing == lastGrabbedByPlayer || bucketedPlayer.isStun || bucketedPlayer.PlayerMovement.isBucket)
            {
                bucketedPlayer = null;
                return;
            }
            UpdatePlayerPoints();
            BucketOn();
        }
    }

    private void BucketOn()
    {
        bucketedPlayer.BucketOn();
        Destroy(gameObject);
    }
    
    private void UpdatePlayerPoints()
    {
        PointsGainUIManager.instance.ShowUIPoints(lastGrabbedByPlayer.playerManager.playerPawn.transform, points);
    }
}