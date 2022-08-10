using UnityEngine;
namespace GamePlay.Entities.Player
{
    public class PlayerInputController : MonoBehaviour
    {
        private CommandContainer _commandContainer;
        
        public float WalkInput;
        public bool WalkLeftDownInput;
        public bool WalkRightDownInput;
        public bool NoWalkInput;
        
        public bool JumpDownInput;
        public float CurrentWalkSpeed;
        public float InputAcceleration;
        public bool AttackDownInput;


        private void Awake() => _commandContainer = GetComponent<CommandContainer>();

        

        private void Update()
        {
            GatherHorizontalMovement(); // collect player inputs
            SetHorizontalMovement(); // assign value between -1 & 1
            GatherKeyboardInputs();
            SetCommands(); // assign inputs to commands


            if (WalkLeftDownInput || WalkRightDownInput)
                NoWalkInput = false;

            else if (!WalkLeftDownInput && !WalkRightDownInput)
                NoWalkInput = true;
        }


        
        private void GatherKeyboardInputs()
        {
            _commandContainer.SpaceDownCommand = Input.GetButton("Jump");
            _commandContainer.ArrowCommand = Input.GetAxisRaw("Horizontal");
            _commandContainer.AttackMouseCommand = Input.GetButton("Fire2");
        }
        
        
        
        private void GatherHorizontalMovement()
        {
            if (NoWalkInput || WalkLeftDownInput && WalkRightDownInput)
                CurrentWalkSpeed = 0;
                //Mathf.MoveTowards(CurrentWalkSpeed, 0, InputDeceleration * Time.deltaTime);
            
            if (WalkRightDownInput)
                CurrentWalkSpeed += Time.deltaTime * InputAcceleration;

            if (WalkLeftDownInput)
                CurrentWalkSpeed -= Time.deltaTime * InputAcceleration;
        }

        
        
        
        private void SetHorizontalMovement()
        {
            CurrentWalkSpeed = Mathf.Clamp(CurrentWalkSpeed, -1, 1);
            WalkInput = CurrentWalkSpeed;
        }


        
        private void SetCommands()
        {
            _commandContainer.WalkCommand = WalkInput;
            _commandContainer.JumpDownCommand = JumpDownInput;
            _commandContainer.AttackDownCommand = AttackDownInput;
        }
    }
}
