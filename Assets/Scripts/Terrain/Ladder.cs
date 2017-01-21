using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {
    // -------------------------------------------------------------------------
    // Variables
    // -------------------------------------------------------------------------
    public int      ladderType = 1;
    public float    expulsionForce = 1f;


    // -------------------------------------------------------------------------
    // Unity functions - Override
    // -------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other) {
        //Only player can use ladder
        if(other.tag != "Player") {
            return;
        }
        PlayerController pc = other.GetComponent<PlayerController>();
        //If player is not already on a ladder
        if(pc.getCurrentLadder() == null) {
            pc.setCurrentLadder(this);
            return;
        }
        //Here, means player is already on a ladder. Check if can switch to this one
        if(pc.getCurrentLadder().getLadderType() == this.ladderType) {
            pc.setCurrentLadder(this);
        }
        else {
            //Player is not allowed to switch to this ladder
            pc.GetComponent<Rigidbody>().AddForce(Vector3.down*expulsionForce);
        }
    }

    private void OnTriggerStay(Collider other) {
        //In a case player go up, switch ladder and go down again
        //Before exiting the old ladder, he will go back to the old ladder
        //without changing the setCurrentLadder. -> this will check again
        if(other.tag != "Player") {
            return;
        }
        PlayerController pc = other.GetComponent<PlayerController>();
        if(pc.getCurrentLadder()==null) {
            pc.setCurrentLadder(this);
        }
    }

    private void OnTriggerExit(Collider other) {
        //Only player can use ladder
        if(other.tag != "Player") {
            return;
        }
        //Check whether player is already on another ladder or totally leave any ladder
        PlayerController pc = other.GetComponent<PlayerController>();
        if(pc.getCurrentLadder() == this) {
            pc.setCurrentLadder(null);
        }
    }


    // -------------------------------------------------------------------------
    // Getters - Setters
    // -------------------------------------------------------------------------
    public int getLadderType() {
        return this.ladderType;
    }
}
