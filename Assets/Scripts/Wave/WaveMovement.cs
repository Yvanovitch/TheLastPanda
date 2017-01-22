using UnityEngine;
using System.Collections;

public class WaveMovement : MonoBehaviour {
    // -------------------------------------------------------------------------
    // Variables
    // -------------------------------------------------------------------------
    public float        minSpeed;
    public float        maxSpeed;
    public float        minScale;
    public float        maxScale;
    public float        yStartPosition;
    public float        growthSpeed;
    public float        dyingSpeed;
    public float        minLifeTime;
    public float        maxLifeTime;
    public GameObject   wavePivot;
    private WaveManager wavesManager;
    private GameObject  playerObject;
    private float       speed;
    private float       scaleMaxRatio;
    private Vector3     scaleMax;
    private bool        isGrowing;
    private bool        isDying;
    private float       lifetime;
    private float       timeBeforeDIE;


    // -------------------------------------------------------------------------
    // Unity functions - Override
    // -------------------------------------------------------------------------
    void Start() {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        wavesManager = GameObject.Find("WavesManager").GetComponent<WaveManager>();
        this.wavePop();
    }

    void Update() {
        wavePivot.transform.Rotate(new Vector3(0, speed, 0));
        if(isGrowing) {
            transform.localScale = Vector3.Lerp(transform.localScale, scaleMax, Time.deltaTime*growthSpeed);
        }
        else if(isDying) {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0f, scaleMax.y, 0f), Time.deltaTime*dyingSpeed);
        }
        timeBeforeDIE -= Time.deltaTime;
        if(timeBeforeDIE<=0) {
            waveDie();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            //TODO: Kill the ugly player and display gameover panel
        }
    }


    // -------------------------------------------------------------------------
    // GamePlay functions
    // -------------------------------------------------------------------------
    private void wavePop() {
        //Get random values
        speed           = Random.Range(minSpeed, maxSpeed);
        scaleMaxRatio   = Random.Range(minScale, maxScale);
        lifetime        = Random.Range(minLifeTime, maxLifeTime);
        //Set wave
        Vector3 playerPos = new Vector3(playerObject.transform.position.x, 0, playerObject.transform.position.z);
        Vector3 wavePos = -playerPos;
        wavePivot.transform.LookAt(wavePos);
        //Debug.DrawRay(wavePivot.transform.position, playerPos, Color.red, 5f);
        //Debug.DrawRay(wavePivot.transform.position, wavePos, Color.blue, 5f);
        scaleMax = new Vector3(
            transform.localScale.x,
            scaleMaxRatio,
            transform.localScale.z);
        transform.localScale = scaleMax; //Will be reset to 0 but just used for bound size here
                                         //Shift init y position to be at the ring bottom

        //float startPos = yStartPosition + GetComponent<Renderer>().bounds.size.y /2; //Old version
        float startPos = yStartPosition; //TODODEBUG Version
        transform.Translate(new Vector3(0, startPos, 0)); //Shift Y pos to be at bottom

        //Call start wave pop
        transform.localScale = new Vector3(0f, scaleMax.y, 0f);
        isGrowing       = true;
        isDying         = false;
        timeBeforeDIE   = lifetime;

        //In case of left movement, need to flip the image
        if(speed==0) { speed++; } //This is just in the case speed randomly choose 0 (This is ugly I know :p)
        if(speed<0) {
            Vector3 toto = scaleMax;
            scaleMax.z *= -1;
        }
    }

    private void waveDie() {
        isGrowing = false;
        isDying = true;
        wavesManager.destroyOneWave();
        Destroy(wavePivot, dyingSpeed); //Destroy in x seconds

    }
}
