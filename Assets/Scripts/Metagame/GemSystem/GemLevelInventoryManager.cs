using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GemLevelInventoryManager : MonoBehaviour
{
    
    //Public fields
    [HideInInspector]public int LevelSlotIndex = 0;
    public int CoinPerGemFee; 


 //Private fields

 [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private GameObject[] _levelSlots;
    [SerializeField] private GameObject[] _inventorySlots;
    [SerializeField] private Libraries _library;
    [SerializeField] private GameObject _inventoryItem;
    [SerializeField] private Transform _inventorySlotParentTransform;
    [SerializeField] private Transform _levelSlotParentTransform;
    [SerializeField] private TMP_Text _atkBonusText;
    [SerializeField] private TMP_Text _hPBonusText;
    [SerializeField] private TMP_Text _moveSpeedBonusText;
    [SerializeField] private TMP_Text _coinsCostText;
    [SerializeField] private PlayOneShotSound _oneShotSound;
    private List<GameObject> _currentItemsInInventory = new();
    private GameObject[] _currentSlottedGems;
    private int LevelStartCost;
    private float _calcAtkBonus;
    private float  _calcHPBonus;
    private float _calcMoveSpeedBonus;
    
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
        for (var i = 0; i < _levelSlots.Length; i++)
        {
            if (_currentSlottedGems[i] != null) continue;
            LevelSlotIndex = i;
            break;
        }
        _currentSlottedGems[LevelSlotIndex] = gemInSlot;
        
        gemInSlot.transform.position = _levelSlots[LevelSlotIndex].transform.position;
        gemInSlot.GetComponent<LevelGemSlot>().SetItemSlot(gem);
        gemInSlot.GetComponentInChildren<TMP_Text>().SetText("");
        gemInSlot.transform.parent = _levelSlotParentTransform;
CalculateCoinCost();
        
        CalculateBonuses(gem, false);
        _oneShotSound.PlayStackingSound();
    }

    public void UnSlotGemInLevel(MaterialItem item, bool subtractionOfBonus, GameObject gemGO)
    {
        CalculateBonuses(item,subtractionOfBonus);
        ClearSlottedGemFromArray(gemGO);
        CalculateCoinCost();
    }

    public void CalculateCoinCost()
    {
        foreach (var slottedGem in _currentSlottedGems)
        {
            if (slottedGem != null)
            {
                LevelStartCost = 500;
                break;
            }
            LevelStartCost = 0;

        }

        UpdateCoinText(LevelStartCost);
    }

    private void UpdateCoinText(int cost)
    {
        _coinsCostText.SetText(LevelStartCost+" coins");
    }


    public void CalculateBonuses(MaterialItem item, bool subtraction)
    {
            if (item.GetItemID().Contains("red"))
            {
                if (subtraction)
                    _calcAtkBonus -= item.LevelBonus;
                else
                    _calcAtkBonus += item.LevelBonus;
                
            }
            else if (item.GetItemID().Contains("green"))
            {
                if (subtraction)
                    _calcHPBonus -= item.LevelBonus;
                else
                    _calcHPBonus += item.LevelBonus;
    
            }
            else if (item.GetItemID().Contains("blue"))
            {
                if (subtraction)
                    _calcMoveSpeedBonus -= item.LevelBonus;
                else
                    _calcMoveSpeedBonus += item.LevelBonus;
                
            }
            
            UpdateBonusTexts(_calcAtkBonus,_calcHPBonus,_calcMoveSpeedBonus);
        
    }

    private void UpdateBonusTexts(float atkBonus, float hPBonus, float moveSpeedBonus)
    {
        _atkBonusText.SetText("Atk bonus: " +atkBonus);
        _hPBonusText.SetText("HP bonus: " +hPBonus);
        _moveSpeedBonusText.SetText("Speed bonus: " +moveSpeedBonus);
    }

    public void ClearSlottedGemFromArray(GameObject slottedGem)
    {
        for (int i = 0; i < _currentSlottedGems.Length; i++)
        {
            if (_currentSlottedGems[i] == slottedGem)
            {
                _currentSlottedGems[i] = null;
            }
        }
    }

    public bool FreeSlotExist()
    {
        return _currentSlottedGems.Any(slottedGem => slottedGem == null);
    }
    
    private void OnDisable()
    {
        LevelStartCost = 0;
        LevelSlotIndex = 0;
        _calcHPBonus = 0;
        _calcAtkBonus = 0;
        _calcMoveSpeedBonus = 0;
        UpdateBonusTexts(_calcAtkBonus,_calcHPBonus,_calcMoveSpeedBonus);
        UpdateCoinText(LevelStartCost);

    }

    public void SaveGemModifiersOnPlay() //Called on starting level button
    {
        foreach (var slottedGem in _currentSlottedGems)
        {
            if (slottedGem== null)
                continue;
            
            
            var slottedGemData = slottedGem.GetComponent<LevelGemSlot>()._item;
            PlayerPrefs.SetFloat(slottedGemData.LevelBonusID, PlayerPrefs.GetFloat(slottedGemData.LevelBonusID)+slottedGemData.LevelBonus);
            PlayerPrefs.SetInt(slottedGemData.GetItemID(), PlayerPrefs.GetInt(slottedGemData.GetItemID()) -1);


        }
        
        PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentCoins.ToString(), PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString()) - LevelStartCost);

            _sceneLoader.LoadScene();
    }
}
