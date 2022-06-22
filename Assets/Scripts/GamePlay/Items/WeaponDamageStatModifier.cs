public class WeaponDamageStatModifier : IStatsModifier
{
    public void ApplyStatChange(Item item, ItemSO itemSo)
    {
        if (item is not Weapon weapon) return;
        if (itemSo is not WeaponSO weaponSo) return;


        //math here for items stat change (weapons)
        weapon.WeaponDamage +=  weaponSo.RaritySo.DamageMultiplier * weaponSo.WeaponBaseDamage;

    }
}