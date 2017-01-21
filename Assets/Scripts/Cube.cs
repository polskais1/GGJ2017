using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {

	public float rotationSpeed;
	//public GameController gameController;
	public bool cubeTapped;
	public bool canTap;
	public bool liveCube;

	void Update () {
		this.gameObject.transform.Rotate (0, rotationSpeed, 0);
		Debug.Log (canTap);
		if (liveCube) {
			checkCanTap ();
			checkCubeTapped ();
			checkCubeTouchedMouse ();
		}

	}

	private void checkCubeTapped(){
		if (canTap) {
			if (Input.touchCount > 0) {
				//checkCubeTouched ();
				checkCubeTouchedMouse();
			}
		}
	}

	private void checkCubeTouchedMouse(){
		if (Input.GetMouseButton) {
			Vector3 mouseVector = Input.mousePosition;
			mouseVector.z = 10;
			Vector3 userMousePosition = Camera.main.ScreenToWorldPoint (mouseVector);

			float lowerX = this.gameObject.transform.position.x - .25f;
			float upperX = this.gameObject.transform.position.x + .25f;
			float lowerY = this.gameObject.transform.position.y - .25f;
			float upperY = this.gameObject.transform.position.y + .25f;

			if ((userMousePosition.x > lowerX) && (userMousePosition.x < upperX) && (userMousePosition.y > lowerY) && (userMousePosition.y < upperY)) {
				cubeTapped = true;
			}
		}
	}
		
	private void checkCubeTouched(){

		Vector3 userTouchVector = Input.touches [Input.touchCount - 1].position;
		userTouchVector.z = 10;
		Vector3 userTouch = Camera.main.ScreenToWorldPoint (userTouchVector);

		float lowerX = this.gameObject.transform.position.x - .25f;
		float upperX = this.gameObject.transform.position.x + .25f;
		float lowerY = this.gameObject.transform.position.x - .25f;
		float upperY = this.gameObject.transform.position.x + .25f;

		if ((userTouch.x > lowerX) && (userTouch.x < upperX) && (userTouch.y > lowerY) && (userTouch.y < upperY)) {
			cubeTapped = true;
		}
	}

	private void checkCanTap(){
		if (!canTap) {
			checkTopInput ();
		} else {
			checkBottomInput();
		}
	}

	private void checkTopInput(){
		if (this.gameObject.transform.position.y < -6.5f) {
			canTap = true;
		}
	}

	private void checkBottomInput(){
		if (this.gameObject.transform.position.y < -7.5f) {
			canTap = false;
			liveCube = false;
		}
	}
		
}
