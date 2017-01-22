using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour {
    public float timeBeforeLooseScreen;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            Debug.Log("Player just Die hahahahaa! ");
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            ph.die();
            Invoke("hahaYouLoose", timeBeforeLooseScreen); //Load loose scene in x sec
        }
    }

    private void hahaYouLoose() {
        Application.LoadLevel("LostScreen");
    }
}
