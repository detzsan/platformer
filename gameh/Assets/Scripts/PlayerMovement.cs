
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
    public float jumpForce = 6f;
    public float jumpDelay = 0.25f;
    private float jumpTimer;

    [Header("Dashes")]
    public float dashForce = 8.5f;
    public float dashDelay = 0.25f;
    public float dashTimer;
    public bool canDash = false;
    public bool isDashing = false;

    [Header("Unity Stuff")]
    public Rigidbody2D rb;
    public LayerMask groundLayer;

    [Header("Physics")]
    public float maxSpeed = 10f;
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float gravityOffTime = 0.3f; // How long before turning gravity back on
    public float fallMultiplier = 3f;

    [Header("Ground Collision")]
    public bool isGrounded = false;
    public float groundLength = 0.4f;
    public Vector3 groundColliderOffset;

    [Header("Wall")]
    public bool onWall = false;
    public bool isWallSliding;
    public float wallLength = 0.65f;
    public Vector3 wallColliderOffset;
    public float wallSlideSpeed = -1f;


     void OnCollisionStay2D(Collision2D col)
 {
     if(col.gameObject.tag == "Ground")
        {
            canDash = true;
        }
 }

    void Update()
    {

        // jump raycast
        isGrounded = Physics2D.Raycast(transform.position + groundColliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - groundColliderOffset, Vector2.down, groundLength, groundLayer); 
        
        // wall raycast
        onWall = Physics2D.Raycast(transform.position + wallColliderOffset, Vector2. right, wallLength, groundLayer);
        

        if(Input.GetButton("Jump"))
        {
            jumpTimer = Time.time + jumpDelay; // if the current time is within the jump timer time (Current time + Jump delay) you can jump.
        }

        if(Input.GetKeyDown(KeyCode.G))
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
        wallSlide();
        if(jumpTimer > Time.time && isGrounded)
        {
            Jump();
        }

        modifyPhysics();

        if(dashTimer > Time.time && canDash)
        {
            Dash();
        }
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
        gravity = 0;
        Invoke("resetGravity", gravityOffTime); // Invokes the function resetGravity, which sets gravity back to 1. gravityOffTime is how long it takes before excecuting the resetGravity function.
        rb.AddForce(new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")) * dashForce, ForceMode2D.Impulse);
        canDash = false;
        dashTimer = 0;

    }

    void wallSlide() // Wallsliding
    {
        if (onWall && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

        if(isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, wallSlideSpeed);
        }
    }

/*
IDEA
for walljump
add a bool called canWallJump
when sliding down wall
set to true.
if u press "jump" key
when canWallJump is active
it makes you jump at an angle
away from the wall. rb.addforce (x,y)
make x and y uneven to make it an angle.

*/

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
                rb.gravityScale = gravity;
            }
            else if(rb.velocity.y > 0 && !Input.GetButton("Jump")) // if jumping but let go of the jump key
            {
                rb.gravityScale = gravity * (fallMultiplier / 2); // limit jump height
            }
        }
    }

    void Flip() // Changing the way Char Faces
    {
        faceRight = !faceRight;
        transform.rotation = Quaternion.Euler(0, faceRight ? 0 : 180, 0);
    }

    void resetGravity() // resets gravity for the dash
    {
        gravity = 1;
    }

    private void OnDrawGizmos() // visual representation of raycast
    {
        Gizmos.color = Color.red;
        // Ground detection raycast
        Gizmos.DrawLine(transform.position + groundColliderOffset, transform.position + groundColliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - groundColliderOffset, transform.position - groundColliderOffset + Vector3.down * groundLength);
        // Wall detection raycast
        Gizmos.DrawLine(transform.position + wallColliderOffset, transform.position + wallColliderOffset + Vector3.right * wallLength);
    }

}