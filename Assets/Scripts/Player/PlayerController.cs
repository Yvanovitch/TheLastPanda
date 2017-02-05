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
    public Animator         anim;
    public AudioClip        audioClimb;
    public AudioClip        audioLand;
    public AudioClip        audioJump;
    public Transform[]      feetColliders;
    public Transform[]      leftColliders;
    public Transform[]      rightColliders;
    
    private AudioSource     audioSource;
    private Rigidbody       rg;
    private LayerMask       groundLayer;
    private Ladder          currentLadder; //Ladder where player is
    private bool            isGrounded;
    private bool            isFacingRight;
    private bool            canPlayerMove;
    
    private bool            isWalking;
    private bool            isClimbing;


    // -------------------------------------------------------------------------
    // Unity functions - Override
    // -------------------------------------------------------------------------
    // Use this for initialization
    void Start () {
        this.audioSource    = GetComponent<AudioSource>();
        this.rg             = GetComponent<Rigidbody>();
        this.groundLayer    = LayerMask.GetMask("Ground");
        this.currentLadder  = null;
        this.isFacingRight  = true;
        this.canPlayerMove  = true;
    }

    //Called at fixed time
    private void FixedUpdate() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float j = Input.GetAxis("Jump");
        this.handlePlayerMovement(h, v, j);
        anim.SetBool("IsWalking", isWalking);
        anim.SetBool("IsClimbing", isClimbing);
    }


    // -------------------------------------------------------------------------
    // GamePlay functions
    // -------------------------------------------------------------------------
    private void handlePlayerMovement(float h, float v, float j) {
        //Reinit all bool used for animation
        isWalking   = false;
        isClimbing  = false;

        //Update isGrounded + check if panda just landed
        bool wasGrounded = isGrounded;
        this.isGrounded = checkIsGrounded();
        if(wasGrounded != isGrounded && isGrounded == true) {
            this.anim.ResetTrigger("TakeOff");
            audioSource.clip = audioLand;
            audioSource.Play();
        }

        if(!canPlayerMove) { return; }
        //Move Left: h<0 (No GameObject must be on the way)
        if((h<0 && !checkLeftColliders())) {
            isWalking = isGrounded ? true : false;
            if(isFacingRight) { flip(); }
            Vector3 movementX = new Vector3(0, moveSpeed*h, 0);
            this.playerPivot.transform.Rotate(movementX);
        }
        //Move Right: h>0 (No GameObject must be on the way)
        else if(h>0 && !checkRightColliders()) {
            isWalking = isGrounded ? true : false;
            if(!isFacingRight) { flip(); }
            Vector3 movementX = new Vector3(0, moveSpeed*h, 0);
            this.playerPivot.transform.Rotate(movementX);
        }
        //Manage ladder movement
        if(currentLadder != null && currentLadder.isUsable == true && !isGrounded) {
            isClimbing = true;
            audioSource.clip = audioClimb;
            audioSource.Play();
            Vector3 movementY = new Vector3(0, ladderSpeed*v, 0);
            rg.velocity =  movementY;
        }
        else if(j>0 && isGrounded) {
            audioSource.clip = audioJump;
            audioSource.Play();
            jump();
        }
        if(this.rg.velocity.y < 0) {
            this.anim.SetBool("IsFalling", true);
        }
        else {
            this.anim.SetBool("IsFalling", false);
        }
    }

    private void jump() {
        this.anim.SetTrigger("TakeOff");
        this.rg.AddForce(new Vector3(0, jumpSpeed, 0));
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
        if(rg.velocity.y!=0) { return false; }
        bool grounded = false;
        foreach(Transform point in feetColliders) {
            grounded = Physics.Raycast(point.position, Vector3.down, collisionGroundDistance);
            //Debug.DrawRay(point.position, Vector3.down, Color.green, 1f); //DEBUG
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
    public void canMove(bool value) {
        canPlayerMove = value;
    }
}
