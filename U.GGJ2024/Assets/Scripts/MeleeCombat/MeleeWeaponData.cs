using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeaponData", menuName = "ScriptableObjects/MeleeWeaponData")]
public class MeleeWeaponData : ScriptableObject
{
    public string weaponName;
    public float Damage;
    public GameObject weaponPrefab;
}
