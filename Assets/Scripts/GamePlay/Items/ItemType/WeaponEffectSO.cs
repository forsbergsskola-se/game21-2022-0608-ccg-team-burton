using System;
using Entity.Items;
using UnityEngine;
 
[CreateAssetMenu(fileName = "WeaponType", menuName = "Item System/WeaponType")]
public class WeaponEffectSO : ItemEffectSO
{
    [Header("Damage Scaling")]
    public float BaseDamageMultiplier;
    public float DamageMultiplierStep;
    [Header("Effect Value Scaling")]
    public float EffectValueStep;
    [Header("Effect Duration Scaling")]
    public float EffectDurationStep;
    
    //TODO: Duplicate code --> cleanup!
    public void ScaleWeaponStats(WeaponSO weaponSo)
    {
        weaponSo.AttackDamageMultiplier = weaponSo.Rarity switch
        {
            Rarity.Common => BaseDamageMultiplier,
            Rarity.Uncommon => BaseDamageMultiplier+DamageMultiplierStep,
            Rarity.Rare =>BaseDamageMultiplier+DamageMultiplierStep*2,
            Rarity.Epic => BaseDamageMultiplier+DamageMultiplierStep*3,
            Rarity.Legendary => BaseDamageMultiplier+DamageMultiplierStep*4,

            _ => weaponSo.AttackDamageMultiplier
        };
        weaponSo.TotalAttackDamage = weaponSo.BaseAttackDamage * weaponSo.AttackDamageMultiplier;

        // base.ScaleStats(weaponSo);
        
    }
}
 

 