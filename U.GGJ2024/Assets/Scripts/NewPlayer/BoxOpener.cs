using System;
using UnityEngine;

public class BoxOpener : MonoBehaviour
{
    private NPlayerManager playerManager;

    [SerializeField] private Transform interactOrigin;
    [SerializeField] private float detectionSphereRadius;

    private void Start()
    {
        playerManager = GetComponentInParent<NPlayerManager>();
        playerManager.InputHandler.OnOpenBox += OnOpenBox;
    }

    private void OnOpenBox()
    {
        Collider[] colliders = Physics.OverlapSphere(interactOrigin.position, detectionSphereRadius);
        foreach (Collider collider in colliders)
        {
            OpenMysteryBox mysteryBox = collider.transform.GetComponent<OpenMysteryBox>();
            if (mysteryBox)
            {
                mysteryBox.OpenBox();
                return;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(interactOrigin.position, detectionSphereRadius);
    }
}