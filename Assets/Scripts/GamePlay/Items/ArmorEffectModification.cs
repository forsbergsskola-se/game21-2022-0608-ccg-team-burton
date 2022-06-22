public class ArmorEffectModification : IStatsModifier
{
    public void ApplyStatChange(Item item, ItemSO itemSo)
    {
        if (item is Armor armor)
        {
            if (itemSo is ArmorSO armorSo)
            {
                armor.EffectValue += armorSo.RarityLevelEffectIncrease * armorSo.RaritySo.EffectMultiplier;
            }
        }
    }
}