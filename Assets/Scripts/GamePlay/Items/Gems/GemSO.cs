using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Gem", menuName = "Item System/New Gem")]
public class GemSO : ScriptableObject
{
    //to be saved as well
    public string ID;
    public bool GemActive;
    public GemType GemType;
}

