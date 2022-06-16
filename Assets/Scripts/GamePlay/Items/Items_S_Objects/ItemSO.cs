using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Items;
using UnityEngine;

namespace Entity.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Item System/New Item")]
    public class ItemSO : ScriptableObject
    {
        public string ItemName;
        public Rarity Rarity;
        public ItemTypeEnum ItemType;
        
        [Header("Attack properties")]
        public int BaseAttackDamage; 
        public float AttackDamageMultiplier; 
        public float TotalAttackDamage;

        private void OnValidate()
        {
            AttackDamageMultiplier = Rarity switch
            {
                Rarity.Common => ItemType switch
                {
                    ItemTypeEnum.Standard => 1f,
                    ItemTypeEnum.Acid => 1f,
                    _ => 1f
                },
                Rarity.Uncommon => ItemType switch
                {
                    ItemTypeEnum.Standard => 2f,
                    ItemTypeEnum.Acid => 1.5f,
                    _ => 1f
                },
                Rarity.Rare => ItemType switch
                {
                    ItemTypeEnum.Standard => 3f,
                    ItemTypeEnum.Acid => 2f,
                    _ => 1f
                },
                Rarity.Epic => ItemType switch
                {
                    ItemTypeEnum.Standard => 4f,
                    ItemTypeEnum.Acid => 2.5f,
                    _ => 1f
                },
                Rarity.Legendary => ItemType switch
                {
                    ItemTypeEnum.Standard => 5f,
                    ItemTypeEnum.Acid => 3f,
                    _ => 1f
                },
                _ => throw new ArgumentOutOfRangeException()
            };

            TotalAttackDamage = BaseAttackDamage * AttackDamageMultiplier;
        }
    }
}
