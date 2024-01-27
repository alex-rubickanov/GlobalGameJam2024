using UnityEngine;

public class MeleeWeaponPickable : MonoBehaviour
{
    [SerializeField] private MeleeWeaponData meleeWeaponData;

    private MeleeCombat playerMeleeCombat;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out playerMeleeCombat))
        {
            playerMeleeCombat.EquipWeapon(meleeWeaponData);
            Destroy(gameObject);
        }
    }
}
