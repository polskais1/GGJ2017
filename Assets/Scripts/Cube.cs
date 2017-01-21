using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {

	public float rotationSpeed;
	public bool cubeTapped;
	public bool canTap;
	public bool liveCube;

	private GameController gameController;

	void Awake () {
		gameController = gameObject.GetComponentInParent<GameController> ();
		liveCube = true;
		canTap = false;
		cubeTapped = false;
	}

	void Update () {
		this.gameObject.transform.Rotate (0, rotationSpeed, 0);

		this.gameObject.transform.Translate (0, gameController.getSpeed (), 0f);

		if (liveCube) {
			checkCanTap ();
			checkCubeTapped ();
//			checkCubeTouchedMouse ();
		}

		if (cubeTapped) {
			gameController.scoreHit (gameObject);
		}

	}

	private void checkCubeTapped(){
		if (canTap) {
//			if (Input.touchCount > 0) {
				//checkCubeTouched ();
				checkCubeTouchedMouse();
//			}
		}
	}

	private void checkCubeTouchedMouse(){
		if (Input.GetMouseButton (0)) {
			Vector3 mouseVector = Input.mousePosition;
			mouseVector.z = 10;
			Vector3 userMousePosition = Camera.main.ScreenToWorldPoint (mouseVector);

			float lowerX = gameObject.transform.position.x - .5f;
			float upperX = gameObject.transform.position.x + .5f;
			float lowerY = gameObject.transform.position.y - .5f;
			float upperY = gameObject.transform.position.y + .5f;

			if ((userMousePosition.x > lowerX) && (userMousePosition.x < upperX) && (userMousePosition.y > lowerY) && (userMousePosition.y < upperY)) {
				cubeTapped = true;
			}
		}
	}
		
	private void checkCubeTouched(){

		Vector3 userTouchVector = Input.touches [Input.touchCount - 1].position;
		userTouchVector.z = 10;
		Vector3 userTouch = Camera.main.ScreenToWorldPoint (userTouchVector);

		float lowerX = gameObject.transform.position.x - .25f;
		float upperX = gameObject.transform.position.x + .25f;
		float lowerY = gameObject.transform.position.y - .25f;
		float upperY = gameObject.transform.position.y + .25f;

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
		if (this.gameObject.transform.position.y < -2.5f) {
			canTap = true;
		}
	}

	private void checkBottomInput(){
		if (this.gameObject.transform.position.y < -3.5f) {
			canTap = false;
			liveCube = false;
			gameController.damagePlayer (gameObject);
		}
	}
		
}
