using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorAnimation : MonoBehaviour {

    private Animator animator;
    private bool open;
    private bool isCoroutineExecuting = false;

    //door opens/closes after a random time between min and max
    public int minTime = 2;
    public int maxTime = 6;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();

        int rand = Random.Range(0,2);
        if (rand == 0)
        {
            animator.SetBool("Open", false);
            open = false;
        }
        else
        {
            animator.SetBool("Open", true);
            open = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!isCoroutineExecuting)
        {
            isCoroutineExecuting = true;

            int time = Random.Range(minTime, maxTime + 1);
            StartCoroutine(ExecuteAfterTime(time));
        }
    }


    //random delay to get the door to open and close
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        if (open)
        {
            animator.SetBool("Open", false);
            open = false;
        }
        else
        {
            animator.SetBool("Open", true);
            open = true;
        }


        isCoroutineExecuting = false;
    }
}
