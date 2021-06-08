using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Horizontal Movement")]
    public float moveSpeed = 15f;
    public Vector2 direction;
    private bool faceRight = true;

    [Header("Vertical Movement")]
    public float jumpForce = 5f;
    public float jumpDelay = 0.25f;
    private float jumpTimer;

    [Header("Dashes")]
    public float dashForce = 20f;
    public bool canDash;
    public float dashDelay = 0.25f;
    public float dashTimer;

    [Header("Unity Stuff")]
    public Rigidbody2D rb;
    public LayerMask groundLayer;

    [Header("Physics")]
    public float maxSpeed = 10f;
    public float linearDrag = 5f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;

    [Header("Collision")]
    public bool isGrounded = false;
    public float groundLength = 0.4f;
    public Vector3 colliderOffset;

    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer); // jump raycast
        
        if(isGrounded)
        {
            canDash = true;
        }

        if(Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay; // if the current time is within the jump timer time (Current time + Jump delay) you can jump.
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            dashTimer = Time.time + dashDelay; // same as jump
        }

        /*
        Explanation of how jump/dash works.
        When you hit the jump/dash key, the jump/dash timer updates to be more than Time.time.
        In the FixedUpdate, if jumpTimer is MORE than Time.time then you jump/dash.
        */

        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        moveChar(direction.x);
        if(jumpTimer > Time.time && isGrounded)
        {
            Jump();
        }

        if(dashTimer > Time.time && canDash)
        {
            Dash();
        }

        modifyPhysics();
    }

    void moveChar(float horizontal)
    {
        rb.AddForce(Vector2.right * horizontal * moveSpeed);

        if((horizontal > 0 && !faceRight || horizontal < 0 && faceRight))
        {
            Flip();
        }

        if(Mathf.Abs(rb.velocity.x) > maxSpeed) // Setting a Maximum Speed
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }

    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpTimer = 0;
    }

    void Dash()
    {
        
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(5,5) * dashForce, ForceMode2D.Impulse);
        dashTimer = 0;
    }

    void modifyPhysics() // Drag
    {
        bool changeDirection = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if(isGrounded)
        {
            if(Mathf.Abs(direction.x) < 0.4f || changeDirection)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
        }

        else // if not on ground
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            }
            else if(rb.velocity.y >0 && !Input.GetButton("Jump")) // if jumping but let go of the jump key
            {
                rb.gravityScale = gravity * (fallMultiplier / 2); // limit jump height
            }
            {

            }
        }
    }

    void Flip() // Changing the way Char Faces
    {
        faceRight = !faceRight;
        transform.rotation = Quaternion.Euler(0, faceRight ? 0 : 180, 0);
    }

    private void OnDrawGizmos() // visual representation of raycast
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }

}