using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerBar : MonoBehaviour {

	public GameController gameController;

	void Awake () {
		gameController = gameObject.GetComponentInParent<GameController> ();
		setBarPosition ();
	}
	
	// Update is called once per frame
	void Update () {
		setBarPosition ();
	}

	private void setBarPosition(){
		float tempY = gameController.getLowerBarPositionY();
		gameObject.transform.position = new Vector3 (gameObject.transform.position.x, tempY, gameObject.transform.position.z);
	}


}
