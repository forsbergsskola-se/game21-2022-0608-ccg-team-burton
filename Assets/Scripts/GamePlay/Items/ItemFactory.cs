using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Items;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ItemFactory 
{
 
    public static Item CreateItemFromInventory(ItemSO itemSo) //TODO FIX STATIC!
    {

        if (itemSo is WeaponSO weaponSo)
        {
            var weapon = new Weapon();
            weapon.ItemName = weaponSo.ItemName;
            weapon.Rarity = weaponSo.Rarity;
            
            var DamageMod = new WeaponDamageStatModifier();
            DamageMod.ApplyStatChange(weapon, weaponSo);  
            return weapon;
        }

        if (itemSo is ArmorSO armorSo)
        {
            var armor = new Armor();
            armor.ItemName = armorSo.ItemName;
            armor.Rarity = armorSo.Rarity;
            var EffectMod = new ArmorEffectModification();
            EffectMod.ApplyStatChange(armor,armorSo);

            return armor;
        }
        

        
        return new Item();
    }
}

public class Item
{
    public string ItemName;
    public Rarity Rarity;

}
public class Weapon : Item
{
    public float WeaponDamage;
    
    
}

public class Armor : Item
{
    public float EffectValue;
}

public class WeaponDamageStatModifier : IStatsModifier
{
    public void ApplyStatChange(Item item, ItemSO itemSo)
    {
        if (item is not Weapon weapon) return;
        if (itemSo is not WeaponSO weaponSo) return;
        //Is this really going here?


        weapon.WeaponDamage = weaponSo.WeaponBaseDamage + (int) weaponSo.Rarity * weaponSo.WeaponBaseDamage; // scaling with int enum bad??

    }
}

public class ArmorEffectModification : IStatsModifier
{
    public void ApplyStatChange(Item item, ItemSO itemSo)
    {
        if (item is Armor armor)
        {
            if (itemSo is ArmorSO armorSo)
            {
                armor.EffectValue = armorSo.BaseEffect + armorSo.RarityLevelEffectIncrease * (int)armorSo.Rarity;
            }
        }
    }
}

public interface IStatsModifier
{
    void ApplyStatChange(Item item, ItemSO itemSo); //Why unused?
}

