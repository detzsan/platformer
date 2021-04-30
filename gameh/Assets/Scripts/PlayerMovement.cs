using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 4f;
    public float gravityMultiplier = 3f;

    public Rigidbody2D rb; 
    
    void Update()
    {
        var move = Input.GetAxisRaw("Horizontal");
         transform.position = transform.position + new Vector3(move * moveSpeed * Time.deltaTime, 0, 0);

        if (Input.GetButton("Jump") && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.velocity = Vector2.up * jumpForce;
        }

        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (gravityMultiplier - 1) * Time.deltaTime;
        }

    }
}
