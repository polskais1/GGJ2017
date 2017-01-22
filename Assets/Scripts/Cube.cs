using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {

	public float rotationSpeed;
	public bool cubeTapped;
	public bool canTap;
	public bool liveCube;

	protected GameController gameController;

	protected void Awake () {
		gameController = gameObject.GetComponentInParent<GameController> ();
		liveCube = true;
		canTap = false;
		cubeTapped = false;
	}

	protected virtual void Update () {
		
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

	protected void checkCubeTapped(){
		if (canTap) {
//			if (Input.touchCount > 0) {
				//checkCubeTouched ();
				checkCubeTouchedMouse();
//			}
		}
	}

	protected void checkCubeTouchedMouse(){
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
		
	protected void checkCubeTouched(){

		Vector3 userTouchVector = Input.touches [Input.touchCount - 1].position;
		userTouchVector.z = 10;
		Vector3 userTouch = Camera.main.ScreenToWorldPoint (userTouchVector);

		float lowerX = gameObject.transform.position.x - .5f;
		float upperX = gameObject.transform.position.x + .5f;
		float lowerY = gameObject.transform.position.y - .5f;
		float upperY = gameObject.transform.position.y + .5f;

		if ((userTouch.x > lowerX) && (userTouch.x < upperX) && (userTouch.y > lowerY) && (userTouch.y < upperY)) {
			cubeTapped = true;
		}
	}

	protected void checkCanTap(){
		if (!canTap) {
			checkTopInput ();
		} else {
			checkBottomInput();
		}
	}

	protected void checkTopInput(){
		if (this.gameObject.transform.position.y < gameController.getUpperBarPositionY()) {
			canTap = true;
		}
	}

	protected virtual void checkBottomInput(){
		if (this.gameObject.transform.position.y < gameController.getLowerBarPositionY()) {
			canTap = false;
			liveCube = false;
			gameController.damagePlayer (gameObject);
		}
	}
}


