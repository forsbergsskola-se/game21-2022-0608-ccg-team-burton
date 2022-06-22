using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Items;
using UnityEngine;

public class ItemSO : ScriptableObject
{
    public string ID;
    public string ItemName;
    public Sprite ItemSprite;

    
    //TODO: Outside of this SO. Would be a field for the item which is set somewhere. RaritySO -_> CommonSO contains all multipliers for different things
    public ItemRaritySO RaritySo;
    public GemSO GemSo;
}