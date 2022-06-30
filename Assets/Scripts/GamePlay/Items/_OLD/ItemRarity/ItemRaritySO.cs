using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rarity", menuName = "Item System/New Rarity")]
public class ItemRaritySO : ScriptableObject
{
    [Tooltip("Example: rarity.common")]
    public string ID;

    public float DamageMultiplier;
    public float EffectMultiplier;
}
