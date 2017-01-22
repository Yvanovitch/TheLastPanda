using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {
    // -------------------------------------------------------------------------
    // Variables
    // -------------------------------------------------------------------------
    public float            moveSpeed = 1;
    public float            ladderSpeed = 1;
    public float            jumpSpeed = 0;
    public float            collisionDistance = 1f;
    public float            collisionGroundDistance = 0.01f;
    public GameObject       playerPivot;
    public GameObject       playerSprite;
    public Transform[]      feetColliders;
    public Transform[]      leftColliders;
    public Transform[]      rightColliders;
    
    private bool            isGrounded;
    private Rigidbody       rg;
    private LayerMask       groundLayer;
    private Ladder          currentLadder; //Ladder where player is
    private bool            isFacingRight;


    // -------------------------------------------------------------------------
    // Unity functions - Override
    // -------------------------------------------------------------------------
    // Use this for initialization
    void Start () {
        this.groundLayer    = LayerMask.GetMask("Ground");
        this.rg             = GetComponent<Rigidbody>();
        this.currentLadder  = null;
        this.isFacingRight  = true;
    }

    public void Update() {
        this.isGrounded = checkIsGrounded();
    }

    //Called at fixed time
    private void FixedUpdate() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        this.handlePlayerMovement(h, v);
        if(Input.GetKeyDown(KeyCode.Space)) {
            this.jump();
        }
    }


    // -------------------------------------------------------------------------
    // GamePlay functions
    // -------------------------------------------------------------------------
    private void handlePlayerMovement(float h, float v) {
        //Move Left: h<0 (No GameObject must be on the way)
        if((h<0 && !checkLeftColliders())) {
            if(isFacingRight) { flip(); }
            Vector3 movementX = new Vector3(0, moveSpeed*h, 0);
            this.playerPivot.transform.Rotate(movementX);
        }
        //Move Right: h>0 (No GameObject must be on the way)
        if(h>0 && !checkRightColliders()) {
            if(!isFacingRight) { flip(); }
            Vector3 movementX = new Vector3(0, moveSpeed*h, 0);
            this.playerPivot.transform.Rotate(movementX);
        }
        //Manage ladder movement
        if(currentLadder != null && currentLadder.isUsable == true) {
            Vector3 movementY = new Vector3(0, ladderSpeed*v, 0);
            rg.velocity =  movementY;
        }
    }

    private void jump() {
        if(!isGrounded) { return; }
        this.rg.AddForce(new Vector3(0, jumpSpeed, 0));
        this.isGrounded = false;
    }

    private void flip() {
        //Flip the PlayerSprite
        isFacingRight = !isFacingRight;
        Vector3 theScale = playerSprite.transform.localScale;
        theScale.x *= -1;
        playerSprite.transform.localScale = theScale;
    }


    // -------------------------------------------------------------------------
    // Tool functions - Check functions
    // -------------------------------------------------------------------------
    private bool checkIsGrounded() {
        Vector3 v = new Vector3(0, -1, 0);
        bool grounded = false;
        foreach(Transform point in feetColliders) {
            grounded = Physics.Raycast(point.position, v, collisionGroundDistance, groundLayer);
            //Debug.DrawRay(point.position, v, Color.blue, 1f); //DEBUG
            if(grounded) { return true; } //We can stop as soon as one is grounded
        }
        return false;
    }

    private bool checkRightColliders() {
        Vector3 v = new Vector3(1, 0, 0);
        v = transform.TransformDirection(v);
        bool collide = false;
        foreach(Transform point in rightColliders) {
            collide = Physics.Raycast(point.position, v, collisionDistance);
            //Debug.DrawRay(point.position, v, Color.red, 1f); //DEBUG
            if(collide) { return true; }
        }
        return false;
    }

    private bool checkLeftColliders() {
        Vector3 v = new Vector3(-1, 0, 0);
        v = transform.TransformDirection(v);
        bool collide = false;
        foreach(Transform point in leftColliders) {
            collide = Physics.Raycast(point.position, v, collisionDistance);
            //Debug.DrawRay(point.position, v, Color.yellow, 1f); //DEBUG
            if(collide) { return true; }
        }
        return false;
    }


    // -------------------------------------------------------------------------
    // Getters - Setters
    // -------------------------------------------------------------------------
    public void setCurrentLadder(Ladder ladder) {
        this.currentLadder = ladder;
    }
    public Ladder getCurrentLadder() {
        return this.currentLadder;
    }
}
