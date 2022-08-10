using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquippableLibrary", menuName = "InventorySystem/Libraries/Equipment Library")]
public class EquippableLibrarySO : ScriptableObject
{
    public List<EquipmentSO> EquipablesLibrary;
}
