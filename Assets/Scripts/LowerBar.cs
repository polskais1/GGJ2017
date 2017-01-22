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
		if (gameController.getGameOver () && gameObject.transform.localScale.x != 0)
			gameObject.transform.localScale = new Vector3 (0f, 0f, 0f);
		else if (!gameController.getGameOver () && gameObject.transform.localScale.x == 0)
			gameObject.transform.localScale = new Vector3 (0.1f, 4f, 0.1f);
		
		setBarPosition ();
	}

	private void setBarPosition(){
		float tempY = gameController.getLowerBarPositionY();
		gameObject.transform.position = new Vector3 (gameObject.transform.position.x, tempY, gameObject.transform.position.z);
	}


}
