using UnityEngine;

public class PlayerFiring : StateMachineBehaviour {

	// the player controller
	private PlayerController player;

    // duck tag
    private const string DUCK_TAG = "Duck";

	// duck tag
	private const string TARGET_TAG = "Target";


	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		player = GameObject.Find("Player").GetComponent<PlayerController>();

		// shoot
		player.SetShells(player.GetShells() - 1);
		RaycastHit hit;
		if (Physics.Raycast(player.GetRay(), out hit)) {
			if (hit.transform.tag == TARGET_TAG) {
				hit.transform.gameObject.GetComponentInParent<DuckController>().Hit(true, hit.textureCoord2);
			} else if (hit.transform.tag == DUCK_TAG) {
				hit.transform.gameObject.GetComponentInParent<DuckController>().Hit(false, hit.textureCoord2);
			} else {
				Logger.LogShot(0);
			}
		} else {
			Logger.LogShot(0);
		}	
	}


	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.ResetTrigger("fire");

		// automatic reload
		if (player.GetShells() <= 0) {
			animator.SetTrigger("reload");
		}
	}
}
