using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileTypes
{
    TileOne,
    TileTwo,
    TileThree
}

public class TileHolder_ML : MonoBehaviour
{
    [SerializeField] private GameObject tileOne;
    [SerializeField] private GameObject tileTwo;
    [SerializeField] private GameObject tileThree;

    public GameObject GetTileOfType(TileTypes type)
    {
        switch (type)
        {
            case TileTypes.TileOne:
                return tileOne;
            case TileTypes.TileTwo:
                return tileTwo;
            case TileTypes.TileThree:
                return tileThree;
        }


        return tileOne;
    }
    
}
