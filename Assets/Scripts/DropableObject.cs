using UnityEngine;

public class DropableObject : MonoBehaviour 
{

	Vector2 direction;
	Vector2 rotationV;

	float gravity = 9.8f;

	public bool rotates = false;
	public float growthTime = 0.7f;
	public float rotationRange = 500.0f;

	private float startTime;

	void Awake() {

		startTime = Time.time;
		transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);

		if (rotates) {

			rotationV = new Vector2 (
				Random.Range (-rotationRange, rotationRange), 
				Random.Range (-rotationRange, rotationRange)
			);

		} else {
			
			direction = new Vector2 (Random.Range (-1.0f, 1.0f), Random.Range (-1.0f, 1.0f));

		}

	}

	void Update () {
		float currentSize = Mathf.Min(
			(Time.time - startTime)/growthTime, 
			1.0f
		);

		if(rotates) {

			transform.Rotate(Vector3.right * rotationV.x * Time.deltaTime);
			transform.Rotate(Vector3.up * rotationV.y * Time.deltaTime);

		} else {
			
			Vector3 position = gameObject.transform.position;

			gameObject.transform.position += new Vector3 (
				direction.x * Time.deltaTime, 
				direction.y * Time.deltaTime, 
				gravity * Time.deltaTime
			);

		}

		transform.localScale = new Vector3 (
			currentSize, 
			currentSize,
			currentSize
		);

	}

}