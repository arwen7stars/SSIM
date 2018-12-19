using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Logger {

	private static List<float> accuracy = new List<float>();
	private static float mean = 0;

	public static void LogShot(float acc) {
		
		// log
		accuracy.Add(acc);

		// update mean
		mean = (mean * (accuracy.Count - 1) + acc) / accuracy.Count;

		Debug.Log("Shots: " + accuracy.Count + ", Accuracy: " + mean);
	}
}
