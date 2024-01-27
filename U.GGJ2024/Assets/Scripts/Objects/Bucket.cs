using UnityEngine;

public class Bucket : GrabbableObject
{
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
            
            BucketOn();
        }
    }

    private void BucketOn()
    {
        bucketedPlayer.BucketOn();
        Destroy(gameObject);
    }
}