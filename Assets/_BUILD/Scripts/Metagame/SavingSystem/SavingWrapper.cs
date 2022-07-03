using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attaches to the GameObject "Saving System"
public class SavingWrapper : MonoBehaviour
{
    const string defaultSaveFile = " save";

    private void Awake()
    {
        //StartCoroutine(LoadLastScene());    
    }
    IEnumerator LoadLastScene()
    {
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }

    public void Load()
    {
        GetComponent<SavingSystem>().Load(defaultSaveFile);
    }
    public void Save()
    {
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }

    public void Delete()
    {
        GetComponent<SavingSystem>().Delete(defaultSaveFile);
    }
}
