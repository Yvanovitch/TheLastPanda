using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    // -------------------------------------------------------------------------
    // Variables
    // -------------------------------------------------------------------------
    public AudioSource              dieSound;
    private PlayerController        pc;


    // -------------------------------------------------------------------------
    // Unity functions - Override
    // -------------------------------------------------------------------------
    void Start() {
        pc = GetComponent<PlayerController>();
    }


    // -------------------------------------------------------------------------
    // GamePlay functions
    // -------------------------------------------------------------------------
    public void die() {
        dieSound.Play();
    }
}
