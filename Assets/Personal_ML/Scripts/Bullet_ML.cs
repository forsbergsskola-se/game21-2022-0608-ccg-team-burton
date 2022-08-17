using Entity;
using UnityEngine;

public class Bullet_ML : MonoBehaviour
{
    public Vector2 travelVector;
    public float timeAlive;
    public int damageAmount;
    public float moveSpeed;
    public float maxLifespan;

    private void Start()
    {
        timeAlive = 0;
    }
    

     private void OnTriggerEnter2D(Collider2D col)
    {
        var layer = col.transform.gameObject.layer;
        
        if (layer == 8)
        {
            col.gameObject.GetComponent<IDamageable>().ModifyHealth(-damageAmount);
            gameObject.SetActive(false);
        }
      
    }

    void Update()
    {
        gameObject.transform.position += (Vector3) travelVector * (Time.deltaTime * moveSpeed);
        timeAlive += Time.deltaTime;

        if (timeAlive >= maxLifespan)
        {
            gameObject.SetActive(false);
        }
    }
}
