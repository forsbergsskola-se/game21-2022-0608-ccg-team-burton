using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent", fileName = "New Game Event")]
public class GameEvent : ScriptableObject
{
    private HashSet<PlayerDeathListener> _listeners = new HashSet<PlayerDeathListener>();

    public void Invoke()
    {
        foreach (var globalEventListener in _listeners)
        {
            globalEventListener.RaiseEvent();
        }
    }

    public void Register(PlayerDeathListener playerDeathListener) => _listeners.Add(playerDeathListener);
    
    public void Deregister(PlayerDeathListener playerDeathListener) => _listeners.Remove(playerDeathListener);
    
}



