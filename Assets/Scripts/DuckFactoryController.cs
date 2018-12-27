using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckFactoryController : MonoBehaviour {

	// prefab to instantiate
	public GameObject duck;

	// start position of prefab
	public Vector3 position;

	// start rotation of prefab
	public Vector3 rotation;

	// movement speed of prefab
	public float speed;

	// prefab x bound
	public float xbound;

	// spawn speed (ducks per second)
	public float spawnspeed;

	// how close we are to next spawn
	private float spawnmeter;


	void Start () {
		NewDuck();
		spawnmeter = 0;
	}

	
	void Update () {

		// update spawn meter
		spawnmeter += spawnspeed * Time.deltaTime;

		// spawn new duck
		if (spawnmeter >= 1) {
			NewDuck();
			spawnmeter = 0;
		}
	}

	
	/**
	* Instantiate a new Duck game object.
	*/
	public void NewDuck() {
		GameObject obj = Instantiate(duck, position, Quaternion.Euler(rotation), transform);
		obj.GetComponent<DuckController>().init(speed, xbound);
	}
}
