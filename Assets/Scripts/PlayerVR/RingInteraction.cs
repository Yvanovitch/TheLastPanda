using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingInteraction : MonoBehaviour {

	private float targetAlpha = 0f;
	private float alphaVelocity = 0.0f;
	public float alphaSmoothTime = 0.3f;
	private bool endTransition = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (endTransition)
			return;
		

		Material material = this.gameObject.GetComponent<Renderer>().material;
		Color color = material.color;
		color.a = Mathf.SmoothDamp (color.a, targetAlpha, ref alphaVelocity, alphaSmoothTime);
		material.color = color;

		if (Mathf.Abs (color.a - targetAlpha) < 0.00001f) {
			color.a = targetAlpha;
			endTransition = true;
		}

		Debug.Log ("Test "+targetAlpha + " a " + color.a);
	}

	public void SetTargetAlpha(float value) {
		targetAlpha = value;
		endTransition = false;
	}
}
