using UnityEngine;
using System.Collections;

public class WaveMovement : MonoBehaviour {
    public float        minSpeed;
    public float        maxSpeed;
    public float        minScale;
    public float        maxScale;
    public GameObject   waveObject;
    public GameObject   platformingPlayer;
    private float       speed;
    private float       scale;

    // Use this for initialization
    void Start() {
        //Get random values
        speed = Random.Range(minSpeed, maxSpeed);
        scale = Random.Range(minScale, maxScale);
        //Set wave
        waveObject.transform.localScale = new Vector3(
            waveObject.transform.localScale.x, 
            scale,
            waveObject.transform.localScale.z);
    }

    void Update() {
        transform.Rotate(new Vector3(0, speed, 0));
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            //TODO: Kill the ugly player and display gameover panel
        }
    }
}
