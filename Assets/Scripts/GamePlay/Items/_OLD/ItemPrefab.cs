// using Entity;
// using Newtonsoft.Json;
// using UnityEngine;
//
// public class ItemPrefab : MonoBehaviour
// {
//     // public EquipmentLibrary EquipmentLibrary;
//     //Values are set from whatever you equip
//     string itemID;
//     // string RarityID;
//     // string GemID;
//
//     // private Weapon Weapon;
//     // private Armor Armor;
//     private UpgradeMaterial _upgradeMaterial;
//     
//     private void Start()
//     {
//         // var jsonString = PlayerPrefs.GetString("Inventoryslot");
//         // var InventoryItem= JsonConvert.DeserializeObject<InventoryItemSerialization>(jsonString);
//
//         // Debug.Log($"Deserialized values: {InventoryItem.itemID}, {InventoryItem.rarityID}, {InventoryItem.gemId}");
//         var loadItem = PlayerPrefs.GetString("InventorySlot"); 
//
//         var itemSo = EquipmentLibrary.GetItemFromLibrary(loadItem);
//         // var raritySo = EquipmentLibrary.GetRarityFromLibrary(InventoryItem.rarityID);
//         // var gemSo = EquipmentLibrary.GetGemFromLibrary(InventoryItem.gemId);
//         
//         SetUpItemPrefab(itemSo);
//     }
//
//     private void SetUpItemPrefab(ItemSO itemSO)
//     {
//         //ItemPrefabSetup
//         
//         //How do I use Weapon vs WeaponSO here properly? How do I configure weapon vs armor in same itemprefab?
//
//         //TODO: Get dependency by DI for itemfactory
//
//         switch (itemSO)
//         {
//             case UpgradeMaterialSO upgradeMaterialSo:
//             {
//                 _upgradeMaterial = ItemFactory.CreateItemFromInventory(upgradeMaterialSo) as UpgradeMaterial;
//                 GetComponent<SpriteRenderer>().sprite = upgradeMaterialSo.ItemSprite;
//                 break;
//             }
//             // case WeaponSO weaponSo:
//             // {
//             //      Weapon = ItemFactory.CreateItemFromInventory(weaponSo, raritySo,gemSo) as Weapon;
//             //
//             //     Debug.Log("Weapon Name: " + Weapon.ItemName);
//             //     Debug.Log("Weapon Rarity " + Weapon.RaritySo.name);
//             //     Debug.Log("Weapon damage " + Weapon.WeaponDamage);
//             //     GetComponent<SpriteRenderer>().sprite = weaponSo.ItemSprite;
//             //
//             //     //ugly
//             //     if (weaponSo.GemSo != null)
//             //     {
//             //         switch (Weapon.Gem.GemType)
//             //         {
//             //             case GemType.Knockback when Weapon.GemActive:
//             //                 gameObject.AddComponent<Knockback>();
//             //                 break;
//             //             case GemType.Slow when Weapon.GemActive:
//             //                 gameObject.AddComponent<SlowingGem>();
//             //                 break;
//             //             case GemType.Stun when Weapon.GemActive:
//             //                 gameObject.AddComponent<StunGem>();
//             //                 break;
//             //         }
//             //     }
//             //
//             //     break;
//             // }
//             // case ArmorSO armorSo:
//             // {
//             //     Armor = ItemFactory.CreateItemFromInventory(armorSo,raritySo,gemSo) as Armor;
//             //     Debug.Log("Armor name: " +Armor.ItemName);
//             //     Debug.Log("Armor Rarity: " +Armor.RaritySo.name);
//             //     Debug.Log("Armor Effect Value (e.g. hp bonus): " +Armor.EffectValue);
//             //     Debug.Log("Armor Gem: "+Armor.Gem.name);
//             //     GetComponent<SpriteRenderer>().sprite = armorSo.ItemSprite;
//             //     break;
//             // }
//         }
//     }
// }