using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propane : GrabbableObject
{
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private float timeToExplode = 3.0f;
    [SerializeField] private float explosionForce = 10.0f;
    [SerializeField] private float explosionRadius = 5.0f;

    private MeshRenderer meshRenderer;
    private float explodeTimer;

    private bool once = false;

    protected override void Start()
    {
        base.Start();
        meshRenderer = GetComponent<MeshRenderer>();
        explodeTimer = timeToExplode;
    }

    protected override void Update()
    {
        base.Update();
        
        explodeTimer -= Time.deltaTime;
        if(explodeTimer <= 0)
            Explode();
    }

    private void Explode()
    {
        if (!once)
        {
            ParticleSystem particle = Instantiate(explosionParticle, transform.position, Quaternion.identity);
            Destroy(particle.gameObject, 5.0f);
            once = true;
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                NPlayerManager playerManager = collider.gameObject.GetComponentInParent<NPlayerManager>();
                if (playerManager.PlayerGrabbing.isGrabbing)
                {
                    playerManager.PlayerGrabbing.LooseObject();
                }
                playerManager.RagdollController.EnableRagdoll();
                Vector3 forceDirection = (playerManager.transform.position - transform.position).normalized;
                playerManager.RagdollController.pelvisRigidbody.AddForce(forceDirection * explosionForce, ForceMode.Impulse);
                playerManager.RagdollController.DisableRagdollWithDelay(3.0f);
            }
        }

        meshRenderer.enabled = false;
        Destroy(gameObject, 5);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
