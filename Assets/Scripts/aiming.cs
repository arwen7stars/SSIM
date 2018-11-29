using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aiming : MonoBehaviour {

	// the player model transform
	private Transform player;

	// Use this for initialization
	void Start () {
		player = transform.Find("player");
	}
	
	// Update is called once per frame
	void Update () {

		// cast ray
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		// aim towards hit object
		if (Physics.Raycast(ray, out hit)) {
			player.LookAt(hit.point);
			Debug.DrawLine(ray.origin, hit.point, Color.red);
		}
	}
}
