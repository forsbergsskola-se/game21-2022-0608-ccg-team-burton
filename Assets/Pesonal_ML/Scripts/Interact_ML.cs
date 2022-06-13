using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_ML : MonoBehaviour
{
    [SerializeField] private float depth;

    [SerializeField] private GameObject testObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var inputRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            SpawnAThing();
        }
        
    }

    private void SpawnAThing()
    {
        var aPlace  = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,800);
        Debug.Log(aPlace);
        var placePos = Input.mousePosition + new Vector3(0, 0, 0);
        Instantiate(testObject, aPlace, Quaternion.identity);
    }
}
