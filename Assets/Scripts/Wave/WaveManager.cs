using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {
    public GameObject wavePivot;

    // Use this for initialization
    void Start(){
        Instantiate(wavePivot);
    }

    // Update is called once per frame
    void Update () {
    }
}