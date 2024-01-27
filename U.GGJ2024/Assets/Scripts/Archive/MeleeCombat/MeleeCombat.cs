using System;
using System.Collections;
using UnityEngine;

public class MeleeCombat : MonoBehaviour
{
    private NPlayerManager playerManager;

    [SerializeField] private MeleeWeapon currentWeapon;
    [SerializeField] private Transform weaponSlot;
    [SerializeField] private Transform interactionOrigin;
    [SerializeField] private float detectionSphereRadius;
    [SerializeField] private float hitCooldown;

    public Action OnMeleeHitStart;

    private void Start()
    {
        playerManager = GetComponentInParent<NPlayerManager>();
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionOrigin.position, detectionSphereRadius);
    }
}