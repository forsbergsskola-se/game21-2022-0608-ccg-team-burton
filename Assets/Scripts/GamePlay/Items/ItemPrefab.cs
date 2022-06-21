using System;
using Entity.Items;
using UnityEngine;

public class ItemPrefab : MonoBehaviour
{
    public void SetItemPrefab(ItemSO itemSO)
    {
        //ItemPrefabSetup
        
        //How do I use Weapon vs WeaponSO here properly?

        
        
        if (itemSO is WeaponSO weaponSo)
        {
            var weapon = ItemFactory.CreateItemFromInventory(weaponSo) as Weapon;
            Debug.Log(weapon.ItemName);
            Debug.Log(weapon.Rarity);
            Debug.Log(weapon.WeaponDamage);
            GetComponent<SpriteRenderer>().sprite = weaponSo.ItemSprite;
            //addcomponenet<GEM>???
        }
        
        if (itemSO is ArmorSO armorSo)
        {
            var armor = ItemFactory.CreateItemFromInventory(armorSo) as Armor;
            Debug.Log(armor.ItemName);
            Debug.Log(armor.Rarity);
            Debug.Log(armor);
            GetComponent<SpriteRenderer>().sprite = armorSo.ItemSprite;
        }
    }
}
