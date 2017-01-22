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
	public GameObject title;
	public GameObject gameOverButton;
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

	public Mesh goodMesh1;
	public Mesh goodMesh2;
	public Mesh goodMesh3;
	public Mesh badMesh1;
	public Mesh badMesh2;
	public Mesh badMesh3;
	public Material goodMat1;
	public Material goodMat2;
	public Material goodMat3;
	public Material badMat1;
	public Material badMat2;
	public Material badMat3;

	public int badCubeCounter;
	public int targetScore;
	public string cubeType;


	private List<GameObject> cubes;
	private float lastCubePositionX;
	private float lastCubeSpawnTime;
	private int streak;
	private int score;
	private bool gameOver;
	private bool inStartSequence;
	private bool inEndSequence;
	private bool betweenRounds;
	private float currentPositionOffset;
	private float targetPositionOffset;
	private float upperBarPositionY;
	private float lowerBarPositionY;
	private float cubeSpawnTimer;

	void Start () {
		lastCubePositionX = Random.Range (2.2f, -2.2f);
		lastCubeSpawnTime = 0f;
		cubeSpawnTimer = 0f;
		cubes = new List<GameObject> ();
		gameOver = true;
		currentPositionOffset = bed.transform.position.y;
		targetPositionOffset = bed.transform.position.y;
		bed.GetComponent<SpriteRenderer> ().sprite = neutral;
		betweenRounds = false;
	}

	void FixedUpdate () {
		if (gameOver) {
			if (Input.GetMouseButtonDown (0) && !inStartSequence && !betweenRounds)
				inStartSequence = true;
			else if (Input.GetMouseButtonDown (0) && !inStartSequence && betweenRounds)
				startNextRound ();
			else if (title.activeSelf && inStartSequence)
				title.SetActive (false);

			if (gameOverButton.activeSelf && inStartSequence)
				gameOverButton.SetActive (false);
			return;
		}

		if (score >= targetScore) {
			endRound ();
			return;
		}

		if (!waveTrail.activeSelf)
			waveTrail.SetActive (true);

		if (lastCubeSpawnTime + spawnInterval < cubeSpawnTimer)
			spawnCube ();
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

		if (!gameOver && score < targetScore)
			cubeSpawnTimer += 1f;

		bed.transform.position = new Vector3 (0, currentPositionOffset, 1);
		upperBarPositionY = currentPositionOffset + upperBarOffset;
		lowerBarPositionY = currentPositionOffset + lowerBarOffset;
	}

//	Logic for starting a new game from the beginning
	private void startNewGame () {
		score = 0;
		difficultyModifier = 1f;
		targetScore = Mathf.RoundToInt (perRoundScore * (difficultyModifier * 10f));
		startNewRound ();
		PlayMusic ();
	}

	private void startNewRound () {
		targetPositionOffset = -6f;
		bed.GetComponent<SpriteRenderer> ().sprite = neutral;
		playerHealth = 3;
		gameOver = false;
		betweenRounds = false;
	}

	private void endGame () {
		waveTrail.SetActive (false);
		foreach (GameObject cube in cubes)
			Destroy (cube);
		gameOver = true;
		inEndSequence = true;
		StopMusic ();
	}

	//	Logic for starting a new round in an ongoing game
	private void startNextRound () {
		difficultyModifier += 1f;
		targetScore = targetScore + Mathf.RoundToInt (perRoundScore * (difficultyModifier * 10f));
		inStartSequence = true;
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
			if (target == 0 && !betweenRounds)
				startNewGame ();
			else if (target == 0)
				startNewRound ();
		}
	}

	private void spawnCube () {
		spawnCounter += spawnSpread;
		setWave ();
		lastCubePositionX = createNewSpawnPositionX (randomWave);
		setCubeType ();
		lastCubeSpawnTime = cubeSpawnTimer;
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

	private void playGoodSound(){
		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();
	}

	private void PlayMusic() {
		GameObject bg = transform.Find ("backgroundMusic").gameObject;
		AudioSource music = bg.GetComponent<AudioSource> ();
		music.Play ();
	}

	private void StopMusic() {
		GameObject bg = transform.Find ("backgroundMusic").gameObject;
		AudioSource music = bg.GetComponent<AudioSource> ();
		music.Stop ();
	}

	private float createNewSpawnPositionX (int wavePattern) {
		float result;
		switch(wavePattern){
		case 2:
			result = (2.15f * (Mathf.Sin (spawnCounter)));
			speed = -.05f;
			spawnInterval = 12f;
			spawnSpread = .5f;
			break;
		case 1:
			result = (2.00f * (Mathf.Sin (spawnCounter)));
			speed = -.045f;
			spawnInterval = 15f;
			spawnSpread = .65f;
			break;
		case 0:
			result = (1.15f * (Mathf.Sin (spawnCounter)));
			speed = -.055f;
			spawnInterval = 18f;
			spawnSpread = .5f;
			break;
		default:
			result = (2.15f * (Mathf.Sin (spawnCounter)));
			speed = -.05f;
			spawnInterval = 12f;
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
		streak++;
		if (streak == 10) {
			streak = 0;
			if (playerHealth < 5) {
				playerHealth++;
				shiftBed (-bedShiftDistance);
			}
		}

		if (streak != 0 && streak % 5 == 0) {
			spawnDrop (new Vector3( cube.transform.position.x, cube.transform.position.y, 1.0f), true);
			playGoodSound ();
		}

		if (playerHealth > 3)
			bed.GetComponent<SpriteRenderer> ().sprite = happy;

		destroyCube (cube);
	}

	public void damagePlayer (GameObject cube) {
		streak = 0;
		Handheld.Vibrate();
		spawnDrop (new Vector3( cube.transform.position.x, cube.transform.position.y, 1.0f), false);

		if (playerHealth == 1) {
			bed.GetComponent<SpriteRenderer> ().sprite = angry;
			endGame ();
			gameOverButton.SetActive (true);
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

	public int getScore () {
		return score;
	}

	public int getStreak () {
		return streak;
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

	private void spawnDrop (Vector3 position, bool good) {
		
		Mesh newMesh;
		Material newMat;
		float which = Random.Range( 0.0f, 1.0f );
		GameObject newDrop = Instantiate (dropable, gameObject.transform);

		newDrop.transform.position = position;

		if (which > 0.6f) {
			newMesh = good ? goodMesh1 : badMesh1;
			newMat = good ? goodMat1 : badMat1;
		} else if (which > 0.3f) {
			newMesh = good ? goodMesh2 : badMesh2;
			newMat = good ? goodMat2 : badMat2;
		} else {
			newMesh = good ? goodMesh3 : badMesh3;
			newMat = good ? goodMat3 : badMat3;
		}
		
		newDrop.gameObject.GetComponentInChildren<MeshFilter> ().mesh = newMesh;
		newDrop.gameObject.GetComponentInChildren<Renderer> ().material = newMat;
		StartCoroutine(destroyDrop (newDrop, 5.0f));

	}
		
	IEnumerator destroyDrop(GameObject dropable, float time)
	{
		yield return new WaitForSeconds(time);

		Destroy (dropable);
	}
}
