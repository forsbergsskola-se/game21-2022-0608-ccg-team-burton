using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Library", menuName = "Libraries/New Item Library")]
public class ItemLibrarySO : ScriptableObject
{
 //Should be dictionary
   [SerializeField]
   public List<ItemSO> ItemsLibrary;
}
 
