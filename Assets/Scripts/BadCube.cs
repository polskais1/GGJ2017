using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BadCube : Cube
{

	protected override void Update ()
	{

		this.gameObject.transform.Rotate (0, rotationSpeed, 0);

		this.gameObject.transform.Translate (0, gameController.getSpeed (), 0f);

		if (liveCube) {
			checkCanTap ();
			checkCubeTapped ();
			//			checkCubeTouchedMouse ();
		}

		if (cubeTapped) {
			gameController.damagePlayer (gameObject);
		}

	}

	protected override void checkBottomInput(){
		if (this.gameObject.transform.position.y < gameController.getLowerBarPositionY()) {
			canTap = false;
			liveCube = false;
			gameController.scoreHit (gameObject);
		}
	}

}

