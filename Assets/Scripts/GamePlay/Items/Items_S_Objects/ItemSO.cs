using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Items;
using UnityEngine;

public class ItemSO : ScriptableObject
{
    public string ItemName;
    public Sprite ItemSprite;

    
    // [HideInInspector] //TODO: Outside of this SO. Would be a field for the item which is set somewhere. RaritySO -_> CommonSO contains all multipliers for different things
    public Rarity Rarity;
}