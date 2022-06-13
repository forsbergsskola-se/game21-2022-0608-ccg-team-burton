using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_ML : MonoBehaviour
{
    private Button saveButton;
    private Button loadButton;
    private TMP_InputField field;
    private string _saveName;
    
    void Start()
    {
        saveButton = GetComponentsInChildren<Button>()[0];
        loadButton = GetComponentsInChildren<Button>()[1];
        saveButton.onClick.AddListener(SaveStuff);
        loadButton.onClick.AddListener(LoadStuff);
        
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
        loadButton.onClick.RemoveAllListeners();
        field.onValueChanged.RemoveAllListeners();
    }

    private void LoadStuff()
    {
        var path = Application.dataPath + "/Resources/EditorSaveFiles/" + _saveName + ".txt";
    
        if (!File.Exists(path))
        {
            Debug.Log("No such file");
            return;
        }

        DeleteAllTiles();
        
        var output = JsonConvert
            .DeserializeObject<List<SaveClass>>(File.ReadAllText(path));
    }


    private void DeleteAllTiles()
    {
        var tiles =  GameObject.FindGameObjectsWithTag("ATile");

        foreach (var t in tiles)
        {
            Destroy(t);
        }
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
       if (_saveName == "")
       {
           _saveName = "DEFAULT";
       }
       
       var path = Application.dataPath + "/Resources/EditorSaveFiles/" + _saveName + ".txt";
     
       File.WriteAllText(path, output);
    }
    
}
