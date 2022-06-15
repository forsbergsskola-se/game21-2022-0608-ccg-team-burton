using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Meta.Gacha
{ 
    
[CreateAssetMenu(fileName = "Loot Box", menuName = "Item System/Loot Box")]
public class LootBoxSO : ScriptableObject
{
    public float TotalProbabilityWeight;
    [Space]
    public List<LootBoxData> LootTables;
    private float _pickedNumber;

    private void OnValidate()
    {
        ValidateTable();
    }

    private void ValidateTable()
    {
        if (LootTables is not {Count: > 0})
        {
            return;
        }

        var currentProbabilityWeightMax = 0f;

        foreach (LootBoxData go in LootTables)
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
        foreach (var go in LootTables)
        {
            go.ProbabilityPercent = go.ProbabilityWeight/TotalProbabilityWeight*100;
        }
    }

    public LootTableSO Pick()
    {
        _pickedNumber = Random.Range(0, TotalProbabilityWeight);
        foreach (var go in LootTables.Where(go => _pickedNumber > go.ProbabilityRangeFrom && _pickedNumber < go.ProbabilityRangeTo))
        {
            if (go == null)
            {
                Debug.LogError("ERROR: No gameobject set in table slot");
            }
            return go.lootTableSo;
        }
        Debug.LogError("GO couldn't be picked... Be sure that all of your active GameObject Tables (GO-class)  have assigned at least one GO! Returning NULL");
        //reset picked number in last method dependent on it (in this case, pick GO)
        _pickedNumber = 0;
        return null;
    }

    [Serializable]
    public class LootBoxData
    {
        public float ProbabilityWeight;
        public float ProbabilityPercent;
        [HideInInspector]
        public float ProbabilityRangeFrom;
        [HideInInspector]
        public float ProbabilityRangeTo;
        public LootTableSO lootTableSo;
    }
}

    
}
