using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashReset : MonoBehaviour
{
    public PlayerMovement playerMovementScript;
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            playerMovementScript.canDash = false;
            Destroy(gameObject);
        }
    }
}
