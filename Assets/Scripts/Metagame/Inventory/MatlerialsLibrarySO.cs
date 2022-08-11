using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Materials Library", menuName = "InventorySystem/Libraries/Materials Library")]
public class MatlerialsLibrarySO : ScriptableObject
{
    public List<MaterialItem> InventoryLibrary;
}
