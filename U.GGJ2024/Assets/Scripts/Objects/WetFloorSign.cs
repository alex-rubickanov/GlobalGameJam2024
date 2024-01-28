using System;
using Unity.VisualScripting;
using UnityEngine;

public class WetFloorSign : GrabbableObject
{
    [Range(-9, 9)] [SerializeField] private int points;
    [SerializeField] private float timeToDestroy;
    [SerializeField] private GameObject water;
    private bool isSpilled = false;
    [SerializeField] private float forceToLeg = 10.0f;
    protected override void Start()
    {
        base.Start();
        water.SetActive(false);
    }


    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && wasThrown)
        {
            SpillWater();
            wasThrown = false;
        }
    }

    private void SpillWater()
    {
        canBeGrabbed = false;
        isSpilled = true;
        water.SetActive(true);
        GetComponent<Rigidbody>().isKinematic = true;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        
        Destroy(gameObject, timeToDestroy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isSpilled) return;
        if(other.CompareTag("Player"))
        {
            NPlayerManager playerManager = other.gameObject.GetComponentInParent<NPlayerManager>();
            if(playerManager.RagdollController.isRagdoll) return;
            
            playerManager.RagdollController.EnableRagdoll();
            playerManager.RagdollController.GetRandomLeg().AddForce(other.transform.forward * forceToLeg, ForceMode.Impulse);
            playerManager.RagdollController.DisableRagdollWithDelay(3.0f);
            SFX();
            UpdatePlayerPoints();
        }
    }
    
    private void UpdatePlayerPoints()
    {
        PointsGainUIManager.instance.ShowUIPoints(lastGrabbedByPlayer.playerManager.playerPawn.transform, points);
    }

    //SFX
    void SFX()
    {
        AudioManager audioManager = AudioManager.instance;
        audioManager.PlayOneShotSfx(audioManager.SlideSfx);
    }
}
