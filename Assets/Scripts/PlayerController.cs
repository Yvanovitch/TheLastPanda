using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {
    // -------------------------------------------------------------------------
    // Variables
    // -------------------------------------------------------------------------
    public float            speed = 1;
    public float            jumpSpeed = 0;
    public GameObject       playerPivot;
    public Camera           cameraFollow;
    public float            cameraSmoothing;

    private bool            isMoving;
    private Rigidbody       rg;
    private bool            canXmove;
    private Vector3         cameraOffset;


    // -------------------------------------------------------------------------
    // Unity functions - Override
    // -------------------------------------------------------------------------
    // Use this for initialization
    void Start () {
        this.canXmove = true;
        this.rg = GetComponent<Rigidbody>();
        this.cameraOffset = new Vector3();
        this.cameraOffset = transform.position - cameraFollow.transform.position;
        //Only y access is interesting for us
        this.cameraOffset.y = 0f;
        this.cameraOffset.x = 0f;
	}

    //Called at fixed time
    private void FixedUpdate() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical"); //Note used atm
        this.handlePlayerMovement(h, v);
        if(Input.GetKeyDown("space")) {
            this.jump();
        }
        moveFollowCamera();
    }


    // -------------------------------------------------------------------------
    // Personal functions - Override
    // -------------------------------------------------------------------------
    private void handlePlayerMovement(float h, float v) {
        //Process X movement if allowed
        if(canXmove) {
            // The /200 is just to get angular speed at "human" range
            Vector3 movementX = new Vector3(0, (speed*h)/200, 0);
            this.playerPivot.transform.Rotate(movementX);
        }
    }

    private void jump() {
        //The *200 is just to get jumpSpeed values in "Human" range (Around 10, 11..)
        Vector3 movementY = new Vector3(0, jumpSpeed*200, 0);
        this.rg.AddForce(movementY);
    }

    private void moveFollowCamera() {
    }
}
