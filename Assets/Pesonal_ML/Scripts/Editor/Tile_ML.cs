using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_ML : MonoBehaviour
{
    public Action<Vector3> SetVector;

    private void OnMouseOver()
    {
         SetVector?.Invoke(transform.position);
    }
}
