using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour {

	public GameObject playArea;

	void Start () {
		Instantiate (playArea);
	}
}
