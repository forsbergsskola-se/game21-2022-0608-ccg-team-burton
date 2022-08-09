using System.Collections;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wait : MonoBehaviour{
    public float waitTime;

    void Start(){
        StartCoroutine(WaitForIntro());
    }

    IEnumerator WaitForIntro(){
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(1);
    }
}
