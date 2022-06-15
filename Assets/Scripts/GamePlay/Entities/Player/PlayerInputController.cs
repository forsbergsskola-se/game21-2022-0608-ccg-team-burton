using UnityEngine;

namespace GamePlay.Entities.Player
{
    public class PlayerInputController : MonoBehaviour
    {
        private CommandContainer _commandContainer;
        
        public float WalkInput { get; private set; }
        public bool WalkLeftInput { get; private set; }
        public bool WalkRightInput { get; private set; }

        public bool JumpInputDown { get; private set; }
        public bool JumpInputUp { get; private set; }
        public bool JumpInput { get; private set; }
        private bool _noInput;

        
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
            
            JumpInputDown = Input.GetKeyDown(KeyCode.Space);
            JumpInputUp = Input.GetKeyUp(KeyCode.Space);
            JumpInput = Input.GetKey(KeyCode.Space);
            
            
            /*switch (WalkRightInput)
            {
                case true when WalkLeftInput: // pressing both keeps player still
                    WalkInput = 0;
                    _noInput = false;
                    break;
                
                case true:
                    WalkInput = 1;
                    _noInput = false;
                    break;
                
                default:
                {
                    if (WalkLeftInput)
                    {
                        WalkInput = -1;
                        _noInput = false;
                    }
                    
                    else
                        _noInput = true;
                    break;
                }
            }*/
        }

        
        private void SetCommands()
        {
            _commandContainer.walkCommand = WalkInput;
            _commandContainer.walkLeftCommand = WalkLeftInput;
            _commandContainer.walkRightCommand = WalkRightInput;
            _commandContainer.JumpCommandDown = JumpInputDown;
            _commandContainer.JumpCommandUp = JumpInputUp;
            _commandContainer.JumpCommand = JumpInput;
        }
    }
}
