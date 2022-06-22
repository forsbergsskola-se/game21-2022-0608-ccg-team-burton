using System;
using Entity;
using Entity.Items;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPrefab : MonoBehaviour
{
    //Values are loaded
    // public string itemID;
    // public string RarityID;
    // public string GemID;
    
    //these values hould be gotten from library via loaded string ID
    public ItemSO ItemSo;
    public ItemRaritySO RaritySo;
    public GemSO GemSo;
    //constructing an item
    private void Start()
    {
        // var itemSo = GetFromLibrary.GetItemFromLibrary(itemID);
        // var raritySo = GetFromLibrary.GetRarityFromLibrary(RarityID);
        // var gemSo = GetFromLibrary.GetGemFromLibrary(GemID);
        
        SetUpItemPrefab(ItemSo, RaritySo, GemSo);
        
        
        
    }

    public void SetUpItemPrefab(ItemSO itemSO, ItemRaritySO raritySo, GemSO gemSo)
    {
        //ItemPrefabSetup
        
        //How do I use Weapon vs WeaponSO here properly? How do I configure weapon vs armor in same itemprefab?

        //TODO: Get dependency by DI for itemfactory

        switch (itemSO)
        {
            case WeaponSO weaponSo:
            {
                var weapon = ItemFactory.CreateItemFromInventory(weaponSo, raritySo,gemSo) as Weapon;

                Debug.Log("Weapon Name: " + weapon.ItemName);
                Debug.Log("Weapon Rarity " + weapon.RaritySo.name);
                Debug.Log("Weapon damage " + weapon.WeaponDamage);
                GetComponent<SpriteRenderer>().sprite = weaponSo.ItemSprite;

                //ugly
                if (weaponSo.GemSo != null)
                {
                    switch (weapon.Gem.GemType)
                    {
                        case GemType.Knockback when weapon.GemActive:
                            gameObject.AddComponent<Knockback>();
                            break;
                        case GemType.Slow when weapon.GemActive:
                            gameObject.AddComponent<SlowingGem>();
                            break;
                        case GemType.Stun when weapon.GemActive:
                            gameObject.AddComponent<StunGem>();
                            break;
                    }
                }

                break;
            }
            case ArmorSO armorSo:
            {
                var armor = ItemFactory.CreateItemFromInventory(armorSo,raritySo,gemSo) as Armor;
                Debug.Log("Armor name: " +armor.ItemName);
                Debug.Log("Armor Rarity: " +armor.RaritySo.name);
                Debug.Log("Armor Effect Value (e.g. hp bonus): " +armor.EffectValue);
                Debug.Log("Armor Gem: "+armor.Gem.name);
                GetComponent<SpriteRenderer>().sprite = armorSo.ItemSprite;
                break;
            }
        }
    }
}