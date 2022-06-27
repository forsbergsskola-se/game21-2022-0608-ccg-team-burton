using UnityEngine;

namespace GamePlay.Entities.Player
{
    public class CommandContainer : MonoBehaviour
    {
        public float WalkCommand;
        public bool WalkLeftCommand;
        public bool WalkRightCommand;
        
        public bool JumpUpCommand;
        public bool JumpDownCommand;
        
        public bool AttackUpCommand;
        public bool AttackDownCommand;
    }
}

