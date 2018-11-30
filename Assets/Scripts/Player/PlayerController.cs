using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	// inital amount of shells
	public int capacity;

	// normalized time it takes to load each shell
	public float shellLoadTime;

	// the shell canvas display
	public Text shellDisplay;

	// the animator
	private Animator anim;

	// the main camera
	private Camera cam;

	// the player model transform
	private Transform playerModel;

	// the ray cast from the current mouse position
	private Ray ray;

	// current amount of shells
	private int shells;


	void Start () {
		cam = Camera.main;
		playerModel = transform.Find("player");
		anim = playerModel.gameObject.GetComponent<Animator>();
		shells = capacity;
	}

	
	void Update () {
		// cast ray from mouse position
		ray = cam.ScreenPointToRay(Input.mousePosition);

		// adjust aim
		aim();

		// read user inputs
		handleInput();

		// update UI
		shellDisplay.text = "" + shells;
	}


	void handleInput() {
		// shoot
		if (Input.GetButton("Fire") && shells > 0) {
			anim.SetTrigger("fire");
		}
	}


	void aim() {
		Vector3 target = ray.GetPoint(cam.farClipPlane);
		playerModel.LookAt(target);
		Debug.DrawLine(ray.origin, target, Color.red);
	}


	public int GetShells() {
		return shells;
	}

	public void SetShells(int shells) {
		this.shells = shells;
	}

	public Ray GetRay() {
		return ray;
	}
}
