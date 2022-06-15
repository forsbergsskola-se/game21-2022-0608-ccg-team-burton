using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Items;
using UnityEngine;

namespace Entity.Items
{
 
    [CreateAssetMenu(fileName = "New Item", menuName = "Item System/New Item")]
    public class ItemSO : ScriptableObject
    {
        public string ItemName;
        public Rarity Rarity;
        public float DropWeight;


        // private void OnValidate()
        // {
        //     if (this.Rarity == Rarity.Common)
        //     {
        //         DropWeight = 400;
        //     }
        // }


    }   
}
