using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    [SerializeField]
    private float movementSpeed;



	// Use this for initialization
	void Start ()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Debug.Log("Horizontal is = " + horizontal);
        handleMovement(horizontal);
	}

    private void handleMovement(float horizontal)
    {
        
        myRigidbody.velocity = new Vector2(horizontal, myRigidbody.velocity.y);
    }

    private bool isGrounded()
    {
        if (myRigidbody.velocity.y <= 0)
        {//if we are falling or standing still

        }
        return true;
    }
}
