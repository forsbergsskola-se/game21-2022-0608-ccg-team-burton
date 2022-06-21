using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon", menuName = "Item System/New Weapon")]
public class WeaponSO : ItemSO
{
    public float WeaponBaseDamage;
    [Tooltip("Increase from common level, e.g. a Rare would be a modifier of 3*RarityLevelModificationStep")]
    public float RarityLevelModificationStep;
    
    //GEMSLOT FOR A WEAPONEFFECT LIKE KNOCKBACK.
}
