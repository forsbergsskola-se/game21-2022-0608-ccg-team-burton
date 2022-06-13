using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ML : MonoBehaviour
{
    private Button saveButton;
    private TMP_InputField field;
    private string _saveName;
    
    void Start()
    {
        saveButton = GetComponentsInChildren<Button>()[0];
        saveButton.onClick.AddListener(SaveStuff);
        field = GetComponentInChildren<TMP_InputField>();
        field.onValueChanged.AddListener(SetName);
    }


    private void SetName(string saveName)
    {
        _saveName = saveName;
    }

    private void OnDisable()
    {
        saveButton.onClick.RemoveAllListeners();
        field.onValueChanged.RemoveAllListeners();
    }

    private void SaveStuff()
    {
       var tiles =  GameObject.FindGameObjectsWithTag("ATile");
       var saveFile = new List<SaveClass>();

       foreach (var t in tiles)
       {
           var aPos = t.gameObject.transform.position;
           var save = new SaveClass()
           {
               PositionX = aPos.x,
               PositionY = aPos.y
           };
           
           saveFile.Add(save);
       }
       
       var output = JsonConvert.SerializeObject(saveFile);
       
       Debug.Log(output);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
