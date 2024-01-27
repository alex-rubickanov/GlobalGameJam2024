using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FartBomb : GrabbableObject
{
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
            if (collider.CompareTag("Player") || collider.GetComponentInParent<NPlayerManager>())
            {
                playerManager = collider.gameObject.GetComponentInParent<NPlayerManager>();
                playerManager.FartOn();
            }
        }

        canBeGrabbed = false;
        Destroy(gameObject, 5);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}