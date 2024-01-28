using UnityEngine;

public class Alcohol : GrabbableObject
{
    private NPlayerManager hittedPlayer;
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
            SFX();
            VFX();
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

    //SFX And VFX
    void SFX()
    {
        AudioManager audioManager = AudioManager.instance;
        audioManager.PlayOneShotSfx(audioManager.bottleSFx);
    }
    public VfxManager vfx => GetComponent<VfxManager>();
    void VFX()
    {
        if (vfx != null)
        {
            vfx.SpawnVFX(vfx.vfxList[0], 2);
        }
    }
}
