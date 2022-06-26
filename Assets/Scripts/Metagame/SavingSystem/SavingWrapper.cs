using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attaches to the GameObject "Saving System"
public class SavingWrapper : MonoBehaviour
{
    const string defaultSaveFile = " save";

    private void Awake()
    {
        StartCoroutine(LoadLastScene());    
    }
    IEnumerator LoadLastScene()
    {
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
    }


    void Update()
    {
        //Call Load or Save when needed
    }

    public void Load()
    {
        GetComponent<SavingSystem>().Load(defaultSaveFile);
    }
    public void Save()
    {
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }
}
