using UnityEngine;

public class PlayerReloading : StateMachineBehaviour {

	// the player controller
	private PlayerController player;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		player = GameObject.Find("Player").GetComponent<PlayerController>();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		int shells = (int) (stateInfo.normalizedTime / player.shellLoadTime);
		if (shells > player.capacity) shells = player.capacity;
		player.SetShells(shells);
	}
}
