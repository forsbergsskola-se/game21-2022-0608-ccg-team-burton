using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public class StructureIntegrityJJMT : MonoBehaviour, IDamageableJJMT
{

    public int Health { get; set; }
    public void ModifyHealth(int damage)
    {
        Health +=  damage;
        Debug.Log($"New structure integrity: {Health}");    
    }
}
