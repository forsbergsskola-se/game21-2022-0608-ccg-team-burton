using UnityEngine;

namespace GamePlay.Entities.Player
{
    public class CommandContainer : MonoBehaviour
    {
        // these fields are visible in the inspector, which can be useful for testing.
        // But in some cases we might want to use {HideInInspector} or getters/setters to hide these fields

        public float WalkCommand;
        public bool JumpUpCommand;
        public bool JumpDownCommand;
        //public bool AttackCommand;
    }
}
