using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		gameObject.panel.onClick.AddListener(() => {
//			panel.LoadSculpture();
//		});

		Application.LoadLevel ("viewer");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
