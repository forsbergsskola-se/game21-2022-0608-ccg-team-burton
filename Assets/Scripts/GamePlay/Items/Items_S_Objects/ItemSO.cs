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
        
        
        
        
        
        [Header("Attack properties")]
        public int BaseAttackDamage; 
        public float AttackDamageMultiplier; 
        public float TotalAttackDamage;

        private void OnValidate() // To visualize changes in inspector
        {
            SetData();
        }

        public void SetData()
        {
           itemTypeSo.ScaleStats(this);
            // TotalAttackDamage = BaseAttackDamage * AttackDamageMultiplier;
        }
    }
}
