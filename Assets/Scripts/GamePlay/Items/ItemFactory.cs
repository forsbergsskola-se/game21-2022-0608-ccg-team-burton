public static class ItemFactory 
{
 
    public static Item CreateItemFromInventory(ItemSO itemSo) //TODO: FIX STATIC!
    {

        if (itemSo is WeaponSO weaponSo)
        {
            var weapon = new Weapon();
            weapon.ItemName = weaponSo.ItemName;
            weapon.RaritySo = weaponSo.RaritySo;
            weapon.WeaponDamage = weaponSo.WeaponBaseDamage;
            
            
            //Ugly
            //GEM addition:
            if (weaponSo.Gem != null)
            {
             
                weapon.Gem = weaponSo.Gem;
                weapon.GemActive = weaponSo.Gem.GemActive;
   
            }
            var DamageMod = new WeaponDamageStatModifier();
            DamageMod.ApplyStatChange(weapon, weaponSo);

            
            
            return weapon;
        }

        if (itemSo is ArmorSO armorSo)
        {
            var armor = new Armor();
            armor.ItemName = armorSo.ItemName;
            armor.RaritySo = armorSo.RaritySo;
            armor.EffectValue = armorSo.BaseEffect;
            
            var EffectMod = new ArmorEffectModification();
            EffectMod.ApplyStatChange(armor,armorSo);

            return armor;
        }
        

        
        return new Item();
    }
}