using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCycle : MonoBehaviour {

	public float colorCycleDuration = 5.0f;
	public bool cycling = true;
	private float hue;
	private Light light;

	// Use this for initialization
	void Start () {
		light = gameObject.GetComponent<Light> ();
		setColors ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!cycling)
			return;
		
		setColors ();
	}

	void setColors() {
		
		hue = Mathf.Abs((Mathf.Sin (Time.time / colorCycleDuration * 2)));
		Debug.Log (hue);

		gameObject.GetComponent<Renderer> ().material.color = Color.HSVToRGB (hue, 1.0f, 1.0f);

		if (light) {
			light.color = Color.HSVToRGB (hue, 1.0f, 1.0f);
		}
	}
}
