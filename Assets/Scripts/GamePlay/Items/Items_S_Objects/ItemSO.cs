using UnityEngine;

namespace Entity.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Item System/New Item")]
    public class ItemSO : ScriptableObject
    {
        public string ItemName;
        public Rarity Rarity;
        public ItemTypeSO itemTypeSo;
        public Sprite ItemSprite;

        public ItemType itemType;
        
        [Header("Set Item base data")]
        public int BaseAttackDamage; 
        public float BaseEffectValue;
        public float BaseEffectDuration;

        [Header("ITEM STATS (Auto-set)")] 
        [Header("Modifiers (Auto-calculated)")]
        public float AttackDamageMultiplier; 
        public float EffectValueModifier;
        public float EffectDurationModifier;

        [Header("Totals (Auto-calculated)")]
        public float TotalAttackDamage;
        public float TotalEffectValue;
        public float TotalEffectDuration;
        
        private void OnValidate() // To visualize changes in inspector
        {
            SetData();
        }

        public void SetData()
        {
           itemTypeSo.ScaleStats(this);
        }
    }
}
