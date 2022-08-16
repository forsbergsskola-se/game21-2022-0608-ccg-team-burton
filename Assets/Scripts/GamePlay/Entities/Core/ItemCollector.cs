using FMOD.Studio;
using FMODUnity;
using TMPro;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI coinText;
    [SerializeField] public int coinValue;
    [HideInInspector] public int _coinCounter;
    public EventReference CollectCoinSoundFile;
    EventInstance _collectCoinSound;


    SoundMananger _soundManager;

    void Awake(){
        _soundManager = FindObjectOfType<SoundMananger>();
    }

    void Start(){
        _collectCoinSound = RuntimeManager.CreateInstance(CollectCoinSoundFile);
        UpdateCoinText(0);
    }


    void OnTriggerEnter2D(Collider2D col){
        if (!col.gameObject.CompareTag("Coin")) return;

        Destroy(col.gameObject);
        _soundManager.PlayStackingSound(_collectCoinSound);
        _coinCounter += coinValue;
        UpdateCoinText(_coinCounter);
    }


    public void UpdateCoinText(int value){
        coinText.text = $"{value}";
    }
}