using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireball : MonoBehaviour
{
    private Rigidbody2D myRigidBody;
    public float projectileSpeed;
    public float timeTillDestroy;
    public float fbStun = 0.5f;
    public string enemyTag = "";
    public Vector2 launchVector = new Vector2(0,0);
    public int damage = 0;
    public Collider2D[] enemyToDamage;
    private CharacterStats enemyStats;

    // Use this for initialization
    void Start ()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(transform.localScale.x == -1)
        {
            transform.Translate( -1 * (projectileSpeed * Time.deltaTime), 0, 0);
        }
        else
        {
            transform.Translate(projectileSpeed * Time.deltaTime,0,0);
        }

        if(timeTillDestroy <= 0 )
        {// object times out.
            Destroy(gameObject);
        }
        else
        {
            timeTillDestroy -= Time.deltaTime;
        }

       // if()
        
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag.Equals(enemyTag))
        {
            enemyStats = c.gameObject.GetComponent<CharacterStats>();
            if (!enemyStats.isShielding && !enemyStats.isInvincible)
            {
                c.GetComponent<Animator>().SetTrigger("hurt");
                float knockbackMult = CalculateKnockbackMult(c);
                c.GetComponent<Rigidbody2D>().velocity = launchVector * knockbackMult;
                enemyStats.stunTime = fbStun;
                enemyStats.damagePercent += damage;
                enemyStats.charState = CharacterStats.state.stun;
            }
            else if (enemyStats.isShielding)
            {
                enemyStats.shieldHealth -= damage;
            }

            Destroy(gameObject);
        }
        
    }

    float CalculateKnockbackMult(Collider2D c)
    {
        CharacterStats stats = c.gameObject.GetComponent<CharacterStats>();
        return (c.gameObject.GetComponent<CharacterStats>().damagePercent < stats.knockbackPercentFloor) ? (stats.knockbackPercentFloor / 100) : ((float)c.gameObject.GetComponent<CharacterStats>().damagePercent / 100);
    }
}

