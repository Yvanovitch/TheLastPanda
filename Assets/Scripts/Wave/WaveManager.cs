using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {
    // -------------------------------------------------------------------------
    // Variables
    // -------------------------------------------------------------------------
    public int          nbMaxWaves;
    public float        minDelayBetween2pop;
    public GameObject   wavePivot;
    public AudioSource  audioWave;

    private int         currentNbWaves;
    private float       lastPopTimer;


    // -------------------------------------------------------------------------
    // Unity functions - Override
    // -------------------------------------------------------------------------
    // Use this for initialization
    void Start(){
        lastPopTimer = 0;
        currentNbWaves = 0;
    }

    // Update is called once per frame
    void Update () {
        lastPopTimer += Time.deltaTime;
        pop1wave();
    }


    // -------------------------------------------------------------------------
    // GamePlay functions
    // -------------------------------------------------------------------------
    public void pop1wave() {
        if(currentNbWaves>=nbMaxWaves || lastPopTimer<=minDelayBetween2pop) { return; }
        Instantiate(wavePivot, transform.position, transform.rotation);
        currentNbWaves++;
        lastPopTimer = 0;
        audioWave.Stop();
        audioWave.Play();
    }
    public void destroyOneWave() {
        currentNbWaves--;
    }
}