﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public Camera mainCamera;
	public GameObject cube;
	public GameObject dropable;
	public GameObject bed;
	public GameObject waveTrail;
	public Sprite neutral;
	public Sprite happy;
	public Sprite angry;
	public float spawnInterval;
	public float spawnSpread;
	public float spawnCounter;
	public float speed;
	public float bedOrigin;
	public float offsetResetSpeed;
	public float upperBarOffset;
	public float lowerBarOffset;
	public int playerHealth;
	public int bedShiftDistance;
	public int randomWave;
	public int waveCounter;

	private List<GameObject> cubes;
	private float lastCubePositionX;
	private float lastCubeSpawnTime;
	private int hitsInARow;
	private bool gameOver;
	private bool inStartSequence;
	private bool inEndSequence;
	private float currentPositionOffset;
	private float targetPositionOffset;
	private float upperBarPositionY;
	private float lowerBarPositionY;

	void Start () {
		lastCubePositionX = Random.Range (2.2f, -2.2f);
		lastCubeSpawnTime = Time.fixedTime;
		cubes = new List<GameObject> ();
		gameOver = true;
		currentPositionOffset = bed.transform.position.y;
		targetPositionOffset = bed.transform.position.y;
		bed.GetComponent<SpriteRenderer> ().sprite = neutral;
	}

	void FixedUpdate () {
		if (gameOver && !inStartSequence) {
			if (Input.GetMouseButton (0))
				inStartSequence = true;
			return;
		}
		
		if (lastCubeSpawnTime + spawnInterval < Time.fixedTime) {
			spawnCube ();
		}
	}

	void Update () {
		if (inStartSequence)
			moveCamera (0.1f, 0);

		if (inEndSequence)
			moveCamera (-0.1f, -4);

		if (currentPositionOffset > targetPositionOffset)
			currentPositionOffset -= offsetResetSpeed;

		if (currentPositionOffset < targetPositionOffset)
			currentPositionOffset += offsetResetSpeed;

		bed.transform.position = new Vector3 (0, currentPositionOffset, 1);
		upperBarPositionY = currentPositionOffset + upperBarOffset;
		lowerBarPositionY = currentPositionOffset + lowerBarOffset;
	}

	private void startGame () {
		targetPositionOffset = -6f;
		bed.GetComponent<SpriteRenderer> ().sprite = neutral;
		playerHealth = 3;
		waveTrail.SetActive (true);
		gameOver = false;
	}

	private void moveCamera (float distance, int target) {
		float positionX = mainCamera.transform.position.x;
		float positionY = mainCamera.transform.position.y + distance;
		float positionZ = mainCamera.transform.position.z;
		mainCamera.transform.position = new Vector3 (positionX, positionY, positionZ);

//		The camera has gotten to its final point and the game can start
		if (Mathf.RoundToInt (positionY) == target) {
			inStartSequence = false;
			inEndSequence = false;
			cubes = new List<GameObject> ();
			if (target == 0)
				startGame ();
		}
	}

	private void spawnCube () {
		spawnCounter += spawnSpread;
		waveCounter++;
		if (waveCounter > 20) {
			waveCounter = 0;
			randomWave = Random.Range (0,3);
			Debug.Log("New Wave!  Wave: " + randomWave);
		}
		lastCubePositionX = createNewSpawnPositionX (randomWave);

		lastCubeSpawnTime = Time.fixedTime;
		GameObject newCube = Instantiate (cube, gameObject.transform);
		newCube.transform.position = new Vector3 (lastCubePositionX, 8, 0);
		cubes.Add (newCube);
	}

	private float createNewSpawnPositionX (int wavePattern) {
		float result;
		switch(wavePattern){
		case 2:
			result = (2.15f * (Mathf.Sin (spawnCounter)));
			speed = -.05f;
			spawnInterval = .3f;
			spawnSpread = .5f;
			break;
		case 1:
			result = (2.00f * (Mathf.Sin (spawnCounter)));
			speed = -.045f;
			spawnInterval = .18f;
			spawnSpread = .65f;
			break;
		case 0:
			result = (1.15f * (Mathf.Sin (spawnCounter)));
			speed = -.055f;
			spawnInterval = .35f;
			spawnSpread = .5f;
			break;
		default:
			result = (2.15f * (Mathf.Sin (spawnCounter)));
			speed = -.05f;
			spawnInterval = .3f;
			spawnSpread = .5f;
			break;
		}
			

		return result;
	}

	private void endGame () {
		waveTrail.SetActive (false);
		foreach (GameObject cube in cubes)
			Destroy (cube);
		gameOver = true;
		inEndSequence = true;
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
		targetPositionOffset = bedOrigin + distance;
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
		if (playerHealth > 3)
			bed.GetComponent<SpriteRenderer> ().sprite = happy;

		destroyCube (cube);
	}

	public void damagePlayer (GameObject cube) {
		hitsInARow = 0;
		if (playerHealth == 1) {
			bed.GetComponent<SpriteRenderer> ().sprite = angry;
			endGame ();
			return;
		}

		if (playerHealth < 4)
			bed.GetComponent<SpriteRenderer> ().sprite = neutral;

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

	public bool getGameOver () {
		return gameOver;
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
