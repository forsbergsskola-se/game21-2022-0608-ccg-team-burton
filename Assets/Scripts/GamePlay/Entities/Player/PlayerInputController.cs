using System;
using UnityEngine;
namespace GamePlay.Entities.Player
{
    public class PlayerInputController : MonoBehaviour
    {
        private CommandContainer _commandContainer;
        public float WalkInput { get; private set; }

        public bool WalkLeftDownInput;
        public bool WalkLeftUpInput;
        public bool WalkRightDownInput;
        public bool WalkRightUpInput;
        public bool NoWalkInput;

        
        
        public bool JumpDownInput;
        public bool JumpUpInput;
        
        public bool AttackDownInput;
        public bool AttackUpInput;


        
        private void Awake()
        {
            _commandContainer = GetComponent<CommandContainer>();
        }

        
        
        private void Update()
        {
            HandleInput(); // collect player inputs
            SetCommands(); // assign inputs to commands

            if (WalkLeftDownInput && WalkRightDownInput)
                NoWalkInput = false;

            if (NoWalkInput) return;
            if (!WalkLeftDownInput && !WalkRightDownInput)
            {
                NoWalkInput = true;
                Debug.Log("Not walking");
            }
        }
        
        

        private void HandleInput()
        {
            WalkInput = Input.GetAxis("Horizontal"); // left and right
        }

        
        
        private void SetCommands()
        {
            _commandContainer.WalkCommand = WalkInput;
            _commandContainer.JumpDownCommand = JumpDownInput;
            _commandContainer.JumpUpCommand = JumpUpInput;
            _commandContainer.AttackDownCommand = AttackDownInput;
            _commandContainer.AttackUpCommand = AttackUpInput;
        }
    }
}
