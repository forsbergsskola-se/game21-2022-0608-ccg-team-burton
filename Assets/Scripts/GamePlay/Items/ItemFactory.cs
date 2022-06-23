public static class ItemFactory 
{
 
    public static Item CreateItemFromInventory(ItemSO itemSo, ItemRaritySO itemRaritySo, GemSO gemSo) //TODO: FIX STATIC!
    {

        if (itemSo is WeaponSO weaponSo)
        {
            var weapon = new Weapon();
            weapon.ItemName = weaponSo.ItemName;
            weapon.RaritySo = itemRaritySo;
            weapon.WeaponDamage = weaponSo.WeaponBaseDamage;
            
            
            //Ugly
            //GEM addition:
            if (gemSo != null)
            {
             
                weapon.Gem = gemSo;
                weapon.GemActive = weaponSo.GemSo.GemActive;
   
            }
            var DamageMod = new WeaponDamageStatModifier();
            DamageMod.ApplyStatChange(weapon, weaponSo);
 
            
            return weapon;
        }

        if (itemSo is ArmorSO armorSo)
        {
            var armor = new Armor();
            armor.ItemSprite = armorSo.ItemSprite;
            armor.ItemName = armorSo.ItemName;
            armor.RaritySo = itemRaritySo;
            armor.EffectValue = armorSo.BaseEffect;
            if (gemSo != null)
            {
             
                armor.Gem = gemSo;
                armor.GemActive = armorSo.GemSo.GemActive;
   
            }
            var EffectMod = new ArmorEffectModification();
            EffectMod.ApplyStatChange(armor,armorSo);

            return armor;
        }
        

        
        return new Item();
    }
}