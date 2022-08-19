using UnityEngine;
using UnityEngine.Events;

public class PlayerDeathListener : MonoBehaviour
{
    [SerializeField] GameEvent _gameEvent;
    [SerializeField] UnityEvent _unityEvent;
    private LevelCompleted _levelCompleted;
    

    void Start()
    {
        _gameEvent.Register(playerDeathListener:this);
        Debug.Log("Registered");
        gameObject.SetActive(false);
        _levelCompleted = FindObjectOfType<LevelCompleted>().GetComponent<LevelCompleted>();
    }

    void OnDestroy()
    {
        _gameEvent.Deregister(playerDeathListener:this);
    }

    public void RaiseEvent()
    {
        _unityEvent.Invoke();
    }

    public void PauseTimer() => _levelCompleted.PauseTimer = true;
}
