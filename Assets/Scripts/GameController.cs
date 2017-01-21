using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public GameObject cube;
	public float spawnInterval;
	public float spawnSpread;
	public float speed;

	private List<GameObject> cubes;
	private float lastCubePositionX;
	private float lastCubeSpawnTime;

	//	TODO create logic so that a block knows when it is in the hit area
	//	TODO create logic for hit/miss tap input
	//	TODO destroy a cube when it exits the scene

	void Start () {
		lastCubePositionX = Random.Range (2.2f, -2.2f);
		lastCubeSpawnTime = Time.realtimeSinceStartup;
		cubes = new List<GameObject> ();
	}

	void FixedUpdate () {
		if (lastCubeSpawnTime + spawnInterval < Time.realtimeSinceStartup)
			spawnCube ();
	}

	private void spawnCube () {
		lastCubePositionX += createNewSpawnPositionX ();
		lastCubeSpawnTime = Time.realtimeSinceStartup;
		GameObject newCube = Instantiate (cube, gameObject.transform);
		newCube.transform.position = new Vector3 (lastCubePositionX, 8, 0);
		cubes.Add (newCube);
	}

	private float createNewSpawnPositionX () {
		if (lastCubePositionX > 2.15)
			return -spawnSpread;
		else if (lastCubePositionX < -2.15)
			return spawnSpread;
		else if (Random.Range (0, 2) == 0)
			return spawnSpread;
		else
			return -spawnSpread;
	}

	public void destroyCube (GameObject cube) {
		Debug.Log ("I destroyed a cube");
		cubes.Remove (cube);
		Destroy (cube);
	}

	public float getSpeed () {
		return speed;
	}

	public float getLastCubePositionX () {
		return lastCubePositionX;
	}

	public List<GameObject> getCubes () {
		return cubes;
	}
}
