using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class WalkingSoundBehaviour : StateMachineBehaviour
{
    
    [SerializeField] private SoundMananger _soundMananger;
    public FMODUnity.EventReference WalkingSoundFile;
    private FMOD.Studio.EventInstance _walkingSound;

    private bool test = false;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!test)
        {
            Debug.Log("test");
            _walkingSound = FMODUnity.RuntimeManager.CreateInstance(WalkingSoundFile);
            test = true;
        }
        _soundMananger.PlaySound(_walkingSound); 

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _soundMananger.StopSound(_walkingSound);
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
