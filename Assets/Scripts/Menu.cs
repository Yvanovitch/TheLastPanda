using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public GameObject canvasCredits;
	public Animator intro;
	public bool start = false;
    public GameObject CanvasMenu;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}

	public void NewGame(){
		SceneManager.LoadScene (1);
	}
	public void Credits(){
		canvasCredits.SetActive (true);
	}
	public void ExitCredits(){
		canvasCredits.SetActive (false);
	}
	public void ExitGame(){
		Application.Quit();
	}


}
