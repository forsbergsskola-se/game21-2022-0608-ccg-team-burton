public class WeaponDamageStatModifier : IStatsModifier
{
    public void ApplyStatChange(Item item, ItemSO itemSo)
    {
        if (item is not Weapon weapon) return;
        if (itemSo is not WeaponSO weaponSo) return;


        //TODO: Rarity is an SO with multipliers
        
        weapon.WeaponDamage +=  (int) weaponSo.Rarity * weaponSo.WeaponBaseDamage; // scaling with int enum bad??

    }
}