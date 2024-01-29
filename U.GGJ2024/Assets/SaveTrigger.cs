using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        NPlayerManager playerManager = other.GetComponentInParent<NPlayerManager>();
        if (playerManager)
        {
            playerManager.DespawnPlayer();
            playerManager.SpawnPlayer();
        }
    }
}
