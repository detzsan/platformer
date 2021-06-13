using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissapear : MonoBehaviour
{
    public bool colliderOn;

    void Start()
    {
        colliderOn = true;
        InvokeRepeating("ColliderCheck", 2f, 2f);
    }

    void ColliderCheck()
    {
            if(colliderOn == true)
            {
                this.GetComponent<Collider2D>().enabled = false;
                colliderOn = false;
            }
            
            if(colliderOn == false)
            {
                this.GetComponent<Collider2D>().enabled = true;
                colliderOn = true;
            }
    }
}
