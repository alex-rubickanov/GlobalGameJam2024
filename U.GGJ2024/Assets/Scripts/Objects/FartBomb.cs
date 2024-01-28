using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FartBomb : GrabbableObject
{
    [Range(-9, 9)]
    [SerializeField] private int pointsToWhoThrowed;
    [Range(-9, 9)]
    [SerializeField] private int pointsToWhoWasAffected;
    [SerializeField] private float explosionRadius;
    private bool once = false;

    protected override void OnCollisionEnter(Collision other)
    {
        if (wasThrown)
        {
            NPlayerManager player = other.transform.GetComponentInParent<NPlayerManager>();
            if (player)
            {
                return;
            }

            if (!once)
            {
                SFX();
                VFX();
                Explode();
                once = true;
            }
        }
    }


    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        NPlayerManager playerManager;
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                playerManager = collider.gameObject.GetComponentInParent<NPlayerManager>();
                playerManager.FartOn();
                UpdatePlayerPoints(playerManager);
            }
        }
        canBeGrabbed = false;
        Destroy(gameObject, 5);
    }
    
    private void UpdatePlayerPoints(NPlayerManager affectedPlayer)
    {
        PointsGainUIManager.instance.ShowUIPoints(affectedPlayer.playerPawn.transform, pointsToWhoWasAffected);
        PointsGainUIManager.instance.ShowUIPoints(lastGrabbedByPlayer.playerManager.playerPawn.transform, pointsToWhoThrowed);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }


    //VFX AND SFX
    void SFX()
    {
        AudioManager audioManager = AudioManager.instance;
        audioManager.PlayOneShotSfx(audioManager.fartBombSfx);
    }
    public VfxManager vfx => GetComponent<VfxManager>();
    void VFX()
    {
        if(vfx != null)
        {
            vfx.SpawnVFX(vfx.vfxList[0], 3);
        }
    }
}