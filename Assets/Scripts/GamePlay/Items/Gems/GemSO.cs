using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Gem", menuName = "Item System/New Gem")]
public class GemSO : ItemSO
{
    //to be saved as well
    public bool GemActive;
    public Gems GemType;


}

//Yes this is ugly. We are in a rush
public enum Gems
{
    Knockback,
    Slow,
    Stun
    
}