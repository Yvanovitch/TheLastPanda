using UnityEngine;
using System.Collections;

public class WaveMovement : MonoBehaviour {
    public float        minSpeed;
    public float        maxSpeed;
    public float        minScale;
    public float        maxScale;
    public float        yStartPosition;
    public GameObject   wavePivot;
    public GameObject   playerObject;
    private float       speed;
    private float       scale;

    // Use this for initialization
    void Start() {
        //Get random values
        speed = Random.Range(minSpeed, maxSpeed);
        scale = Random.Range(minScale, maxScale);
        //Set wave
        Vector3 playerPos = new Vector3(playerObject.transform.position.x, 0, playerObject.transform.position.z);
        Vector3 wavePos = -playerPos;
        Debug.DrawRay(wavePivot.transform.position, playerPos, Color.red, 5f);
        Debug.DrawRay(wavePivot.transform.position, wavePos, Color.blue, 5f);
        transform.localScale = new Vector3(
            transform.localScale.x,
            scale,
            transform.localScale.z);
        wavePivot.transform.LookAt(wavePos);
        transform.Translate(new Vector3(0, yStartPosition, 0)); //Shift Y pos to be at bottom
    }

    void Update() {
        wavePivot.transform.Rotate(new Vector3(0, speed, 0));
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            //TODO: Kill the ugly player and display gameover panel
        }
    }
}
