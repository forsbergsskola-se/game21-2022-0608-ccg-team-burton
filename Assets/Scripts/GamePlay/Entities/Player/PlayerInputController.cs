using UnityEngine;
namespace GamePlay.Entities.Player
{
    public class PlayerInputController : MonoBehaviour
    {
        private CommandContainer _commandContainer;
        public float WalkInput { get; private set; }
        [HideInInspector] public bool WalkLeftDownInput;
        [HideInInspector] public bool WalkLeftUpInput;
        [HideInInspector] public bool WalkRightDownInput;
        [HideInInspector] public bool WalkRightUpInput;
        [HideInInspector] public bool NoWalkInput;
        
        public bool JumpDownInput;
        public bool JumpUpInput;
        
        
        public bool AttackDownInput;
        [HideInInspector] public bool AttackUpInput;


        
        private void Awake() => _commandContainer = GetComponent<CommandContainer>();

        
        
        private void Update()
        {
            HandleInput(); // collect player inputs
            SetCommands(); // assign inputs to commands

            if (WalkLeftDownInput && WalkRightDownInput)
                NoWalkInput = false;

            if (NoWalkInput) return;
            if (!WalkLeftDownInput && !WalkRightDownInput)
                NoWalkInput = true;
        }
        
        
        
        // left and right
        private void HandleInput() => WalkInput = Input.GetAxis("Horizontal"); 



        private void SetCommands()
        {
            _commandContainer.WalkCommand = WalkInput;
            _commandContainer.WalkLeftCommand = WalkLeftDownInput;
            _commandContainer.WalkRightCommand = WalkRightDownInput;
            
            _commandContainer.JumpDownCommand = JumpDownInput;
            _commandContainer.JumpUpCommand = JumpUpInput;
            
            _commandContainer.AttackDownCommand = AttackDownInput;
            _commandContainer.AttackUpCommand = AttackUpInput;
        }
    }
}
