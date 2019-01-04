using UnityEngine;

public class PlayerReloading : StateMachineBehaviour {

	// camera position while reloading
	public Vector3 target;

	// normalized time to restore camera position
	public float restoreCameraNormTime;

	// the player controller
	private PlayerController player;

	// the camera controller
	private CameraController cam;

    // reloading sound index
    private const int RELOAD_IDX = 1;


	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        GameObject playerObj = GameObject.Find("Player");
		player = playerObj.GetComponent<PlayerController>();
		cam = playerObj.GetComponentInChildren<CameraController>();

		// play camera animation
		cam.SetTarget(target);

		// close the curtains
		CurtainsController.CloseCurtains();

        // play reloading sound
        AudioSource[] shotgun_sounds = player.GetComponents<AudioSource>();
        shotgun_sounds[RELOAD_IDX].Play();
    }


	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		// reload
		int shells = (int) (stateInfo.normalizedTime / player.shellLoadTime);

		// deal with overflow and set
		if (shells > player.capacity) {
			shells = player.capacity;
		}
		player.SetShells(shells);

		// play camera animation
		if (stateInfo.normalizedTime >= restoreCameraNormTime) {
			cam.ResetTarget();
            // stop reloading sound
            AudioSource[] shotgun_sounds = player.GetComponents<AudioSource>();
            shotgun_sounds[RELOAD_IDX].Stop();
        }
	}


	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        cam.ResetTarget();
		CurtainsController.OpenCurtains();
    }
}
