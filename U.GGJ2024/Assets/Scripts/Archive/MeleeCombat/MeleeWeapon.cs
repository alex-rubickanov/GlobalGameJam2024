using UnityEngine;

public abstract class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private MeleeWeaponData meleeWeaponData;
    protected MeleeCombat owner;
    [SerializeField] protected float hitMultiplier;
    
    public void SetOwner(MeleeCombat owner)
    {
        this.owner = owner;
    }
    
    public MeleeWeaponData GetWeaponData()
    {
        return meleeWeaponData;
    }
}
