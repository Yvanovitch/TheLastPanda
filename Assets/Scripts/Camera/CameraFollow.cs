using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    // -------------------------------------------------------------------------
    // Variables
    // -------------------------------------------------------------------------
    public GameObject       target;
    public float            distance;
    public float            cameraSmoothing;


    // -------------------------------------------------------------------------
    // Unity functions - Override
    // -------------------------------------------------------------------------
    void Update () {
        //Rotate camera to face elt
        transform.rotation = target.transform.rotation;
        //Move camera position.
        Vector3 offset = target.transform.forward.normalized * distance;
        Vector3 targetPos = target.transform.position - (target.transform.forward+offset);
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            cameraSmoothing*Time.deltaTime);
    }
}
