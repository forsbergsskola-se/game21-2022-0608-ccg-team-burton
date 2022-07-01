using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemRarity Library", menuName = "Libraries/New Item Rarity Library")]
public class ItemRarityLibrarySO : ScriptableObject
{
    public List<ItemRaritySO> ItemRarityLibrary;
}
