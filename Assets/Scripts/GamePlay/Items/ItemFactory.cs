public static class ItemFactory 
{
 
    public static Item CreateItemFromInventory(ItemSO itemSo) //TODO: FIX STATIC!
    {

        if (itemSo is WeaponSO weaponSo)
        {
            var weapon = new Weapon();
            weapon.ItemName = weaponSo.ItemName;
            weapon.Rarity = weaponSo.Rarity;
            weapon.WeaponDamage = weaponSo.WeaponBaseDamage;
            
            var DamageMod = new WeaponDamageStatModifier();
            DamageMod.ApplyStatChange(weapon, weaponSo);  
            
            
            return weapon;
        }

        if (itemSo is ArmorSO armorSo)
        {
            var armor = new Armor();
            armor.ItemName = armorSo.ItemName;
            armor.Rarity = armorSo.Rarity;
            armor.EffectValue = armorSo.BaseEffect;
            var EffectMod = new ArmorEffectModification();
            EffectMod.ApplyStatChange(armor,armorSo);

            return armor;
        }
        

        
        return new Item();
    }
}