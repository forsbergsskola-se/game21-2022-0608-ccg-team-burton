using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entity.Items;
using Meta.Gacha;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "ItemTable", menuName = "Item System/ItemTable")]
public class ItemTableSO : ScriptableObject
{
    public Rarity tableRarity;
  public float TotalProbabilityWeight;
    [Space]
    public List<ItemData> ItemsList;
    private float _pickedNumber;
    
    private void OnValidate()
    {
        ValidateTable();
    }

    private void ValidateTable()
    {
        if (ItemsList is not {Count: > 0})
        {
            return;
        }

        var currentProbabilityWeightMax = 0f;

        foreach (ItemData go in ItemsList)
        {
            if (go.ProbabilityWeight < 0)
            {
                Debug.Log("You can't have negative probability weight!");
                go.ProbabilityWeight = 0f;
            }
            else
            {
                go.ProbabilityRangeFrom = currentProbabilityWeightMax;
                currentProbabilityWeightMax += go.ProbabilityWeight;
                go.ProbabilityRangeTo = currentProbabilityWeightMax;
            }
        }
        TotalProbabilityWeight = currentProbabilityWeightMax;
        foreach (var go in ItemsList)
        {
            go.ProbabilityPercent = go.ProbabilityWeight/TotalProbabilityWeight*100;
        }
    }

    public ItemSO PickItem()
    {
        _pickedNumber = Random.Range(0, TotalProbabilityWeight);
        foreach (var go in ItemsList.Where(go => _pickedNumber > go.ProbabilityRangeFrom && _pickedNumber < go.ProbabilityRangeTo))
        {
            if (go == null)
            {
                Debug.LogError("ERROR: No Object set in table slot");
            }

            go.ItemSO.Rarity = tableRarity;
            go.ItemSO.SetData();
            
            
            return go.ItemSO;
        }
        Debug.LogError("GO couldn't be picked... Be sure that all of your active GameObject Tables (GO-class)  have assigned at least one GO! Returning NULL");
        //reset picked number in last method dependent on it (in this case, pick GO)
        _pickedNumber = 0;
        return null;
    }

     

    [Serializable]
    public class ItemData
    {
        public float ProbabilityWeight;
        public float ProbabilityPercent;
        [HideInInspector]
        public float ProbabilityRangeFrom;
        [HideInInspector]
        public float ProbabilityRangeTo;
        public ItemSO ItemSO;
    }  
   
}
