using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {
    public int          nbMaxWaves;
    public float        minDelayBetween2pop;
    public GameObject   wavePivot;

    private int         currentNbWaves;
    private float       lastPopTimer;

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

    public void pop1wave() {
        if(currentNbWaves>=nbMaxWaves || lastPopTimer<=minDelayBetween2pop) { return; }
        Instantiate(wavePivot, transform.position, transform.rotation);
        currentNbWaves++;
        lastPopTimer = 0;
    }
    public void destroyOneWave() {
        currentNbWaves--;
    }
}