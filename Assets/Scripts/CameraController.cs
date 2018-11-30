using UnityEngine;

public class CameraController : MonoBehaviour {

	// cursor icon
	public Texture2D cursor;

	// speed at which camera moves
	public float speed;

	// original localPosition of the camera
	private Vector3 original;

	// localPosition to move camera to
	private Vector3 target;


	void Start () {
		// change cursor to crosshair and fix its hotspot
		Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.Auto);

		original = transform.localPosition;
		target = original;
	}


	void Update() {
		if (transform.localPosition != target) {
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);
		}
	}


	public void ResetTarget() {
		target = original;
	}


	public void SetTarget(Vector3 target) {
		this.target = target;
	}
}
