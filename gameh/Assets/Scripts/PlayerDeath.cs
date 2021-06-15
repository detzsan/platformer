using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public PlayerMovement playerMovementScript;

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider.tag == "Fail")
        {
            
            FindObjectOfType<GameManager>().EndGame();
            playerMovementScript.enabled = false;
        }
    }
}
