using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {

	public float rotationSpeed;
	public GameController gameController;

	void Update () {
		this.gameObject.transform.Rotate (0, rotationSpeed, 0);

		this.gameObject.transform.Translate (0, -0.05f, 0f);
	}
}
