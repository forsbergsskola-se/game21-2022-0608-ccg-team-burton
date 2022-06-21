using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Items;
using UnityEngine;

public class ItemSO : ScriptableObject
{
    public string ItemName;
    public Rarity Rarity;
    public ItemEffectSO itemEffectSo;

    // public ItemType itemType;
        
    [Header("Set Item Effect data")]
    // public int BaseAttackDamage; 
    public float BaseEffectValue;
    public float BaseEffectDuration;

    [Header("ITEM STATS (Auto-set)")] 
    [Header("Modifiers (Auto-calculated)")]
    // public float AttackDamageMultiplier; 
    public float EffectValueModifier;
    public float EffectDurationModifier;

    [Header("Totals (Auto-calculated)")]
    // public float TotalAttackDamage;
    public float TotalEffectValue;
    public float TotalEffectDuration;

    private void OnValidate()
    {
        //Update inspector values in editor in RT by calling SetItemStats();
        throw new NotImplementedException();
    }

    public void SetItemStats(ItemSO itemSo)
    {
        itemSo.itemEffectSo.ScaleItemStats(this);
    } 
}
