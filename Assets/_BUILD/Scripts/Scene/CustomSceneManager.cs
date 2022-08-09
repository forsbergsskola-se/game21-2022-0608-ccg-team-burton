using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    [SerializeField] int _index;

    public int Index
    {
        
        get { return _index; }
    }

    public void DoCoroutine()
    {
        StartCoroutine(Transite());
    }

    private IEnumerator Transite()
    {
        if(Index < 0)
        {
            Debug.LogError("Scene to load is not Set!");
            yield break;
        }
        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(_index);
        print("Scene Loaded");
        Destroy(gameObject);
    }
}
