using System.Collections;
using System.Collections.Generic;
using Protoypes.Harry;
using UnityEngine;

public class TimeLineHandler : MonoBehaviour
{
    [SerializeField] private GameObject _timeLine;
    [SerializeField] private NewMovement _playerMovement;
    [SerializeField] float _secondsToActivate;

    void Start()
    {
        //If timeline has run once this level
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.TimeLineActive.ToString()) == 0)
        {
            _timeLine.SetActive(true);
            StartCoroutine(ActivationRoutine());
            return;
        }

        //If timeline has been run on this level previously
        _playerMovement.enabled = true;

    }
    
    private IEnumerator ActivationRoutine()
    {
        //Wait for 14 secs. 
        yield return new WaitForSeconds(_secondsToActivate);
        _playerMovement.enabled = true;
        PlayerPrefs.SetInt(PlayerPrefsKeys.TimeLineActive.ToString(), 1);
 
    }

    public void ResetTimelineActive()
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.TimeLineActive.ToString(), 0);
    }
}
