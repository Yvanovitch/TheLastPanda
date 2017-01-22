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

    private bool            isJumping;
    private bool            isWalking;
    private bool            isClimbing;


    // -------------------------------------------------------------------------
    // Unity functions - Override
    // -------------------------------------------------------------------------
    // Use this for initialization
    void Start () {
        this.audioSource    = GetComponent<AudioSource>();
        this.groundLayer    = LayerMask.GetMask("Ground");
        this.rg             = GetComponent<Rigidbody>();
        this.currentLadder  = null;
        this.isFacingRight  = true;
        this.canPlayerMove  = true;
    }

    public void Update() {
        //Update isGrounded + check if panda just landed
        bool wasGrounded = isGrounded;
        this.isGrounded = checkIsGrounded();
        if(wasGrounded != isGrounded && isGrounded == true) {
            audioSource.clip = audioLand;
            audioSource.Play();
        }
        anim.SetBool("IsWalking", isWalking);
        anim.SetBool("IsJumping", isJumping);
        anim.SetBool("IsClimbing", isClimbing);
    }

    //Called at fixed time
    private void FixedUpdate() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        this.handlePlayerMovement(h, v);
    }


    // -------------------------------------------------------------------------
    // GamePlay functions
    // -------------------------------------------------------------------------
    private void handlePlayerMovement(float h, float v) {
        //Reinit all bool used for animation
        isWalking   = false;
        isJumping   = false;
        isClimbing  = false;
        if(!canPlayerMove) { return; }
        //Move Left: h<0 (No GameObject must be on the way)
        if((h<0 && !checkLeftColliders())) {
            isWalking = true;
            if(isFacingRight) { flip(); }
            Vector3 movementX = new Vector3(0, moveSpeed*h, 0);
            this.playerPivot.transform.Rotate(movementX);
        }
        //Move Right: h>0 (No GameObject must be on the way)
        else if(h>0 && !checkRightColliders()) {
            isWalking = true;
            if(!isFacingRight) { flip(); }
            Vector3 movementX = new Vector3(0, moveSpeed*h, 0);
            this.playerPivot.transform.Rotate(movementX);
        }
        //Manage ladder movement
        if(currentLadder != null && currentLadder.isUsable == true) {
            isClimbing = true;
            audioSource.clip = audioClimb;
            audioSource.Play();
            Vector3 movementY = new Vector3(0, ladderSpeed*v, 0);
            rg.velocity =  movementY;
        }
        else if(Input.GetKeyDown(KeyCode.Space) || v>0) {
            isJumping = true;
            audioSource.clip = audioJump;
            audioSource.Play();
            jump();
        }
        else if(!isGrounded) {
            isJumping = true;
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
            grounded = Physics.Raycast(point.position, v, collisionGroundDistance);
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
    public void canMove(bool value) {
        canPlayerMove = value;
    }
}
