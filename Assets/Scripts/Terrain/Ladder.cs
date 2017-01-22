using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {
    // -------------------------------------------------------------------------
    // Variables
    // -------------------------------------------------------------------------
    public int      ladderType = 1;
    public bool     isUsable = true;


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
            isUsable = true;
            pc.setCurrentLadder(this);
            return;
        }
        //Here, means player is already on a ladder. Check if can switch to this one
        if(pc.getCurrentLadder().getLadderType() == this.ladderType) {
            isUsable = true;
            pc.setCurrentLadder(this);
        }
        else {
            //Player is not allowed to switch to this ladder
            isUsable = false;
            pc.GetComponent<Rigidbody>().velocity = Vector3.zero;
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
            isUsable = true;
        }
        else {
            isUsable=true;
            if(pc.getCurrentLadder()!=null) {
                pc.getCurrentLadder().isUsable = true;
            }
        }
    }


    // -------------------------------------------------------------------------
    // Getters - Setters
    // -------------------------------------------------------------------------
    public int getLadderType() {
        return this.ladderType;
    }
}
