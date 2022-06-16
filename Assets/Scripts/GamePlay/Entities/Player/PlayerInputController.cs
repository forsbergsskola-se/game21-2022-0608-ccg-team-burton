using UnityEngine;

namespace GamePlay.Entities.Player
{
    public class PlayerInputController : MonoBehaviour
    {
        private CommandContainer _commandContainer;
        public float WalkInput { get; private set; }
        //public bool JumpInputDown { get; private set; }
        //public bool JumpInputUp { get; private set; }
        public bool JumpDownInput { get; private set; }
        public bool JumpUpInput { get; private set; }


        
        private void Awake()
        {
            _commandContainer = GetComponent<CommandContainer>();
        }

        
        private void Update()
        {
            HandleInput();
            SetCommands();
        }
        
        
        private void HandleInput()
        {
            WalkInput = Input.GetAxis("Horizontal");
            //JumpInputDown = Input.GetKeyDown(KeyCode.Space);
            //JumpInputUp = Input.GetKeyUp(KeyCode.Space);
            JumpDownInput = Input.GetKeyDown(KeyCode.Space);
            JumpUpInput = Input.GetKey(KeyCode.Space);

        }

        
        private void SetCommands()
        {
            _commandContainer.WalkCommand = WalkInput;
            _commandContainer.JumpDownCommand = JumpDownInput;
            _commandContainer.JumpUpCommand = JumpUpInput;
        }
    }
}
