using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterController : MonoBehaviour {

    public GameObject AiSpawn;
    public GameObject AiShield;
    private CharacterStats stats;
    private Animator myAnimator;
    private bool shielding = false;
    private Rigidbody2D body;
    private float respawnTimer = 0;
    public bool invincible { get; set; }
    public bool grounded;
    private float startTimer;
    private float knockbackMult = 0.1f;
    private float moveLag = 0;

    // Use this for initialization
    void Start ()
    {
        AiSpawn = GameObject.FindGameObjectWithTag("P2_Spawn");
        AiShield = GameObject.FindGameObjectWithTag("P2_Shield");
        AiShield.SetActive(false);
        stats = gameObject.GetComponent<CharacterStats>();
        body = gameObject.GetComponent<Rigidbody2D>();
        invincible = false;
        myAnimator = gameObject.GetComponent<Animator>();
        startTimer = stats.startTime;

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void Shield()
    {
        AiShield.SetActive(true);
        shielding = true;
        stats.isShielding = true;
    }

    void ShieldOff()
    {
        AiShield.SetActive(false);
        shielding = false;
        stats.isShielding = false;
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        Debug.Log(" just entered " + c.gameObject.layer + " collider");
        if (c.gameObject.tag.Equals("Blastzone"))
        {
            transform.position = AiSpawn.transform.position;
            stats.damagePercent = 0;
            stats.numberOfLives -= 1;
            body.velocity = new Vector2(0, 0);
            respawnTimer = stats.respawnTime;
            invincible = true;
        }
        else if (c.gameObject.tag.Equals("AIWalkStop"))
        {
            myAnimator.SetFloat("speed", 0f);
            myAnimator.SetTrigger("basicAttack");
        }
        else if (c.gameObject.tag.Equals("jumpZone"))
        {
            myAnimator.SetTrigger("jump");
            grounded = false;
        }
        else if (c.gameObject.tag.Equals("Player1"))
        {
            Debug.Log("Mortal Kombat");

            // choice tree for attacks, based off of the velocity of the player 
            /*
             if the player is moving in a positive y vector, then throw an up attack
             '' down attack
             '' if the are moving away in x then throw f attack
             '' if moving closer, then use neutral attack or jab
             '' 
             
             
             */
            myAnimator.SetTrigger("basicAttack");
            if (!c.gameObject.GetComponent<CharacterStats>().isShielding)
            {
                if (moveLag <= 0)
                {
                    knockbackMult = CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.bAirLaunch.x * gameObject.transform.localScale.x * -1, stats.bAirLaunch.y) * knockbackMult;
                    c.gameObject.GetComponent<CharacterStats>().stunTime = stats.bAirStun;
                    c.gameObject.GetComponent<CharacterStats>().damagePercent += stats.bAirDamage;
                    Debug.Log(c.name);
                    moveLag = stats.jabLag;
                }
                else
                {// slow him down
                    moveLag -= Time.deltaTime;
                }
                
            }
            else
            {
                c.gameObject.GetComponent<CharacterStats>().shieldHealth -= stats.bAirDamage;
            }
            
        }

    }
    float CalculateKnockbackMult(Collider2D c)
    {
        return (c.gameObject.GetComponent<CharacterStats>().damagePercent < stats.knockbackPercentFloor) ? (stats.knockbackPercentFloor / 100) : ((float)c.gameObject.GetComponent<CharacterStats>().damagePercent / 100);
    }
}
