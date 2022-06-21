using System.Collections;
using System.Collections.Generic;
using Entity.Items;
using UnityEngine;

public class ItemEffectSO : ScriptableObject
{
    [Header("Damage Scaling")]
    public float BaseDamageMultiplier;
    public float DamageMultiplierStep;
    [Header("Effect Value Scaling")]
    public float EffectValueStep;
    [Header("Effect Duration Scaling")]
    public float EffectDurationStep;
    
     
    public void ScaleItemStats(ItemSO itemSo)
    {

        //FOR WEAPONS ONLY
        if (itemSo.itemEffectSo.GetType() == typeof(WeaponSO))
        {
            //Add weapon part here
            
        }
            
        //For all items (ARMOR + WEAPON)
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
