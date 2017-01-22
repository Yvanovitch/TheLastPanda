using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPanda : MonoBehaviour {
    // -------------------------------------------------------------------------
    // Variables
    // -------------------------------------------------------------------------
    public AudioSource  pickUpAudio;
    private int         pickUpCounter;


    // -------------------------------------------------------------------------
    // Unity functions - Override
    // -------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "PickUpPanda") {
            pickUpCounter++;
            pickUpAudio.Play();
            Destroy(other);
        }
    }
}
