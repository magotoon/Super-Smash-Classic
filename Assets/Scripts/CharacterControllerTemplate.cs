using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerTemplate : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    [SerializeField]
    private float character_Jumpforce = 400f;
    [Range(0, 1)]
    [SerializeField]
    private LayerMask whatIsGround;
    private bool facingRight = true;
    private bool isGrounded = true;
    private bool hasDoubleJump = true;
    private float horizontal = 0f;
    private float vertical = 0f;


	// Use this for initialization
	void Start ()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void FixedUpdate()
    {
        
    }

    public void move(float horizontal, bool jump, bool doubleJump)
    {

    }

    void jump()
    {

    }

    void upB()
    {

    }

    void downB()
    {

    }
    void neutralB()
    {

    }

    void attack()
    {

    }

}
