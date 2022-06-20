using Entity.Items;
using UnityEngine;
 
[CreateAssetMenu(fileName = "WeaponType", menuName = "Item System/WeaponType")]
public class ItemTypeSO : ScriptableObject
{
    public float _baseDamageMultiplier;
    public float _damageMultiplierStep;
    
    public void ScaleStats(ItemSO itemSo)
    {
        itemSo.AttackDamageMultiplier = itemSo.Rarity switch
        {
            Rarity.Common => _baseDamageMultiplier,
            Rarity.Uncommon => _baseDamageMultiplier+_damageMultiplierStep,
            Rarity.Rare =>_baseDamageMultiplier+_damageMultiplierStep*2,
            Rarity.Epic => _baseDamageMultiplier+_damageMultiplierStep*3,
            Rarity.Legendary => _baseDamageMultiplier+_damageMultiplierStep*4,

            _ => itemSo.AttackDamageMultiplier
        };
        itemSo.TotalAttackDamage = itemSo.BaseAttackDamage * itemSo.AttackDamageMultiplier;
    }
    
    
    

}
 

 