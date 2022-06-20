using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Items;
using UnityEngine;

public class ItemPrefab : MonoBehaviour
{
    public ItemSO ItemData;
    public Sprite ItemSprite;
    public float ItemDamage;

    public void SetItemData(ItemSO item)
    {
        ItemSprite = item.ItemSprite;
        ItemDamage = item.TotalAttackDamage;
        
        //if item.so.type == Acid {
        //var effect = Add component<AcidLogic>()
        //effect.setData(item.totaleffectvalue, item.totaleffectduration)
        //}
    }

     
}
