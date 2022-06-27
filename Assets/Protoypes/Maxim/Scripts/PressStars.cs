using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressStars : MonoBehaviour, ISaveable
{
    private int _currentStarsNum = 0;
    public int levelIndex;
    
    public void StarsButton(int _starsNum)
    {
        _currentStarsNum = _starsNum;
        if (_currentStarsNum > PlayerPrefs.GetInt("Lv" + levelIndex))
        {
            // PlayerPrefs.SetInt("Lv" + levelIndex, _starsNum );
            
        }
        SceneManager.LoadScene("UI_WorldMap_Prototype");
        
        // Debug.Log(PlayerPrefs.GetInt("Lv" + levelIndex, _starsNum ));
    }
    

    public object CaptureState()
    {
        Debug.Log("Yo we goin");
        return _currentStarsNum;
    }
    
    public void RestoreState(object state)
    {
        throw new System.NotImplementedException();
    }
}
