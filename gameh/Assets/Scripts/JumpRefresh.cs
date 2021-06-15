using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRefresh : MonoBehaviour
{
    public PlayerMovement playerMovementScript;
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            playerMovementScript.canJump = false;
            Destroy(gameObject);
        }
    }
}
