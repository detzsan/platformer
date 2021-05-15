using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 4f;
    public float gravityMultiplier = 3f;
    public float dashSpeed = 10f;
    public float dashCooldown = 0.5f;

     public bool canJump;
     public bool canDash;
     public bool isGrounded;
 
    public Rigidbody2D rb;

    void OnCollisionStay2D(Collision2D col)
    {
        if(col.collider.tag == "Ground")
        {
            canJump = true;
            canDash = true;
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        isGrounded = false;
    }

    void FixedUpdate()
    {
        var move = Input.GetAxisRaw("Horizontal");
        transform.position = transform.position + new Vector3(move * moveSpeed * Time.fixedDeltaTime, 0, 0);
        

        if (Input.GetButton("Jump") && canJump == true) //Mathf.Abs(rb.velocity.y) < 0.001f
        {
            rb.velocity = Vector2.up * jumpForce;
            canJump = false;
        }

        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (gravityMultiplier - 1) * Time.fixedDeltaTime;
        }

        // Dash


        // expand this code, make it so the buttons you press also determine direction. for example, make hitting only dash make u go forward, pressing up make u go up, pressing up and right make u go up and right, etc.
        if ((Input.GetKey(KeyCode.G)) && canDash == true)
        {
            canDash = false;
            rb.AddForce(new Vector2(dashSpeed,0), ForceMode2D.Impulse);

            StartCoroutine("DashTimer");
        }
    }

    // Timer to wait for dash to return.
    IEnumerator DashTimer()
    {   
        // If they touched the ground, then isGrounded is set to true and they can dash again.
        if(isGrounded == true)
        {
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
    }
}
