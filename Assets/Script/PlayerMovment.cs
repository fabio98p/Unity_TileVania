using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovment : MonoBehaviour
{
    [SerializeField] float movmentVelocity = 250f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;


    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator animator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    LayerMask groundLayer;
    LayerMask climbingLayer;
    float gravityScaleAtStart;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = rb2d.gravityScale;
        groundLayer = LayerMask.GetMask("Ground");
        climbingLayer = LayerMask.GetMask("Climbing");
    }

    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }
    void OnJump(InputValue value)
    {
        if (!playerFeetCollider.IsTouchingLayers(groundLayer)) { return; }
        if (value.isPressed)
        {
            rb2d.velocity += new Vector2(0f, jumpSpeed);
        }
    }
    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * movmentVelocity * Time.deltaTime, rb2d.velocity.y);
        rb2d.velocity = playerVelocity;

        bool playerIsRunning = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerIsRunning);
    }
    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.velocity.x), 1f);
        }
    }
    void ClimbLadder()
    {
        if (!playerFeetCollider.IsTouchingLayers(climbingLayer)) 
        {
            rb2d.gravityScale = gravityScaleAtStart;
            animator.SetBool("isClimbing", false) ;
            return;
        }

        Vector2 climbVelocity = new Vector2(rb2d.velocity.x, moveInput.y * climbSpeed);
        rb2d.velocity = climbVelocity;
        rb2d.gravityScale = 0f;

        bool playerIsClimbing = Mathf.Abs(rb2d.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerIsClimbing);
    }

}
