using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_floorDetection : MonoBehaviour {

    private AICharacterController controller;
    private CharacterStats stats;
    private float groundCheck;
    private float groundDetTime = 0.1f;
    private Animator myAnimator;

    private void Awake()
    {
        controller = gameObject.GetComponentInParent<AICharacterController>();
        stats = gameObject.GetComponentInParent<CharacterStats>();
        groundCheck = 0;
        myAnimator = gameObject.GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag.Equals("platform"))
        {
            groundCheck += Time.deltaTime;
            //Debug.Log(groundCheck);
        }
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        if (c.gameObject.tag.Equals("platform"))
        {
            if (groundCheck >= groundDetTime)
            {
                controller.grounded = true;
                myAnimator.SetBool("grounded", true);
                Debug.Log(controller.grounded);
            }
            else
            {
                groundCheck += Time.deltaTime;
                Debug.Log(groundCheck);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag.Equals("platform"))
        {
            controller.grounded = false;
            myAnimator.SetBool("grounded", false);
            groundCheck = 0;
        }
    }
}
