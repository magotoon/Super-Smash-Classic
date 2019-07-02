using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
    public Transform spawnPoint;
    public int lives;
    private Rigidbody2D myRigidbody;
    private Animator characterAnimator;
    public float movementSpeed;
    private bool facingRight = true;
    public Transform groundCheck;
    public float groundRadius;
    private bool isPlayerGrounded;
    public LayerMask whatIsGround;
    [SerializeField]
    public float jumpForce;
    private bool hasJumped = false;
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping = false;
    //end of movement variables



    // attack variables.
    private float timeBtwAttack;
    private float timeBtwShots;
    public float startTimeBtwShots;
    public float startTimeBtwAttack;
    public Transform attackPosFront;
    public Transform attackPosAbove;
    public float attackRangeFront;
    public float attackRangeAbove;
    public LayerMask whatIsEnemy;
    Collider2D[] enemyToDamage;
    public Transform rangedStartPoint;
    public GameObject projectile;
    public float projectileSpeed;
    private GameObject fireball;
    public bool AIPlayerBool = false; // TODO: Manuel if you can set this bool to true if the AI is player 2

    // end of attack variables





    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        characterAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        if(timeBtwAttack <= 0 )
        {
            if(hInput.GetButton("P1_Special")) // TODO: if not a charge shot make it work with GetButtonDown()
            {// if the player hits the special attack button
                characterAnimator.SetTrigger("specialAttack");
                if(Input.GetKey(KeyCode.W))
                {// if the user is holding the up key then do recovery
                    characterAnimator.SetBool("Dup", true);
                    enemyToDamage = Physics2D.OverlapCircleAll(attackPosAbove.position, attackRangeAbove, whatIsEnemy);
                }
                else
                {
                    characterAnimator.SetBool("Dup", false);
                }
                
                if(characterAnimator.GetFloat("speed") > 0 ) //TODO: check for input from the stick/key not speed
                {// if the user is moving due to horizontal input do forward special
                    characterAnimator.SetBool("Dfor",true);
                    enemyToDamage = Physics2D.OverlapCircleAll(attackPosFront.position, attackRangeFront, whatIsEnemy);
                }
                else
                {// if the user is using the neutral special attack
                    characterAnimator.SetBool("Dfor", false);
                    if (timeBtwShots <= 0)
                    {
                        if(facingRight)
                        {
                            fireball = Instantiate(projectile, rangedStartPoint.position, transform.rotation);
                            
                        }
                        else
                        {
                           fireball = Instantiate(projectile, rangedStartPoint.position, transform.rotation);
                           fireball.transform.localScale *= -1;
                        }
                    
                    }
                    else
                    {
                        timeBtwShots -= Time.deltaTime;
                    }
                }

            }
            if(hInput.GetButton("P1_Attack")) // TODO: Make it work with GetButtonDown
            {// if the player hits the basic attack button
                characterAnimator.SetTrigger("basicAttack");
                if(Input.GetKey(KeyCode.W)) //TODO:Patrick make this for pos y inputs to make it more dynamic
                {// if the player is holding up on the controller
                    characterAnimator.SetBool("Dup",true);
                    enemyToDamage = Physics2D.OverlapCircleAll(attackPosAbove.position, attackRangeAbove, whatIsEnemy);
                    
                }
                else
                {
                    characterAnimator.SetBool("Dup", false);
                    enemyToDamage = Physics2D.OverlapCircleAll(attackPosFront.position,attackRangeFront,whatIsEnemy);
                }

                 
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log("isground = " + isPlayerGrounded.ToString());
        float horizontal = Input.GetAxis("Horizontal");
        handleMovement(horizontal);
        flip(horizontal);
        isPlayerGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        characterAnimator.SetBool("grounded", isPlayerGrounded);
        myRigidbody.velocity = new Vector2(movementSpeed * horizontal, myRigidbody.velocity.y);
    }

    private void handleMovement(float horizontal)
    { 
        characterAnimator.SetFloat("speed" , Mathf.Abs(horizontal)); 
    }

    private void flip(float horizontal)
    {
        if(horizontal < 0 && facingRight || horizontal > 0 && !facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosAbove.position, attackRangeAbove);
        Gizmos.DrawWireSphere(attackPosFront.position, attackRangeFront);
    }

    private void HandleInput()
    {
        if(hInput.GetButtonDown("P1_Jump") && isPlayerGrounded)      
        {
            isJumping = true;
            hasJumped = true;
            characterAnimator.SetTrigger("jump");
            jumpTimeCounter = jumpTime;
            //myRigidbody.velocity = Vector2.up * jumpForce;
        }

        if(hInput.GetButton("P1_Jump") && isJumping == true )
        {
            if(jumpTimeCounter > 0)
            {
                characterAnimator.SetTrigger("jump");
                myRigidbody.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
                hasJumped = false;
            }
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    private void OnDestroy()
    {
       
        
        GameObject Spawn = Instantiate(gameObject);
        Debug.Log(gameObject.name);
            if (gameObject.layer.Equals("Player1"))
            {
                gameEnds(1);
            }
            else if (gameObject.layer.Equals("Player2"))
            {
                if (AIPlayerBool)
                {
                    gameEnds(3);
                }
                else
                {
                    gameEnds(2);
                }

            }

        Spawn.transform.position = spawnPoint.position;
    }

    private void gameEnds(int playerWhoLost)
    {

        /*TODO:Manuel
         * If you can pass the players characters forward from the character select scene into the current one, and then pass that along in these mnethods to have it display the characters in a winning or losing artwork. that would cover winning and losing.
         * 
         */


        if(playerWhoLost == 1)
        {//if player 1 lost
            SceneManager.LoadScene("Player1Wins");
        }
        else if(playerWhoLost == 2)
        {// if player 2 lost 
            SceneManager.LoadScene("Player2Wins");
        }
        else
        {// if AI player lost
            SceneManager.LoadScene("AIWins");
        }
    }
}
