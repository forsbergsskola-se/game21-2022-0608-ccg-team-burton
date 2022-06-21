using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTest : MonoBehaviour
{
    private Transform cannonTrans;
    private Transform shootVec;
    private ArcCollider collider;
    private float delay;
    private float delayMax = 0.5f;
    private bool playerSeen;
    private float rotMod = 2;
    private bool canMove = true;
    [SerializeField] private EnemyVarsTurret vars;
    void Start()
    {
        cannonTrans = GetComponent<Transform>();
        shootVec = GetComponentInChildren<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        int layermask = 1 << 8;
     
        if (cannonTrans.rotation.z * 100 > 45 && canMove)
        {
            rotMod = -1;
         
        }
        else if(cannonTrans.rotation.z * 100 < -45 && canMove)
        {
            rotMod = 1;
        }

        cannonTrans.Rotate(new Vector3(0,0,1), Time.deltaTime * rotMod);

        if (Physics2D.Raycast(shootVec.position, transform.right, 30, layermask))
        {
            Debug.DrawRay(shootVec.position, transform.right *100, Color.black);
            rotMod = 0;
            canMove = false;
        }
        else
        {
            Debug.DrawRay(shootVec.position, transform.right *100, Color.gray);
            rotMod = 1;
            canMove = true;
        }
        
        if (delay > delayMax)
        {
          //  collider.CalculatePoints();
            delay -= delayMax;
        }
        
        delay += Time.deltaTime * 1;
    }
}
