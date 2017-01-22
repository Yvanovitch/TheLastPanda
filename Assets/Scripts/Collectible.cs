using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    public int count = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "collectible")
        {
            count++;
            Destroy(other);
            Debug.Log("Test collectible");
        }
        Debug.Log("Test collectible");
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "collectible")
        {
            count++;
            Destroy(other);
            Debug.Log("Test collectible");
        }
        Debug.Log("Test collectible");
    }
}
