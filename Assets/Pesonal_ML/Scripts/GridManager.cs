using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private float tileSize;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Vector3 startPos;
    
    void Start()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
               var thePre = Instantiate(tilePrefab, new Vector3(i, j) + startPos, Quaternion.identity);
               thePre.transform.Rotate(new Vector3(1,0,0) , -90);
               thePre.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }    
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
