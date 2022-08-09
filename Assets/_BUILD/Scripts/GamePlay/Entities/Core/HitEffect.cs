using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class HitEffect : MonoBehaviour{
    
    public Transform cam;
    public float stopTime;
    public float slowTime;
    public float shake;

    Vector3 camPosOriginal;
    bool _stopping;

    public void TimeStop(){
        if (!_stopping){
            _stopping = true;
            Time.timeScale = 0;

            StartCoroutine("Stop");
            StartCoroutine("CamShake");
            StartCoroutine("CamShake");
        }
    }

    IEnumerator Stop(){
        yield return new WaitForSecondsRealtime(stopTime);
        Time.timeScale = 0.01f;

        yield return new WaitForSecondsRealtime(slowTime);
        Time.timeScale = 1;
        _stopping = false;
    }

    IEnumerator CamShake(){
        camPosOriginal = cam.position;
        cam.position =
            new Vector3(cam.position.x + Random.Range(-shake, shake),
                cam.position.y + Random.Range(-shake, shake), 
                cam.position.z + Random.Range(-shake, shake));
        yield return new WaitForSecondsRealtime(0.05f);
        cam.position =
            new Vector3(cam.position.x + Random.Range(-shake, shake),
                cam.position.y + Random.Range(-shake, shake),
                cam.position.z + Random.Range(-shake, shake));
        yield return new WaitForSecondsRealtime(0.05f);

        
        cam.position = camPosOriginal;
    }

    
}
