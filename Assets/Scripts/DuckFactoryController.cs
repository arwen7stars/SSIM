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

	// initial movement speed
	private float ispeed;

	// prefab x bound
	public float xbound;

	// spawn speed (ducks per second)
	public float spawnspeed;

	// initial spawn speed
	private float ispawnspeed;

	// how close we are to next spawn
	private float spawnmeter;


	void Start () {
		NewDuck();
		spawnmeter = 0;
		ispeed = speed;
		ispawnspeed = spawnspeed;
	}

	
	void Update () {
        // if game is paused ie changing rounds, don't spawn any ducks
        if (GameManager.instance.roundPause) return;
        
        // update spawn meter
        spawnmeter += spawnspeed * Time.deltaTime;

        // spawn new duck
        if (spawnmeter >= 1)
        {
            NewDuck();
            spawnmeter = 0;
        }
        
	}


	public void UpdateSpeed(float percentIncrease)
	{
		speed += percentIncrease * ispeed;
		spawnspeed += percentIncrease * ispawnspeed;
	}

	
	/**
	* Instantiate a new Duck game object.
	*/
	public void NewDuck() {
		GameObject obj = Instantiate(duck, position, Quaternion.Euler(rotation), transform);
		obj.GetComponent<DuckController>().Init(speed, xbound);
	}
}
