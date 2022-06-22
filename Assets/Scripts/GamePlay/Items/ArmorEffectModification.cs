public class ArmorEffectModification : IStatsModifier
{
    public void ApplyStatChange(Item item, ItemSO itemSo)
    {
        if (item is Armor armor)
        {
            if (itemSo is ArmorSO armorSo)
            {
                //math here for items stat change (armor)
                armor.EffectValue += armorSo.BaseEffect * armorSo.RaritySo.EffectMultiplier;
            }
        }
    }
}