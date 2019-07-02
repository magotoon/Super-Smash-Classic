using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDrop : MonoBehaviour {
    public float threshold = 0.3f;
    Vector2 newpos;

    private void OnCollisionStay2D(Collision2D c)
    {
        if (c.gameObject.tag.Equals("Player1"))
        {
            if (Mathf.Abs(hInput.GetAxis("P1_Vertical")) > threshold)
            {
                c.gameObject.layer = 14;
                //c.gameObject.GetComponent<>();
            }
        }
        else if (c.gameObject.tag.Equals("Player2"))
        {
            if (Mathf.Abs(hInput.GetAxis("P2_Vertical")) > threshold)
            {
                c.gameObject.layer = 15;
            }
        }
    }
}
