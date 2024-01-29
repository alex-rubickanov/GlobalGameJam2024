using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : GrabbableObject
{
    [Range(-9, 9)]
    [SerializeField] private int points;
    private MeshRenderer meshRenderer;
    [SerializeField] private float delayToHide = 1f;
    [SerializeField] private float forceToLeg = 10.0f;
    [SerializeField] private float timeToDestroy = 2f;
    private bool isPlanted = false;
    
    private bool once = false;
    protected override void Start()
    {
        base.Start();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && wasThrown)
        {
            PlantBanana();
            wasThrown = false;
        }
    }

    private void PlantBanana()
    {
        canBeGrabbed = false;
        StartCoroutine(HideWithDelay());
        isPlanted = true;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isPlanted) return;
        if(other.CompareTag("Player"))
        {
            NPlayerManager playerManager = other.gameObject.GetComponentInParent<NPlayerManager>();
            if(playerManager.RagdollController.isRagdoll) return;

            meshRenderer.enabled = true;
            
            playerManager.RagdollController.EnableRagdoll();
            playerManager.RagdollController.GetRandomLeg().AddForce(other.transform.forward * forceToLeg, ForceMode.Impulse);
            playerManager.RagdollController.DisableRagdollWithDelay(3.0f);
            
            UpdatePlayerPoints(playerManager);
            
            if (!once)
            {
                SFX();
                Destroy(gameObject, timeToDestroy);
            }
        }
    }
    
    private IEnumerator HideWithDelay()
    {
        yield return new WaitForSeconds(delayToHide);
        meshRenderer.enabled = false;
    }
    
    private void UpdatePlayerPoints(NPlayerManager playerManager)
    {
        if (playerManager.playerPawn == lastGrabbedByPlayer.playerManager.playerPawn) return;
        PointsGainUIManager.instance.ShowUIPoints(lastGrabbedByPlayer.playerManager.playerPawn, points);
    }




    //SFX
    void SFX()
    {
        AudioManager audioManager = AudioManager.instance;
        audioManager.PlayOneShotSfx(audioManager.SlideSfx);
    }
}
