using UnityEngine;
using USComics_Entity;

namespace USComics_Movement {
	public class EndOfDeath : StateMachineBehaviour {
		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

			GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
			if (null != playerCharacter) {
				PlayerController playerControllerScript = playerCharacter.GetComponent<PlayerController>();
				if (null != playerControllerScript) {
					playerControllerScript.DoSpawn();
				}
			}
		}
	}
}
