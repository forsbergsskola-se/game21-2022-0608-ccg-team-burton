public class ArmorEffectModification : IStatsModifier
{
    public void ApplyStatChange(Item item, ItemSO itemSo)
    {
        if (item is Armor armor)
        {
            if (itemSo is ArmorSO armorSo)
            {
                armor.EffectValue = armorSo.BaseEffect + armorSo.RarityLevelEffectIncrease * (int)armorSo.Rarity;
            }
        }
    }
}