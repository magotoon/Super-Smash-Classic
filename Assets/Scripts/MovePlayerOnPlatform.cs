using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerOnPlatform : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player1" || other.transform.tag == "Player2")
        {
            //Debug.Log("Entering platform");
            other.transform.parent = transform;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "Player1" || other.transform.tag == "Player2")
        {
            //Debug.Log("Leaving platform");
            other.transform.parent = null;
        }
    }
 
}
