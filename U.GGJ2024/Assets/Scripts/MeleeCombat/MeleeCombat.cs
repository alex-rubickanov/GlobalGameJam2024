using System;
using System.Collections;
using UnityEngine;

public class MeleeCombat : MonoBehaviour
{
    [SerializeField] private MeleeWeapon currentWeapon;
    [SerializeField] private Transform weaponSlot;
    [SerializeField] private Transform weaponColliderOrigin;
    [SerializeField] private float detectionSphereRadius;
    [SerializeField] private float hitCooldown;
    private bool isAttackCooldown = false;
    public Action OnMeleeHitStart;
    
    private PlayerManager PlayerManager => GetComponentInParent<PlayerManager>();
    private void Start()
    {
        PlayerManager.InputHandler.OnMeleeHit += Attack;
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
        results = Physics.OverlapSphere(weaponColliderOrigin.position, detectionSphereRadius);
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

    IEnumerator AttackCooldown()
    {
        isAttackCooldown = true;
        yield return new WaitForSeconds(hitCooldown);
        isAttackCooldown = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weaponColliderOrigin.position, detectionSphereRadius);
    }
    
    private bool CanAttack => PlayerManager.CanAttack() && currentWeapon != null && !isAttackCooldown;
}