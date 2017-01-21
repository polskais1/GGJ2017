using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrail : MonoBehaviour {

	public GameController gameController;

	void Start () {
		this.gameObject.transform.position = new Vector3 (0, 8, 1);
	}

	void Update () {
		float trailX = gameController.getLastCubePositionX () - gameObject.transform.position.x;
		LineRenderer linerenderer = gameObject.GetComponent<LineRenderer> ();

		List<GameObject> cubes = gameController.getCubes ();
		linerenderer.numPositions = cubes.Count;

		for (int i = 0; i < cubes.Count; i++) {
			linerenderer.SetPosition (i, cubes [i].transform.position);
		}
	}
}
