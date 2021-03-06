﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrail : MonoBehaviour {

	public GameController gameController;

	void Start () {
		this.gameObject.transform.position = new Vector3 (0, 8, 1);
	}

	void Update () {
		renderLineFromCubes ();
	}

	public void renderLineFromCubes () {
		LineRenderer linerenderer = gameObject.GetComponent<LineRenderer> ();
		List<GameObject> cubes = gameController.getCubes ();
		linerenderer.numPositions = cubes.Count;

		for (int i = 0; i < cubes.Count; i++) {
			if (cubes [i] != null)
				linerenderer.SetPosition (i, cubes [i].transform.position);
		}
	}
}
