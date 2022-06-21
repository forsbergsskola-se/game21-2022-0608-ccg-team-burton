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
            var effectMod = new WeaponEffectMod();

            DamageMod.ApplyStatChange(weapon, weaponSo);  
            effectMod.ApplyStatChange(weapon, weaponSo);


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
        Debug.Log(weaponSo.WeaponBaseDamage);

        weapon.WeaponDamage = weaponSo.WeaponBaseDamage + (int) weaponSo.Rarity * weaponSo.WeaponBaseDamage; // scaling with int enum bad??
        Debug.Log(weapon.WeaponDamage);

        // float damageMod = item.Rarity switch
        // {
        //     Rarity.Common => 1,
        //     Rarity.Uncommon => 2,
        //     Rarity.Rare => 3,
        //     Rarity.Epic => 4,
        //     Rarity.Legendary =>5,
        //     _ => throw new ArgumentOutOfRangeException()
        // };
        //
        // weapon.WeaponDamage = damageMod *weaponSo.WeaponBaseDamage;
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
                armor.EffectValue = armorSo.BaseEffect +
                                    armorSo.RarityLevelEffectIncrease * (int)armorSo.Rarity;
            }
        }
    }
}

public class WeaponEffectMod : IStatsModifier
{
    public void ApplyStatChange(Item item, ItemSO itemSo)
    {
        Debug.Log("Effects gogogo");
    }
}


public interface IStatsModifier
{
    void ApplyStatChange(Item item, ItemSO itemSo); //Why unused?
}

