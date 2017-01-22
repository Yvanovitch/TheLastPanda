using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCollectible : MonoBehaviour {
    private int count = 0;

    void OnTriggerEnter(Collider other){
        if(other.tag == "Collectible") {
            Debug.Log("Pick Up collectible");
            other.transform.position = Vector3.zero;
            count++;
            
            if(count == 10) {
                Application.LoadLevel("WinScreen");
            }
        }
    }
}
