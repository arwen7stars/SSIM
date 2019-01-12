using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	// inital amount of shells
	public int capacity;

	// normalized time it takes to load each shell
	public float shellLoadTime;

	// the shell canvas display
	public Text shellDisplay;

    // the player model transform
    public Transform playerModel;

    // the animator
    private Animator anim;

	// the main camera
	private Camera cam;

	// the ray cast from the current mouse position
	private Ray ray;

	// current amount of shells
	private int shells;

	// prevous mouse position
	private Vector3 prevMousePosition;


	void Start () {
		prevMousePosition = new Vector3(float.MinValue, float.MinValue);
		cam = Camera.main;
		anim = playerModel.gameObject.GetComponent<Animator>();
		shells = capacity;
        shellDisplay.text = "SHELLS: " + shells + " out of " + capacity;
    }

	
	void Update () {
		Vector3 mousePosition = Input.mousePosition;

		// cast ray from mouse position
		ray = cam.ScreenPointToRay(mousePosition);

		// adjust aim
		Aim();

        // ignore user input if game over or if game is has not started yet
        if (GameManager.instance.gameStart && !GameManager.instance.gameOver 
            && !GameManager.instance.gameStop && !GameManager.instance.roundPause)
        {
			// first time
			if (prevMousePosition.x == float.MinValue && prevMousePosition.y == float.MinValue)
			{
				prevMousePosition = mousePosition;
			}

			// store mouse traveled distance this update frame
			GameManager.instance.LogMouse(Vector3.Distance(mousePosition, prevMousePosition));
			
			// read user inputs
            HandleInput();
        }

		// update mouse pos even through menus (but dont store it)
		prevMousePosition = mousePosition;

		// update UI
		shellDisplay.text = "SHELLS: " + shells + " out of " + capacity;
	}


	void HandleInput() {
		// shoot
		if (Input.GetButton("Fire") && shells > 0) {
			anim.SetTrigger("fire");
		}
	}


	void Aim() {
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
