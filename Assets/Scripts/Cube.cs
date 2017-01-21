using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {

	public GameController gameController;

	void Update () {
		this.gameObject.transform.Rotate (0, gameController.getRotationSpeed(), 0);

		this.gameObject.transform.Translate (new Vector3( Mathf.Sin (gameController.getWaveModifier()), 0), this.gameObject.GetComponentInParent<Transform> ());
//		this.gameObject.transform.Translate (new Vector3( Mathf.Sin (gameController.getWaveModifier()), 0));
	}
}
