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
        private readonly SetItemAttackData _setItemAttackData;

        public ItemSO()
        {
            _setItemAttackData = new SetItemAttackData(this);
        }


        private void OnValidate() // TO visualize changes in inspector
        {
            SetData();
        }

        public void SetData()
        {
            _setItemAttackData.SetAttackDamageMultiplier();
            
            TotalAttackDamage = BaseAttackDamage * AttackDamageMultiplier;
        }
    }
}
