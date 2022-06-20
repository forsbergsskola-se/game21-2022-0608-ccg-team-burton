using System;
using Entity.Items;
using UnityEngine;
 
[CreateAssetMenu(fileName = "WeaponType", menuName = "Item System/WeaponType")]
public class ItemTypeSO : ScriptableObject
{
    [Header("Damage Scaling")]
    public float BaseDamageMultiplier;
    public float DamageMultiplierStep;
    [Header("Effect Value Scaling")]
    public float EffectValueStep;
    [Header("Effect Duration Scaling")]
    public float EffectDurationStep;
    
    
    //TODO: Duplicate code --> cleanup!
    public void ScaleStats(ItemSO itemSo)
    {
        itemSo.AttackDamageMultiplier = itemSo.Rarity switch
        {
            Rarity.Common => BaseDamageMultiplier,
            Rarity.Uncommon => BaseDamageMultiplier+DamageMultiplierStep,
            Rarity.Rare =>BaseDamageMultiplier+DamageMultiplierStep*2,
            Rarity.Epic => BaseDamageMultiplier+DamageMultiplierStep*3,
            Rarity.Legendary => BaseDamageMultiplier+DamageMultiplierStep*4,

            _ => itemSo.AttackDamageMultiplier
        };
        itemSo.TotalAttackDamage = itemSo.BaseAttackDamage * itemSo.AttackDamageMultiplier;

        itemSo.EffectValueModifier = itemSo.Rarity switch
        {
            Rarity.Common => 0,
            Rarity.Uncommon => EffectValueStep,
            Rarity.Rare =>  EffectValueStep*2,
            Rarity.Epic =>  EffectValueStep*3,
            Rarity.Legendary =>  EffectValueStep*4,
            _ => itemSo.EffectValueModifier
        };
        if (itemSo.BaseEffectValue != 0)
            itemSo.TotalEffectValue = itemSo.BaseEffectValue + itemSo.EffectValueModifier;
        else
            itemSo.TotalEffectValue = 0;
        
        itemSo.EffectDurationModifier = itemSo.Rarity switch
        {
            Rarity.Common => 0,
            Rarity.Uncommon =>  EffectDurationStep,
            Rarity.Rare =>  EffectDurationStep*2,
            Rarity.Epic =>  EffectDurationStep*3,
            Rarity.Legendary => EffectDurationStep*4,
            _ => itemSo.EffectValueModifier
        };
        if (itemSo.BaseEffectDuration != 0)
            itemSo.TotalEffectDuration = itemSo.BaseEffectDuration + itemSo.EffectDurationModifier;
        else
            itemSo.TotalEffectDuration = 0;
    }
    
}
 

 