using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject cube;
	public float spawnInterval;

	private float lastBlockPositionX;
	private float lastBlockSpawn;

	//	TODO create logic for when to spawn a block
	//	TODO create logic to ensure that the spawned blocks are in a wave pattern
	//	TODO blocks need to move down towards the hit area
	//	TODO create logic so that a block knows when it is in the hit area
	//	TODO create logic for hit/miss tap input

	void Start () {
		lastBlockPositionX = Random.Range (2.2f, -2.2f);

		spawnBlock (lastBlockPositionX);
	}

	void FixedUpdate () {
		
	}

	private void spawnBlock (float positionX) {
		GameObject newCube = Instantiate (cube);
		newCube.transform.position = new Vector3 (positionX, 0, 0);


	}
}
