using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gem Library", menuName = "Libraries/Gem Library")]
public class GemLibrarySO : ScriptableObject
{
    public List<ActionItem> GemLibrary;
}
