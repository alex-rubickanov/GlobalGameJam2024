using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private PlayerManager playerManager => GetComponentInParent<PlayerManager>();
    
    [SerializeField] private float maxHealth;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("TAKEN DAMAGE: " + damage);
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            playerManager.KillPlayer();
            currentHealth = maxHealth;
        }
    }
}
