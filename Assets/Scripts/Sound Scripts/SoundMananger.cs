using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMananger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DoThing();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DoThing()
    {
        Debug.Log("DoThing");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game play/Pick up currency");
    }




} 
