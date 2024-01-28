using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Propane : GrabbableObject
{
    [Range(-9, 9)] [SerializeField] private int points;
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private float timeToExplode = 3.0f;
    [SerializeField] private float explosionForce = 10.0f;
    [SerializeField] private float explosionRadius = 5.0f;
    
    private MeshRenderer meshRenderer;
    private float explodeTimer;

    private bool once = false;
    private bool once2 = false;

    public UnityEvent OnExplosion;

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
        if (explodeTimer <= 0)
        {
            if (!once2)
            {
                Explode();
                once2 = true;
            }
        }
    }

    private void Explode()
    {
        if (!once)
        {
            ParticleSystem particle = Instantiate(explosionParticle, transform.position, Quaternion.identity);
            OnExplosion.Invoke();
            SFX();
            Destroy(particle, 5.0f);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        Rigidbody rigidbody;
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
                playerManager.RagdollController.pelvisRigidbody.AddForce(forceDirection * explosionForce,
                    ForceMode.Impulse);
                playerManager.RagdollController.DisableRagdollWithDelay(3.0f);
                
                UpdatePlayerPoints(playerManager);
            }
            else if (collider.transform.TryGetComponent(out rigidbody))
            {
                if (!rigidbody.isKinematic)
                {
                    Vector3 forceDirection = (rigidbody.transform.position - transform.position).normalized;
                    rigidbody.AddForce(forceDirection * explosionForce,
                        ForceMode.Impulse);
                }
            }
        }

        meshRenderer.enabled = false;
        Destroy(gameObject, 5);
    }

    private void UpdatePlayerPoints(NPlayerManager affectedPlayer)
    {
        PointsGainUIManager.instance.ShowUIPoints(
            affectedPlayer.playerPawn, points);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
    
    void SFX()
    {
        AudioManager audioManager = AudioManager.instance;
        audioManager.PlayOneShotSfx(audioManager.bombSfx);
    }
}