using TMPro;
using UnityEngine;

public class TeleportCheat : MonoBehaviour
{
    public bool Active;
    public int CheatCoins;
    public GameObject Player;
    public GameObject Goal;
    public TextMeshProUGUI coinText;
    public ItemCollector ItemCollector;

    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = Active;
        gameObject.GetComponent<CircleCollider2D>().enabled = Active;
        if (!Active) return;
            transform.position = Player.transform.position + new Vector3(0, 4.5f, 0);
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        ItemCollector._coinCounter += CheatCoins;
        coinText.text = ItemCollector._coinCounter.ToString();
        Player.transform.position = Goal.transform.position + new Vector3(-10, 0.5f, 0);
    }
}
