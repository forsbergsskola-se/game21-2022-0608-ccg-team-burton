using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayNonStackingSoundBehaviour : StateMachineBehaviour
{
    [SerializeField] private SoundMananger _soundMananger;
    public FMODUnity.EventReference SoundFile;
    private FMOD.Studio.EventInstance _sound;

    private bool initiated;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!initiated)
        {
            _sound = FMODUnity.RuntimeManager.CreateInstance(SoundFile);
            initiated = true;
        }

        _soundMananger.PlaySound(_sound);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _soundMananger.StopSound(_sound);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
