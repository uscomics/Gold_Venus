using UnityEngine;
using USComics_Movement;

namespace USComics_Movement {
    public class EndOfTransitionTo : StateMachineBehaviour {
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != playerCharacter) {
                MovementTransitionManager movementTransitionManagerScript = playerCharacter.GetComponent<MovementTransitionManager>();
                if (null != movementTransitionManagerScript) {
                    movementTransitionManagerScript.TransitionToComplete();
                }
            }

        }
    }
}
