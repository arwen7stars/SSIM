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

	// scale of the prefab
	public float scale;

	// initial scale of the prefab
	private float iscale;

	// movement speed of prefab
	public float speed;

	// initial movement speed
	private float ispeed;

	// prefab x bound
	public float xbound;

	// spawn speed (ducks per second)
	public float spawnspeed;

    // max allowed scale
    public float maxallowedscale;

    // initial spawn speed
    private float ispawnspeed;

	// how close we are to next spawn
	private float spawnmeter;


	void Start () {
		spawnmeter = 0;
		iscale = scale;
		ispeed = speed;
		ispawnspeed = spawnspeed;
	}

	
	void Update () {
        // if game is paused ie changing rounds, don't spawn any ducks
        if (GameManager.instance.roundPause || !GameManager.instance.gameStart) return;
        
        // update spawn meter
        spawnmeter += spawnspeed * Time.deltaTime;

        // spawn new duck
        if (spawnmeter >= 1)
        {
            NewDuck();
            spawnmeter = 0;
        }
        
	}


	/**
	* Updates scale and speed params by the given ammount.
	*
	* @param scalePercentIncrease Percentage to increase the current scale by (non-linear).
	* @param speedPercentIncrease Percentage to increase the current speed by (linear).
	*/
	public void UpdateScaleAndSpeedBy(float scalePercentIncrease, float speedPercentIncrease)
	{
        // scale
        scale += scalePercentIncrease * scale;

        if (scale > maxallowedscale)
            scale = maxallowedscale;

        // speed and spawn rate
        speed += speedPercentIncrease * ispeed;
		spawnspeed += speedPercentIncrease * ispawnspeed;
	}

	
	/**
	* Instantiate a new Duck game object.
	*/
	public void NewDuck() {
		GameObject obj = Instantiate(duck, position, Quaternion.Euler(rotation), transform);
		obj.GetComponent<DuckController>().Init(speed, xbound, scale);
	}
}
