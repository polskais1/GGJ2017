using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public Camera mainCamera;
	public GameObject cube;
	public GameObject badCube;
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
	public float difficultyModifier;
	public float perRoundScore;
	public int playerHealth;
	public int bedShiftDistance;
	public int randomWave;
	public int waveCounter;
	public int badCubeCounter;
	public int targetScore;
	public string cubeType;

	private List<GameObject> cubes;
	private float lastCubePositionX;
	private float lastCubeSpawnTime;
	private int hitsInARow;
	private int score;
	private bool gameOver;
	private bool inStartSequence;
	private bool inEndSequence;
	private bool betweenRounds = true;
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
		targetScore = Mathf.RoundToInt (perRoundScore * (difficultyModifier * 10f));
		betweenRounds = false;
	}

	void FixedUpdate () {
		if (gameOver) {
			if (Input.GetMouseButtonDown (0) && !inStartSequence && !betweenRounds)
				inStartSequence = true;
			else if (Input.GetMouseButtonDown (0) && !inStartSequence && !betweenRounds)
				startNextRound ();
			return;
		}

		if (score >= targetScore) {
			endRound ();
			return;
		}

		if (!waveTrail.activeSelf)
			waveTrail.SetActive (true);
		
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

//	Logic for starting a new game from the beginning
	private void startNewGame () {
		targetPositionOffset = -6f;
		bed.GetComponent<SpriteRenderer> ().sprite = neutral;
		playerHealth = 3;
		gameOver = false;
		score = 0;
	}

	private void endGame () {
		waveTrail.SetActive (false);
		foreach (GameObject cube in cubes)
			Destroy (cube);
		gameOver = true;
		inEndSequence = true;
	}

//	Logic for starting a new round in an ongoing game
	private void startNextRound () {
		difficultyModifier += 0.1f;
		targetScore = targetScore + Mathf.RoundToInt (perRoundScore * (difficultyModifier * 10f));
	}

	private void endRound () {
		endGame ();
		betweenRounds = true;
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
				startNewGame ();
		}
	}

	private void spawnCube () {
		spawnCounter += spawnSpread;
		setWave ();
		lastCubePositionX = createNewSpawnPositionX (randomWave);
		setCubeType ();
		lastCubeSpawnTime = Time.fixedTime;
		GameObject newCube;
		switch (cubeType) {
		case "GoodCube":
			newCube = Instantiate (cube, gameObject.transform);
			break;
		case "BadCube":
			newCube = Instantiate (badCube, gameObject.transform);
			break;
		default:
			newCube = Instantiate (cube, gameObject.transform);
			break;
		}

		newCube.transform.position = new Vector3 (lastCubePositionX, 8, 0);
		cubes.Add (newCube);
	}

	private void setCubeType(){
		badCubeCounter++;
		if (badCubeCounter>Random.Range(10,15)) {
			cubeType = "BadCube";
		}
		if(badCubeCounter>Random.Range(17,21)){
			badCubeCounter = 0;
			cubeType = "GoodCube";
		}
	}

	private void setWave(){
		waveCounter++;
		if (waveCounter > 20) {
			waveCounter = 0;
			randomWave = Random.Range (0,3);
		}
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
		score++;
		Debug.Log (score);
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
		return speed * difficultyModifier;
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
