using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdMovement : MonoBehaviour {

    private Animator animator;
    private Rigidbody2D rb;
    private Transform transform;
    private BoxCollider2D bc;
    private bool isCoroutineExecuting = false;
    public float idleTime = 1.0f;
    private bool isRight;

    public int minPeckNum = 1;
    public int maxPeckNum = 3;
    public int minMoveNum = 3;
    public int maxMoveNum = 10;

    //state = -1: need to choose a state
    //state = 0: pecking
    //state = 1: moving
    //state = 2: turning
    //else: idle
    private int state = -1;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        bc = GetComponent<BoxCollider2D>();

        //start facing either left or right
        int rand = Random.Range(0, 2);
        //Debug.Log(rand);
        if (rand == 0)
        {
            isRight = true;
            Vector3 scale = transform.localScale;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            isRight = false;
            Vector3 scale = transform.localScale;
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state == -1)
        {
            state = Random.Range(0, 4);
            if (state == 0)
                StartCoroutine("Peck");
            else if (state == 1)
                StartCoroutine("Move");
            else if (state == 2)
                StartCoroutine("Turn");
            else
                StartCoroutine("Idle");
        }
    }

    IEnumerator Peck()
    {
        int peckNum = Random.Range(minPeckNum, maxPeckNum + 1);
        
        while (peckNum > 0)
        {
            animator.SetInteger("peck", peckNum);
            peckNum--;
            yield return new WaitForSeconds(0.15f);
        }
        animator.SetInteger("peck", 0);

        state = -1;
    }

    IEnumerator Move()
    {

        int stepNum = Random.Range(minMoveNum, maxMoveNum + 1);
        while (stepNum > 0)
        {
            if (isRight)
            {
                rb.velocity = new Vector2(0.5f,1);
            }
            else
            {
                rb.velocity = new Vector2(-0.5f, 1);
            }
            stepNum--;
            yield return new WaitForSeconds(0.2f);

            //check if inside birdBoundary
            Collider2D[] cols = Physics2D.OverlapBoxAll(bc.bounds.center, bc.bounds.extents, 0.0f, LayerMask.GetMask("birdBoundary"));
            foreach (Collider2D c in cols)
            {
                ModifiedTurn();
                if (isRight)
                {
                    rb.velocity = new Vector2(0.5f, 1);
                }
                else
                {
                    rb.velocity = new Vector2(-0.5f, 1);
                }
                yield return new WaitForSeconds(0.2f);
            }

            
        }

        state = -1;
    }

    IEnumerator Turn()
    {
        animator.SetTrigger("turn");

        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(-(scale.x), scale.y, scale.z);
        isRight = !isRight;

        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("idle");

        state = -1;
    }

    IEnumerator Idle()
    {
        yield return new WaitForSeconds(idleTime);
        state = -1;
    }

    private void ModifiedTurn()
    {
        animator.SetTrigger("turn");

        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(-(scale.x), scale.y, scale.z);
        isRight = !isRight;

        animator.SetTrigger("idle");
    }
}
