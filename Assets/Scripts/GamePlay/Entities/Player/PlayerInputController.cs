using UnityEngine;
namespace GamePlay.Entities.Player
{
    public class PlayerInputController : MonoBehaviour
    {
        private CommandContainer _commandContainer;
        public float WalkInput { get; private set; }
        public bool JumpDownInput;
        public bool JumpUpInput;

        
        private void Awake()
        {
            _commandContainer = GetComponent<CommandContainer>();
        }

        
        
        private void Update()
        {
            HandleInput(); // collect player inputs
            SetCommands(); // assign inputs to commands
        }
        
        
        
        private void HandleInput()
        {
            WalkInput = Input.GetAxis("Horizontal"); // left and right
            JumpDownInput = Input.GetKeyDown(KeyCode.Space); // jump pressed
            JumpUpInput = Input.GetKey(KeyCode.Space); // jump released
        }

        
        
        private void SetCommands()
        {
            _commandContainer.WalkCommand = WalkInput;
            _commandContainer.JumpDownCommand = JumpDownInput;
            _commandContainer.JumpUpCommand = JumpUpInput;
        }
    }
}
