﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SamusFloorDetect : MonoBehaviour
{

    private AI_SamusController controller;
    private CharacterStats stats;
    private float groundCheck;
    private float groundDetTime = 0.03f;

    private void Awake()
    {
        controller = gameObject.GetComponentInParent<AI_SamusController>();
        stats = gameObject.GetComponentInParent<CharacterStats>();
        groundCheck = 0;
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
                controller.jumpsLeft = stats.numberOfExtraJumps;

                //Debug.Log(controller.grounded);
            }
            else
            {
                groundCheck += Time.deltaTime;
                //Debug.Log(groundCheck);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag.Equals("platform"))
        {
            controller.grounded = false;
            groundCheck = 0;
        }
    }
}
