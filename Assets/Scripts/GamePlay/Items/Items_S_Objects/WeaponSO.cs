using UnityEngine;

namespace Entity.Items
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Item System/New Weapon")]
    public class WeaponSO : ItemSO
    {
        [Header("Set weapon base data")]
        public int BaseAttackDamage; 
        [Header("ITEM STATS (Auto-set)")] 
        [Header("Modifiers (Auto-calculated)")]
        public float AttackDamageMultiplier; 
        [Header("Totals (Auto-calculated)")]
        public float TotalAttackDamage;

        // public Rarity Rarity => base.Rarity;
    }
}
