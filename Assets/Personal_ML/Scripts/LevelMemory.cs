using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelMemory : ScriptableObject
{
    public List<List<CubeFacts>> GridList = new();
    public bool fullyScanned;
}
