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
	public float upperBarPositionY;
	public float lowerBarPositionY;
	public int playerHealth;
	public int bedShiftDistance;

	private List<GameObject> cubes;
	private float lastCubePositionX;
	private float lastCubeSpawnTime;
	private int hitsInARow;
	private bool gameOver;

	void Start () {
		lastCubePositionX = Random.Range (2.2f, -2.2f);
		lastCubeSpawnTime = Time.realtimeSinceStartup;
		cubes = new List<GameObject> ();
		gameOver = false;
	}

	void FixedUpdate () {
		if (gameOver)
			return;
		
		if (lastCubeSpawnTime + spawnInterval < Time.realtimeSinceStartup) {
			spawnCube ();
		}
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
		bed.transform.Translate (0, distance, 0);
	}

	public void scoreHit (GameObject cube) {
		hitsInARow++;
		if (hitsInARow == 10 && playerHealth < 5) {
			hitsInARow = 0;
			playerHealth++;
			shiftBed (-bedShiftDistance);
		}
		if (hitsInARow != 0 && hitsInARow % 5 == 0) {
			spawnDrop (cube.transform.position);
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

	private void spawnDrop (Vector3 position) {

		GameObject newDrop = Instantiate (dropable, gameObject.transform);
		newDrop.transform.position = position;

		StartCoroutine(destroyDrop (newDrop, 5.0f));

	}
		
	IEnumerator destroyDrop(GameObject dropable, float time)
	{
		yield return new WaitForSeconds(time);

		Destroy (dropable);
	}
}
