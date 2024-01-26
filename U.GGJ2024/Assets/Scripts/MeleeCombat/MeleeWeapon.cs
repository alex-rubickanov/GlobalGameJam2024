using UnityEngine;

public abstract class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private MeleeWeaponData meleeWeaponData;
    
    public MeleeWeaponData GetWeaponData()
    {
        return meleeWeaponData;
    }
}
