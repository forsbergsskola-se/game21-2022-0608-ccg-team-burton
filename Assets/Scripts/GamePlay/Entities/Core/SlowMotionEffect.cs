using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionEffect : MonoBehaviour{
    [SerializeField] float _slowMotionTimeScale;

    float _startTimeScale;
    float _startFixedDeltaTime;

    void Start(){
        _startTimeScale = Time.timeScale;
        _startFixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update(){
        
    }

    public void StartSlowMotion(){
        Time.timeScale = _slowMotionTimeScale;
        Time.fixedDeltaTime = _startFixedDeltaTime * _slowMotionTimeScale;
        Debug.Log("SlowMotion: Start");
    }

    public void StopSlowMotion(){
        Time.timeScale = _startTimeScale;
        Time.fixedDeltaTime = _startFixedDeltaTime;
        Debug.Log("SlowMotion: Stop");
    }
}
