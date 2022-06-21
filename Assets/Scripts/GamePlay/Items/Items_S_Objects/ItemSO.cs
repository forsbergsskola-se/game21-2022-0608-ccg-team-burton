using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Items;
using UnityEngine;

public class ItemSO : ScriptableObject
{
    public string ItemName;
    [HideInInspector]
    public Rarity Rarity;

    // private void OnValidate()
    // {
    //     Rarity = Rarity.Common;
    // }
}