using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeathListener : MonoBehaviour
{
    [SerializeField] GameEvent _gameEvent;
    [SerializeField] UnityEvent _unityEvent;

    void Awake()
    {
        _gameEvent.Register(playerDeathListener:this);
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        _gameEvent.Deregister(playerDeathListener:this);
    }

    public void RaiseEvent()
    {
        _unityEvent.Invoke();
    }
}
