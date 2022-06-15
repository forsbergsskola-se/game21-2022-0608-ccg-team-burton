using GamePlay.Entities.Player;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    private CommandContainer commandContainer;
    private Transform _playerPos;
    public float PlayerNearDistance = 30f;

    private void Start()
    {
        commandContainer = GetComponentInChildren<CommandContainer>();
        _playerPos = FindObjectOfType<PlayerInputController>().gameObject.transform;
    }
    
    private void Update()
    {
        var proximityToPlayer = Mathf.Abs(_playerPos.position.x - transform.position.x);

        // only moves if close enough to Player
        if (proximityToPlayer > PlayerNearDistance)
        {
            commandContainer.WalkCommand = 0;
            return;
        }
        
        var directionToPlayer = (_playerPos.position - transform.position).normalized;
        var horizontalDirectionToPlayer = directionToPlayer.x;

        commandContainer.WalkCommand = horizontalDirectionToPlayer;
    }
}