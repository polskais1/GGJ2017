using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIFlash : MonoBehaviour {
	public float duration = 1.0F;
	public Renderer rend;
	float blink = 0;

	void Start () {
		rend = GetComponent<Renderer>();
	}	

	void Update () {
		blink = Mathf.Sin(Time.time * 5.0f);
		bool show;

		if (blink > 0.0f) {
			show = false;
		} else {
			show = true;
		}

		rend.enabled = show;
	}
}
