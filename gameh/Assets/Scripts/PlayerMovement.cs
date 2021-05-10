using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 4f;
    public float gravityMultiplier = 3f;

     public bool isGrounded;
 
    public Rigidbody2D rb;

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }


    void FixedUpdate()
    {
        var move = Input.GetAxisRaw("Horizontal");
        transform.position = transform.position + new Vector3(move * moveSpeed * Time.fixedDeltaTime, 0, 0);
        

        if (Input.GetButton("Jump") && isGrounded == true) //Mathf.Abs(rb.velocity.y) < 0.001f
        {
            rb.velocity = Vector2.up * jumpForce;
        }

        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (gravityMultiplier - 1) * Time.fixedDeltaTime;
        }

        // Dash


        // expand this code, make it so the buttons you press also determine direction. for example, make hitting only dash make u go forward, pressing up make u go up, pressing up and right make u go up and right, etc.
        if ((Input.GetKey(KeyCode.G)) && isGrounded == true)
        {
            rb.AddForce(new Vector2(3,0), ForceMode2D.Impulse);
            
        }
    }
}
