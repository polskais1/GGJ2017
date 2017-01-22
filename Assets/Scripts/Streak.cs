using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Streak : MonoBehaviour {

	public GameController gameController;

	void Update () {
		gameObject.GetComponent<Text> ().text = "Streak: " + gameController.getStreak ();
	}
}
