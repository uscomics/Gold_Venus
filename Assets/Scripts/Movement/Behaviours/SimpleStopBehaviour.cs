using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Debug;
using USComics_Movement;

public class SimpleStopBehaviour : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
        if (null != playerCharacter)
        {
            MovementTransitionManager movementTransitionManagerScript = playerCharacter.GetComponent<MovementTransitionManager>();
            if (null != movementTransitionManagerScript)
            {
                movementTransitionManagerScript.TransitionFromStarted();
                DebugConsole debugConsoleScript;
                GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
                if (null != debugConsole)
                {
                    debugConsoleScript = debugConsole.GetComponent<DebugConsole>();
                    if (null != debugConsoleScript) debugConsoleScript.SetOther1("SimpleStopBehaviour.OnStateEnter");
                }
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
        if (null != playerCharacter)
        {
            MovementTransitionManager movementTransitionManagerScript = playerCharacter.GetComponent<MovementTransitionManager>();
            if (null != movementTransitionManagerScript)
            {
                movementTransitionManagerScript.TransitionFromComplete();
                DebugConsole debugConsoleScript;
                GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
                if (null != debugConsole)
                {
                    debugConsoleScript = debugConsole.GetComponent<DebugConsole>();
                    if (null != debugConsoleScript) debugConsoleScript.SetOther1("SimpleStopBehaviour.OnStateEnter");
                }
            }
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
