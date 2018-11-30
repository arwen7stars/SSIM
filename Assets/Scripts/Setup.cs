using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup : MonoBehaviour {

	// cursor icon
	public Texture2D cursor;

	void Start () {
		// change cursor to crosshair and fix its hotspot
		Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.Auto);
	}
}
