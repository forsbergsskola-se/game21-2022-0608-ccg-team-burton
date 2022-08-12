using System.Collections.Generic;
using NewGraph.NodeTypes.ActionNodes;
using UnityEngine;

namespace NewGraph.NodeTypes.CompositeNodes
{
    public class CannonSelectNode : CompositeNode
    {
        private bool choiceMade;
        private Dictionary<CurrentCommand, BaseNode> ownedNodes = new();
        [HideInInspector] public CurrentCommand currentCommand;
        private BaseNode _currentChoice;

        public override void OnStart()
        {
            SetPossibleNodes();
            CheckOptions();
        }

        public override void OnExit()
        {
            
        }
        private void SetPossibleNodes()
        {
            foreach (var n in children)
            {
                var attack = n as RangedAttackNode;

                if (attack)
                {
                    ownedNodes.Add(CurrentCommand.Attack, attack);
                }
                
                var idle = n as IdleNode;

                if (idle)
                {
                    ownedNodes.Add(CurrentCommand.Idle, idle);
                }
            }
        }

        private void CheckOptions()
        {
            var eyeComp = agent.enemyEyes.compoundActions;
            
            if (eyeComp.HasFlag(CompoundActions.PlayerNoticed))
            {
                Debug.Log("player noticed");

                currentCommand = CurrentCommand.Attack;
            }

            else
            {
                currentCommand = CurrentCommand.Idle;
            }
            choiceMade = true;
        }

     

        public override State OnUpdate()
        {
            if(!choiceMade) CheckOptions();

            if (currentCommand == CurrentCommand.None) return State.Update;
        
            _currentChoice = ownedNodes[currentCommand];

            switch (_currentChoice.Update())
            {
                case State.Failure:
                    return State.Failure;
            
                case State.Update:
                    return State.Update;

                case State.Success:
                    choiceMade = false;
                    break;
            }
            return State.Update;
        }
    }
}