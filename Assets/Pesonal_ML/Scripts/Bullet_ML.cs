using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_ML : MonoBehaviour
{
    public Vector2 travelVector;
    public float lifeTime = 4;
    private float timeAlive;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += (Vector3) travelVector * Time.deltaTime;
        timeAlive += Time.deltaTime * 1;

        if (timeAlive >= lifeTime)
        {
            gameObject.SetActive(false);
        }
    }
}
