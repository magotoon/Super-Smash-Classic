using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class marioExplode : MonoBehaviour
{
    public float timeTillDestroy = 1;
    public float stun = 0.5f;
    public string enemyTag = "";
    public int damage = 0;
    public Vector2 launchVector = new Vector2(0, 0);
    private CharacterStats enemyStats;

    void Update()
    {

        if (timeTillDestroy <= 0)
        {// object times out.
            Destroy(gameObject);
        }
        else
        {
            timeTillDestroy -= Time.deltaTime;
        }
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

    float CalculateKnockbackMult(Collider2D c)
    {
        CharacterStats stats = c.gameObject.GetComponent<CharacterStats>();
        return (c.gameObject.GetComponent<CharacterStats>().damagePercent < stats.knockbackPercentFloor) ? (stats.knockbackPercentFloor / 100) : ((float)c.gameObject.GetComponent<CharacterStats>().damagePercent / 100);
    }
}
