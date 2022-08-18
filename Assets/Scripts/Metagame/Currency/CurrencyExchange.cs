using FMODUnity;
using TMPro;
using UnityEngine;

public class CurrencyExchange : MonoBehaviour
{
    [SerializeField] private int _coinCost;
    [SerializeField] private int _buttonsGained;

    [SerializeField] private TMP_Text _coinsTextOnButton;
    [SerializeField] private TMP_Text _buttonsRecivedText;
    [SerializeField] private TMP_Text _coinsUIText;
    [SerializeField] private TMP_Text _buttonsUIText;

    [SerializeField] private PlayOneShotSound _oneShotSound;
    

    private void Start()
    {
        _coinsTextOnButton.SetText(_coinCost.ToString() +" Coins");
        _buttonsRecivedText.SetText(_buttonsGained.ToString());
        
    }

    public void ExchangeCoinsForButtons()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString()) < _coinCost)
            return;
        
        PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentCoins.ToString(), PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString()) - _coinCost);
        PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentButtons.ToString(), PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentButtons.ToString()) + _buttonsGained);
        
        
        _coinsUIText.SetText(PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoins.ToString()).ToString());
        _buttonsUIText.SetText(PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentButtons.ToString()).ToString());
        
        _oneShotSound.PlaySound();
        
    }
    

}
