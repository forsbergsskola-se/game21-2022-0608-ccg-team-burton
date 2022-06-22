using System;
using Entity;
using Entity.Items;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPrefab : MonoBehaviour
{
    public void SetUpItemPrefab(ItemSO itemSO)
    {
        //ItemPrefabSetup
        
        //How do I use Weapon vs WeaponSO here properly? How do I configure weapon vs armor in same itemprefab?

        //TODO: Get dependency by DI

        if (itemSO is WeaponSO weaponSo)
        {
            var weapon = ItemFactory.CreateItemFromInventory(weaponSo) as Weapon;

            Debug.Log("Weapon Name: " + weapon.ItemName);
            Debug.Log("Wewpon Rarity " + weapon.RaritySo.name);
            Debug.Log("Weapon damage " + weapon.WeaponDamage);
            GetComponent<SpriteRenderer>().sprite = weaponSo.ItemSprite;

            //ugly
            if (weaponSo.Gem != null)
            {
             
                if (weapon.Gem.GemType == Gems.Knockback && weapon.GemActive)
                {
                    gameObject.AddComponent<Knockback>();
                
                } else if (weapon.Gem.GemType == Gems.Slow && weapon.GemActive)
                {
                    gameObject.AddComponent<Slowing>();

                }   
            }

        }
        
        if (itemSO is ArmorSO armorSo)
        {
            var armor = ItemFactory.CreateItemFromInventory(armorSo) as Armor;
            Debug.Log("Armor name: "+armor.ItemName);
            Debug.Log("Armor Rarity: " +armor.RaritySo.name);
            Debug.Log("Armor Effect Value (e.g. hp bonus): " +armor.EffectValue);
            GetComponent<SpriteRenderer>().sprite = armorSo.ItemSprite;
        }
    }
}