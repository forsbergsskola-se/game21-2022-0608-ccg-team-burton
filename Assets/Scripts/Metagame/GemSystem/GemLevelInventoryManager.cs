using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GemLevelInventoryManager : MonoBehaviour
{
    
    //Public fields
    public int _levelSlotIndex = 0;

    //Private fields
    [SerializeField] private GameObject[] _levelSlots;
    [SerializeField] private GameObject[] _inventorySlots;
    [SerializeField] private Libraries _library;
    [SerializeField] private GameObject _inventoryItem;
    [SerializeField] private Transform _inventorySlotParentTransform;
    [SerializeField] private Transform _levelSlotParentTransform;
    [SerializeField] private TMP_Text _atkBonusText;
    [SerializeField] private TMP_Text _hPBonusText;
    [SerializeField] private TMP_Text _moveSpeedBonusText;
    [SerializeField] private PlayOneShotSound _oneShotSound;
    private List<GameObject> _currentItemsInInventory = new();
    private GameObject[] _currentSlottedGems;
    
    private void Start()
    {
        _currentSlottedGems = new GameObject[_levelSlots.Length];
    }

    private void OnEnable()
    {
        LoadGemsFromInventory();   

    }

    public void LoadGemsFromInventory()
    {
        var inventorySlotIndex = 0;
        
        DestroyCurrentItemsInInventory();
        
        foreach (var materialItem in _library.MatlerialsLibrarySo.Materials)
        {
            if (materialItem.GetItemID().Contains("red")||materialItem.GetItemID().Contains("blue")||materialItem.GetItemID().Contains("green"))
            {
                if (SetItemOnSlot(materialItem, ref inventorySlotIndex)) ;
            }
        }
    }

    private bool SetItemOnSlot(MaterialItem materialItem, ref int inventorySlotIndex)
    {
        if (PlayerPrefs.GetInt(materialItem.GetItemID()) <= 0)
            return true;

        var gemInSlot = Instantiate(_inventoryItem, _inventorySlots[inventorySlotIndex].transform.position, Quaternion.identity);
        _currentItemsInInventory.Add(gemInSlot);
        gemInSlot.transform.parent = _inventorySlotParentTransform;
        gemInSlot.GetComponent<LevelGemSlot>().SetItemSlot(materialItem);
        inventorySlotIndex++;
        return false;
    }

    private void DestroyCurrentItemsInInventory()
    {
        foreach (var currentItem in _currentItemsInInventory)
        {
            Destroy(currentItem);
        }
    }

    public void SlotGemInLevel(MaterialItem gem)
    {
        
        if (!FreeSlotExist())
            return;


        var gemInSlot = Instantiate(_inventoryItem, Vector2.zero, Quaternion.identity);
        for (int i = 0; i < _levelSlots.Length; i++)
        {
            if (_currentSlottedGems[i] != null) continue;
            _levelSlotIndex = i;
            break;
        }
        _currentSlottedGems[_levelSlotIndex] = gemInSlot;
        
        gemInSlot.transform.position = _levelSlots[_levelSlotIndex].transform.position;
        gemInSlot.GetComponent<LevelGemSlot>().SetItemSlot(gem);
        gemInSlot.GetComponentInChildren<TMP_Text>().SetText("");
        gemInSlot.transform.parent = _levelSlotParentTransform;
        
        CalculateBonuses(gem, false);
        _oneShotSound.PlaySound();
    }

   float atkBonus = 0f;
        float  hPBonus = 0f;
        float moveSpeedBonus = 0f;
    public void CalculateBonuses(MaterialItem item, bool subtraction)
    {
     
            if (item.GetItemID().Contains("red"))
            {
                if (subtraction)
                    atkBonus -= item.LevelBonus;
                else
                    atkBonus += item.LevelBonus;
                
            } else if (item.GetItemID().Contains("green"))
            {
                if (subtraction)
                    hPBonus -= item.LevelBonus;
                else
                    hPBonus += item.LevelBonus;
    
            } else if (item.GetItemID().Contains("blue"))
            {
                if (subtraction)
                    moveSpeedBonus -= item.LevelBonus;
                else
                    moveSpeedBonus += item.LevelBonus;
                
            }
            
            UpdateBonusTexts(atkBonus,hPBonus,moveSpeedBonus);
        
    }

    public void UpdateBonusTexts(float atkBonus, float hPBonus, float moveSpeedBonus)
    {
        _atkBonusText.SetText("Atk bonus: " +atkBonus);
        _hPBonusText.SetText("HP bonus: " +hPBonus);
        _moveSpeedBonusText.SetText("Move speed bonus: " +moveSpeedBonus);
    }
    
    

    public bool FreeSlotExist()
    {
        return _currentSlottedGems.Any(slottedGem => slottedGem == null);
    }
    
    private void OnDisable()
    {
        _levelSlotIndex = 0;
        hPBonus = 0;
        atkBonus = 0;
        moveSpeedBonus = 0;
        UpdateBonusTexts(atkBonus,hPBonus,moveSpeedBonus);
    }

    public void SaveGemModifiersOnPlay() //Called on starting level button
    {
        foreach (var slottedGem in _currentSlottedGems)
        {
            var slottedGemData = slottedGem.GetComponent<LevelGemSlot>()._item;
            PlayerPrefs.SetFloat(slottedGemData.LevelBonusID, PlayerPrefs.GetFloat(slottedGemData.LevelBonusID)+slottedGemData.LevelBonus);
            PlayerPrefs.SetInt(slottedGemData.GetItemID(), PlayerPrefs.GetInt(slottedGemData.GetItemID()) -1);
        }
    }

}
