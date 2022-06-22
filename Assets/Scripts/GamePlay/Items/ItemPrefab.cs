using System;
using Entity.Items;
using UnityEngine;

public class ItemPrefab : MonoBehaviour
{
    public void SetUpItemPrefab(ItemSO itemSO)
    {
        //ItemPrefabSetup
        
        //How do I use Weapon vs WeaponSO here properly? How do I configure weapon vs armor ion same itemprefab?

        //TODO: Get dependency by DI
        
        if (itemSO is WeaponSO weaponSo)
        {
            var weapon = ItemFactory.CreateItemFromInventory(weaponSo) as Weapon;
            
            Debug.Log("Weapon Name: "+weapon.ItemName);
            Debug.Log("Wewapon Rarity" +weapon.Rarity);
            Debug.Log("Weapon damage" +weapon.WeaponDamage);
            GetComponent<SpriteRenderer>().sprite = weaponSo.ItemSprite;
            //addcomponenet<GEM>??? --> issue if not monobehaviour I guess?
            
        }
        
        if (itemSO is ArmorSO armorSo)
        {
            var armor = ItemFactory.CreateItemFromInventory(armorSo) as Armor;
            Debug.Log("Armor name: "+armor.ItemName);
            Debug.Log("Armor Rarity: " +armor.Rarity);
            Debug.Log("Armor Effect VAlue (e.g. hp bonus): " +armor.EffectValue);
            GetComponent<SpriteRenderer>().sprite = armorSo.ItemSprite;
        }
    }
}
