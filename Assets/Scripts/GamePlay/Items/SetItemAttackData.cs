using System;

namespace Entity.Items
{
    public class SetItemAttackData
    {
        private ItemSO _itemSo;

        public SetItemAttackData(ItemSO itemSo)
        {
            _itemSo = itemSo;
        }

        public void SetAttackDamageMultiplier()
        {
            _itemSo.AttackDamageMultiplier = _itemSo.Rarity switch
            {
                Rarity.Common => _itemSo.ItemType switch
                {
                    ItemTypeEnum.Standard => 1f,
                    ItemTypeEnum.Acid => 1f,
                    _ => 1f
                },
                Rarity.Uncommon => _itemSo.ItemType switch
                {
                    ItemTypeEnum.Standard => 2f,
                    ItemTypeEnum.Acid => 1.5f,
                    _ => 1f
                },
                Rarity.Rare => _itemSo.ItemType switch
                {
                    ItemTypeEnum.Standard => 3f,
                    ItemTypeEnum.Acid => 2f,
                    _ => 1f
                },
                Rarity.Epic => _itemSo.ItemType switch
                {
                    ItemTypeEnum.Standard => 4f,
                    ItemTypeEnum.Acid => 2.5f,
                    _ => 1f
                },
                Rarity.Legendary => _itemSo.ItemType switch
                {
                    ItemTypeEnum.Standard => 5f,
                    ItemTypeEnum.Acid => 3f,
                    _ => 1f
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}