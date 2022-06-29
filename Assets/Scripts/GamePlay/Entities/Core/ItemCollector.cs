using System;
using TMPro;
using UnityEngine;

public class ItemCollector : MonoBehaviour{

    [SerializeField] public TextMeshProUGUI coinText;
    [SerializeField] public int coinValue;
    [HideInInspector] public int _coinCounter = 0;

    private SoundMananger _soundManager;
    public FMODUnity.EventReference CollectCoinSoundFile;
    private FMOD.Studio.EventInstance _collectCoinSound;

    private void Awake()
    {
        _soundManager = FindObjectOfType<SoundMananger>();
    }

    private void Start()
    {
        _collectCoinSound = FMODUnity.RuntimeManager.CreateInstance(CollectCoinSoundFile);

    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Coin")) return;
        
        Destroy(col.gameObject);
        _soundManager.PlayStackingSound(_collectCoinSound);
        _coinCounter +=  coinValue;
        UpdateCoinText(_coinCounter);
    }

    public void UpdateCoinText(int value) => coinText.text = "Coins: " + value ;
}
