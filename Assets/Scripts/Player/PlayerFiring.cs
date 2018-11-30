using UnityEngine;

public class PlayerFiring : StateMachineBehaviour {

	// the player controller
	private PlayerController player;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		player = GameObject.Find("Player").GetComponent<PlayerController>();

		player.SetShells(player.GetShells() - 1);
		RaycastHit hit;
		if (Physics.Raycast(player.GetRay(), out hit)) {
			Debug.Log("target hit: " + hit.transform.name);
		}	
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.ResetTrigger("fire");

		// automatic reload
		if (player.GetShells() <= 0) {
			animator.SetTrigger("reload");
		}
	}
}
