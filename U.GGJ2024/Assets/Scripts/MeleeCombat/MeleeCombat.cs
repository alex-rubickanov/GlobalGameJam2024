using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MeleeCombat : MonoBehaviour
{
    private NPlayerManager playerManager;

    [SerializeField] private MeleeWeapon currentWeapon;
    [SerializeField] private Transform weaponSlot;
    [SerializeField] private Transform interactionOrigin;
    [SerializeField] private float detectionSphereRadius;
    [SerializeField] private float hitCooldown;

    private bool isAttackCooldown = false;
    public Action OnMeleeHitStart;

    private void Start()
    {
        playerManager = GetComponentInParent<NPlayerManager>();
        playerManager.InputHandler.OnMeleeHit += Attack;
        playerManager.InputHandler.OnEquipWeapon += TryToEquip;
    }

    public void EquipWeapon(MeleeWeaponData weaponData)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
            currentWeapon = null;
        }

        GameObject spawnedWeapon = Instantiate(weaponData.weaponPrefab, weaponSlot);
        currentWeapon = spawnedWeapon.GetComponent<MeleeWeapon>();
        currentWeapon.SetOwner(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"POS: {weaponSlot.position} || LOCPOS: {weaponSlot.localPosition}");
            Debug.Log($"ROT: {weaponSlot.rotation} || LOCROT: {weaponSlot.localRotation}");
        }
    }

    private void Attack()
    {
        if (CanAttack)
        {
            StartCoroutine(AttackCooldown());
            OnMeleeHitStart?.Invoke();
        }
    }

    public void DetectHit()
    {
        Collider[] results;
        results = Physics.OverlapSphere(interactionOrigin.position, detectionSphereRadius);
        foreach (var result in results)
        {
            if (result == GetComponent<Collider>()) continue;

            IDamageable damageable;

            if (result.TryGetComponent(out damageable))
            {
                damageable.TakeDamage(currentWeapon.GetWeaponData().Damage);
            }
        }
    }

    private void TryToEquip()
    {
        Collider[] results;
        results = Physics.OverlapSphere(interactionOrigin.position, detectionSphereRadius);
        foreach (var result in results)
        {
            MeleeWeapon weapon;
            if (result.TryGetComponent(out weapon))
            {
                EquipWeapon(weapon.GetWeaponData());
                return;
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        isAttackCooldown = true;
        yield return new WaitForSeconds(hitCooldown);
        isAttackCooldown = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionOrigin.position, detectionSphereRadius);
    }

    private bool CanAttack => playerManager.CanAttack() && currentWeapon != null && !isAttackCooldown;
}