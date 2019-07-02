using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    private Rigidbody2D myRigidBody;
    private CharacterStats enemyStats;
    private Animator animator;
    private bool exploding = false;
    private bool hit = false;

    public float projectileSpeed = 2;
    private float timeTillDestroy = 7;
    public float damageTime = 0.1f;
    public float stun = 0.5f;
    public string enemyTag = "";
    public Vector2 launchVector = new Vector2(0, 0);
    public int damage = 0;
    public Collider2D explotionRadious;

    public bool sounded = false;
    public AudioClip audio_explotion;
    public AudioSource playerAudio;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (Mathf.Sign(transform.localScale.x) == -1)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed * -1, 0);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, 0);
        }
    }

    void Update () {
        timeTillDestroy -= Time.deltaTime;

        if (timeTillDestroy > 5)
        {

        }
        else if (timeTillDestroy > 3.7 && timeTillDestroy <= 4)
        {
            animator.SetTrigger("almost");
        }
        else if (timeTillDestroy > 0.8 && timeTillDestroy <= 1.1)
        {
            if (!sounded)
            {
                playerAudio.clip = audio_explotion;
                playerAudio.Play();
                sounded = true;
            }
            animator.SetTrigger("boom");
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            if (damageTime > 0)
            {
                //Debug.Log("exploding");
                exploding = true;
                damageTime -= Time.deltaTime;
            }
            else exploding = false;
        }
        else if (timeTillDestroy < 0)
        {
            exploding = false;
            Destroy(gameObject);
        }
		
	}

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag.Equals(enemyTag))
        {
            animator.SetTrigger("almost");
            if (timeTillDestroy > 1.2f)
            {
                timeTillDestroy = 1.2f;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        if (exploding)
        {
            if (c.gameObject.tag.Equals("Player1") || c.gameObject.tag.Equals("Player2"))
            {
                //Debug.Log(exploding);
                enemyStats = c.gameObject.GetComponent<CharacterStats>();
                if (!enemyStats.isShielding && !enemyStats.isInvincible)
                {
                    if (hit == false)
                    {
                        c.GetComponent<Animator>().SetTrigger("hurt");
                    }
                    hit = true;

                    float knockbackMult = CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = launchVector * knockbackMult;
                    enemyStats.stunTime = stun;
                    enemyStats.damagePercent += damage;
                    enemyStats.charState = CharacterStats.state.stun;
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= damage;
                }
            }
        }
    }

    float CalculateKnockbackMult(Collider2D c)
    {
        CharacterStats stats = c.gameObject.GetComponent<CharacterStats>();
        return (c.gameObject.GetComponent<CharacterStats>().damagePercent < stats.knockbackPercentFloor) ? (stats.knockbackPercentFloor / 100) : ((float)c.gameObject.GetComponent<CharacterStats>().damagePercent / 100);
    }
}
