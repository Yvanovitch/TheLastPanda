using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F9))
			SceneManager.LoadScene ("MainScene");

        /*GameObject.Find("Camera (eye)").GetComponent<Camera>().targetDisplay = 1;
        GameObject head = GameObject.Find("Camera (head)");
            if (head != null) {
            head.GetComponent<Camera>().targetDisplay = 1;
        }*/
    }
}
