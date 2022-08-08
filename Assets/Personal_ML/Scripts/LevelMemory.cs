using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMemory : ScriptableObject
{
    public List<List<CubeFacts>> GridList = new();
    public bool fullyScanned;
}
