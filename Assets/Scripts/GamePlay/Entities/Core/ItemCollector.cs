using FMOD.Studio;
using FMODUnity;
using TMPro;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI coinText;
    [SerializeField] public int coinValue;
    [SerializeField] PlayOneShotSound _oneShotSound;

    [HideInInspector] public int _coinCounter;
    [SerializeField] EventReference CollectCoinSoundFile;




    void Awake(){
    }

    void Start(){
        UpdateCoinText(0);
    }


    void OnTriggerEnter2D(Collider2D col){
        if (!col.gameObject.CompareTag("Coin")) return;

        Destroy(col.gameObject);
        _oneShotSound.PlayStackingSound(CollectCoinSoundFile);
        _coinCounter += coinValue;
        UpdateCoinText(_coinCounter);
    }


    public void UpdateCoinText(int value){
        coinText.text = $"{value}";
    }
}