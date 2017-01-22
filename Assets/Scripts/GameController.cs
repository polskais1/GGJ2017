using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public GameObject cube;
	public GameObject dropable;
	public GameObject bed;
	public float spawnInterval;
	public float spawnSpread;
	public float speed;
	public float offsetResetSpeed;
	public float upperBarOffset;
	public float lowerBarOffset;
	public int playerHealth;
	public int bedShiftDistance;

	private List<GameObject> cubes;
	private float lastCubePositionX;
	private float lastCubeSpawnTime;
	private int hitsInARow;
	private bool gameOver;
	private float currentPositionOffset;
	private float targetPositionOffset;
	private float upperBarPositionY;
	private float lowerBarPositionY;

	void Start () {
		lastCubePositionX = Random.Range (2.2f, -2.2f);
		lastCubeSpawnTime = Time.realtimeSinceStartup;
		cubes = new List<GameObject> ();
		gameOver = false;
		currentPositionOffset = bed.transform.position.y;
		targetPositionOffset = bed.transform.position.y;
	}

	void FixedUpdate () {
		if (gameOver)
			return;
		
		if (lastCubeSpawnTime + spawnInterval < Time.realtimeSinceStartup) {
			spawnCube ();
		}
	}

	void Update () {
		if (currentPositionOffset > targetPositionOffset)
			currentPositionOffset -= offsetResetSpeed;

		if (currentPositionOffset < targetPositionOffset)
			currentPositionOffset += offsetResetSpeed;

		bed.transform.position = new Vector3 (0, currentPositionOffset, 1);
		upperBarPositionY = currentPositionOffset + upperBarOffset;
		lowerBarPositionY = currentPositionOffset + lowerBarOffset;
	}

	private void startGame () {
		cubes = new List<GameObject> ();
		gameOver = false;
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

	private void endGame () {
		gameOver = true;
		foreach (GameObject cube in cubes)
			Destroy (cube);
	}

	private void destroyCube (GameObject cube) {
		cubes.Remove (cube);
		Destroy (cube);
	}


	public float getUpperBarPositionY(){
		return upperBarPositionY;
	}

	public float getLowerBarPositionY(){
		return lowerBarPositionY;
	}

	private void shiftBed (float distance) {
		targetPositionOffset = bed.transform.position.y + distance;
	}

	public void scoreHit (GameObject cube) {
		hitsInARow++;
		if (hitsInARow == 10 && playerHealth < 5) {
			hitsInARow = 0;
			playerHealth++;
			shiftBed (-bedShiftDistance);
		}

		destroyCube (cube);
	}

	public void damagePlayer (GameObject cube) {
		hitsInARow = 0;
		if (playerHealth == 1) {
			endGame ();
			return;
		}

		playerHealth--;
		destroyCube (cube);
		shiftBed (bedShiftDistance);
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

	private void spawnDrop () {
		GameObject newDrop = Instantiate (dropable, gameObject.transform);
		newDrop.transform.position = new Vector3 (4, 4, 0);
	}
}
