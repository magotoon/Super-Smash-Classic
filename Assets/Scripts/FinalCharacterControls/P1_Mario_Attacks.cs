using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1_Mario_Attacks : CharAttacks {

    private P1_CharController controller;
    private CharacterStats stats;
    private CharacterStats enemyStats;
    private GameObject marioFireball;
    private GameObject marioExplode;
    private float knockbackMult;

    private Rigidbody2D rb;
    private bool inUpSpecial = false;

    private void Start()
    {
        controller = gameObject.GetComponent<P1_CharController>();
        stats = gameObject.GetComponent<CharacterStats>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        gameObject.GetComponent<Animator>().SetBool("inUpSpecial", false);
    }

    private void Update()
    {
        if (inUpSpecial && rb.velocity.y <= 0)
        {
            rb.gravityScale = 0.05f;
        }
        else if (!inUpSpecial)
        {
            rb.gravityScale = 1.0f;
        }

        if (!controller.freeFall && inUpSpecial)
        {
            inUpSpecial = false;
            gameObject.GetComponent<Animator>().SetBool("inUpSpecial", false);
            rb.gravityScale = 1.0f;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        //Debug.Log("Collided with " + col.gameObject.tag);
        if (inUpSpecial && rb.velocity.y <= 0)
        {
            //Debug.Log("Collided with " + col.gameObject.tag);
            inUpSpecial = false;
            gameObject.GetComponent<Animator>().SetBool("inUpSpecial", false);
        }
    }

    public override void BackAir()
    {
        //if (controller.timeInAttack >= 0.533)
        if (controller.timeInAttack >= 0.4f)
        {
            controller.multiHit = false;
            controller.hit = true;
            marioExplode = Instantiate(controller.projectile2, controller.rangedStartPoint2.position, transform.rotation);

            marioExplode.GetComponent<marioExplode>().enemyTag = "Player2";
            marioExplode.GetComponent<marioExplode>().damage = stats.bAirDamage;
            marioExplode.GetComponent<marioExplode>().stun = stats.bAirStun;
            marioExplode.GetComponent<marioExplode>().launchVector = new Vector2(stats.bAirLaunch.x * -1, stats.bAirLaunch.y);
            controller.moveLag = stats.bAirLag;
        }
    }

    public override void DownAir()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[21].bounds.center, stats.hitBoxes[21].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));

        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -3);

        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player2"))
            {
                controller.multiHit = false;
                controller.hit = true;
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 3);

                enemyStats = c.gameObject.GetComponent<CharacterStats>();
                if (!enemyStats.isShielding && !enemyStats.isInvincible)
                {
                    c.GetComponent<Animator>().SetTrigger("hurt");
                    knockbackMult = controller.CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.dAirLaunch.x * Mathf.Sign(gameObject.transform.localScale.x), stats.dAirLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.dAirStun;
                    enemyStats.damagePercent += stats.dAirDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= stats.dAirDamage;
                }
            }
        }
        controller.moveLag = stats.dAirLag;
    }

    public override void DownSmash()
    {
        Collider2D[] cols;

        if ((controller.timeInAttack >= 0.2 && controller.timeInAttack < 0.267) || (controller.timeInAttack >= 0.333 && controller.timeInAttack < 0.4) || (controller.timeInAttack >= 0.467 && controller.timeInAttack < 0.533) || (controller.timeInAttack >= 0.6 && controller.timeInAttack < 0.667))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[19].bounds.center, stats.hitBoxes[19].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
            processForwardSmash(cols);
        }
        else if ((controller.timeInAttack >= 0.267 && controller.timeInAttack < 0.333) || (controller.timeInAttack >= 0.4 && controller.timeInAttack < 0.467) || (controller.timeInAttack >= 0.533 && controller.timeInAttack < 0.6))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[18].bounds.center, stats.hitBoxes[18].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
            processForwardSmash(cols);
        }

    }
    public void processDownSmash(Collider2D[] cols)
    {
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player2"))
            {
                controller.multiHit = false;
                controller.hit = true;

                enemyStats = c.gameObject.GetComponent<CharacterStats>();
                if (!enemyStats.isShielding && !enemyStats.isInvincible)
                {
                    c.GetComponent<Animator>().SetTrigger("hurt");
                    knockbackMult = controller.CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.dSmashLaunch.x * Mathf.Sign(gameObject.transform.localScale.x), stats.dSmashLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.dSmashStun;
                    enemyStats.damagePercent += stats.dSmashDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= stats.dSmashDamage;
                }
            }
        }
        controller.moveLag = stats.dSmashLag;
    }

    public override void DownSpecial()
    {
        Special();
    }

    public override void DownTilt()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[2].bounds.center, stats.hitBoxes[2].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player2"))
            {
                controller.multiHit = false;
                controller.hit = true;

                enemyStats = c.gameObject.GetComponent<CharacterStats>();
                if (!enemyStats.isShielding && !enemyStats.isInvincible)
                {
                    c.GetComponent<Animator>().SetTrigger("hurt");
                    knockbackMult = controller.CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.dTiltLaunch.x * Mathf.Sign(gameObject.transform.localScale.x), stats.dTiltLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.dTiltStun;
                    enemyStats.damagePercent += stats.dTiltDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= stats.dTiltDamage;
                }
            }
            controller.moveLag = stats.jabLag;
        }
    }

    public override void ForwardAir()
    {
        Collider2D[] cols;

        if (controller.timeInAttack >= 0.367 && controller.timeInAttack < 0.433)
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[5].bounds.center, stats.hitBoxes[5].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
            processFair(cols);
        }
        else if (controller.timeInAttack >= 0.433 && controller.timeInAttack < 0.5)
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[6].bounds.center, stats.hitBoxes[6].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
            processFair(cols);
        }
    }
    public void processFair(Collider2D[] cols)
    {
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player2"))
            {
                controller.multiHit = false;
                controller.hit = true;

                enemyStats = c.gameObject.GetComponent<CharacterStats>();
                if (!enemyStats.isShielding && !enemyStats.isInvincible)
                {
                    c.GetComponent<Animator>().SetTrigger("hurt");
                    knockbackMult = controller.CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.fAirLaunch.x * Mathf.Sign(gameObject.transform.localScale.x), stats.fAirLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.fAirStun;
                    enemyStats.damagePercent += stats.fAirDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= stats.fAirDamage;
                }
            }
        }
        controller.moveLag = stats.fAirLag;
    }

    public override void Jab()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[0].bounds.center, stats.hitBoxes[0].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player2"))
            {
                controller.multiHit = false;
                controller.hit = true;

                enemyStats = c.gameObject.GetComponent<CharacterStats>();
                if (!enemyStats.isShielding && !enemyStats.isInvincible)
                {
                    c.GetComponent<Animator>().SetTrigger("hurt");
                    knockbackMult = controller.CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.jabLaunch.x * Mathf.Sign(gameObject.transform.localScale.x), stats.jabLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.jabStun;
                    enemyStats.damagePercent += stats.jabDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= stats.jabDamage;
                }
            }
        }
        controller.moveLag = stats.jabLag;
    }

    public override void NeutralAir()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[3].bounds.center, stats.hitBoxes[3].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player2"))
            {
                controller.multiHit = false;
                controller.hit = true;

                enemyStats = c.gameObject.GetComponent<CharacterStats>();
                if (!enemyStats.isShielding && !enemyStats.isInvincible)
                {
                    c.GetComponent<Animator>().SetTrigger("hurt");
                    knockbackMult = controller.CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.nAirLaunch.x * Mathf.Sign(gameObject.transform.localScale.x), stats.nAirLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.nAirStun;
                    enemyStats.damagePercent += stats.nAirDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= stats.nAirDamage;
                }
            }
        }
        controller.moveLag = stats.nAirLag;
    }

    public override void SideSmash()
    {
        Collider2D[] cols;

        if (controller.timeInAttack >= 0.367 && controller.timeInAttack < 0.433)
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[22].bounds.center, stats.hitBoxes[22].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
            processForwardSmash(cols);
        }
        else if (controller.timeInAttack >= 0.433 && controller.timeInAttack < 0.5)
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[23].bounds.center, stats.hitBoxes[23].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
            processForwardSmash(cols);
        }
    }
    public void processForwardSmash(Collider2D[] cols)
    {
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player2"))
            {
                controller.multiHit = false;
                controller.hit = true;

                enemyStats = c.gameObject.GetComponent<CharacterStats>();
                if (!enemyStats.isShielding && !enemyStats.isInvincible)
                {
                    c.GetComponent<Animator>().SetTrigger("hurt");
                    knockbackMult = controller.CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.sSmashLaunch.x * Mathf.Sign(gameObject.transform.localScale.x), stats.sSmashLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.sSmashStun;
                    enemyStats.damagePercent += stats.sSmashDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= stats.sSmashDamage;
                }
            }
        }
        controller.moveLag = stats.sSmashLag;
    }

    public override void SideSpecial()
    {

        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[20].bounds.center, stats.hitBoxes[20].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        Collider2D[] projCols = Physics2D.OverlapBoxAll(stats.hitBoxes[20].bounds.center, stats.hitBoxes[20].bounds.extents, 0.0f, LayerMask.GetMask("projectiles"));

        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player2"))
            {
                controller.multiHit = false;
                controller.hit = true;

                GameObject enemy = c.gameObject;
                enemyStats = enemy.GetComponent<CharacterStats>();
                if (!enemyStats.isShielding && !enemyStats.isInvincible)
                {
                    c.GetComponent<Animator>().SetTrigger("hurt");
                    //Debug.Log("Mario Cape hit enemy");
                    knockbackMult = controller.CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.sSpecialLaunch.x * gameObject.transform.localScale.x, stats.sSpecialLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.sSpecialStun;
                    enemyStats.damagePercent += stats.sSpecialDamage;
                    enemyStats.charState = CharacterStats.state.stun;

                    Vector3 enemnyVel = enemy.GetComponent<Rigidbody2D>().velocity;
                    enemnyVel = new Vector3(-(enemnyVel.x), enemnyVel.y, enemnyVel.z);


                    Transform enemyTra = enemy.GetComponent<Transform>();
                    Vector3 scale = enemyTra.localScale;
                    enemyTra.localScale = new Vector3(-(scale.x), scale.y, scale.z);
                    try
                    {
                        enemy.GetComponent<P2_CharController>().facingRight = !enemy.GetComponent<P2_CharController>().facingRight;
                    }
                    catch
                    {
                        try
                        {
                            enemy.GetComponent<samus_P2_CharController>().facingRight = !enemy.GetComponent<samus_P2_CharController>().facingRight;
                        }
                        catch
                        {
                            try
                            {
                                enemy.GetComponent<AI_LinkController>().facingRight = !enemy.GetComponent<AI_LinkController>().facingRight;
                            }
                            catch
                            {
                                try
                                {
                                    enemy.GetComponent<AI_MarioController>().facingRight = !enemy.GetComponent<AI_MarioController>().facingRight;
                                }
                                catch
                                {
                                    try
                                    {
                                        enemy.GetComponent<AI_FBController>().facingRight = !enemy.GetComponent<AI_FBController>().facingRight;
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            enemy.GetComponent<AI_SamusController>().facingRight = !enemy.GetComponent<AI_SamusController>().facingRight;
                                        }
                                        catch
                                        {
                                            //Debug.Log("wrong controller");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= stats.sSpecialDamage;
                }
            }
        }

        /*int i = 0;
        while (i < projCols.Length)
        {
            Debug.Log("Cape Hit:\t" + i + ". " + projCols[i].name + "\t" + projCols[i].gameObject.tag);
            i++;
        }*/

        string newEnemy = "Player2";
        foreach (Collider2D c in projCols) {
            controller.multiHit = false;
            controller.hit = true;
            //Debug.Log(c.gameObject.tag+" reflected by P1 cape");

            if (c.gameObject.tag.Equals("LinkArrow"))
            {
                //Debug.Log("Arrow reflect");
                GameObject proj = c.gameObject;
                Arrow projScript = proj.GetComponent<Arrow>();
                if (projScript.damage <= 50 && projScript.enemyTag != newEnemy)
                {
                    projScript.enemyTag = newEnemy;
                    projScript.damage *= 2;

                    Transform projTra = proj.GetComponent<Transform>();
                    Vector3 scale = projTra.localScale;
                    projTra.localScale = new Vector3(-(scale.x), scale.y, scale.z);

                    
                }
            }

            if (c.gameObject.tag.Equals("bomb1") || c.gameObject.tag.Equals("bomb2"))
            {
                GameObject proj = c.gameObject;
                Bomb projScript = proj.GetComponent<Bomb>();
                //Debug.Log("Bomb reflect");
                if (projScript.damage <= 50 && projScript.enemyTag != newEnemy)
                {
                    projScript.enemyTag = newEnemy;
                    projScript.damage *= 2;
                    Vector3 projVel = proj.GetComponent<Rigidbody2D>().velocity;
                    projVel = new Vector3(-(projVel.x), projVel.y, projVel.z);

                    Transform projTra = proj.GetComponent<Transform>();
                    Vector3 scale = projTra.localScale;
                    projTra.localScale = new Vector3(-(scale.x), scale.y, scale.z);
                }
            }

            if (c.gameObject.tag.Equals("FirebrandFire"))
            {
                GameObject proj = c.gameObject;
                fireball projScript = proj.GetComponent<fireball>();
                //Debug.Log("Firebrand fire reflect");
                if (projScript.damage <= 50 && projScript.enemyTag != newEnemy)
                {
                    projScript.enemyTag = newEnemy;
                    projScript.damage *= 2;

                    Transform projTra = proj.GetComponent<Transform>();
                    Vector3 scale = projTra.localScale;
                    projTra.localScale = new Vector3(-(scale.x), scale.y, scale.z);
                }
            }

            if (c.gameObject.tag.Equals("SamusCharged"))
            {
                GameObject proj = c.gameObject;
                chargedShotController projScript = proj.GetComponent<chargedShotController>();
                //Debug.Log("Samus Charged reflect");
                if (projScript.damage <= 50 && projScript.enemyTag != newEnemy)
                {
                    projScript.enemyTag = newEnemy;
                    projScript.damage *= 2;

                    Transform projTra = proj.GetComponent<Transform>();
                    Vector3 scale = projTra.localScale;
                    projTra.localScale = new Vector3(-(scale.x), scale.y, scale.z);
                }
            }

            if (c.gameObject.tag.Equals("SamusUncharged"))
            {
                GameObject proj = c.gameObject;
                unchargedShotController projScript = proj.GetComponent<unchargedShotController>();
                //Debug.Log("Samus Uncharged reflect");
                if (projScript.damage <= 50 && projScript.enemyTag != newEnemy)
                {
                    projScript.enemyTag = newEnemy;
                    projScript.damage *= 2;

                    Transform projTra = proj.GetComponent<Transform>();
                    Vector3 scale = projTra.localScale;
                    projTra.localScale = new Vector3(-(scale.x), scale.y, scale.z);
                }
            }

            /*if (c.gameObject.tag.Equals("SamusMiss"))
            {
                GameObject proj = c.gameObject;
                missleController projScript = proj.GetComponent<missleController>();
                //Debug.Log("Samus Missle reflect");
                if (projScript.damage <= 50 && projScript.enemyTag != newEnemy)
                {
                    projScript.enemyTag = newEnemy;
                    projScript.damage *= 2;

                    Transform projTra = proj.GetComponent<Transform>();
                    Vector3 scale = projTra.localScale;
                    projTra.localScale = new Vector3(-(scale.x), scale.y, scale.z);
                }
            }*/

            if (c.gameObject.tag.Equals("MarioFireball"))
            {
                GameObject proj = c.gameObject;
                marioFireball projScript = proj.GetComponent<marioFireball>();
                //Debug.Log("Mario fire reflect");
                if (projScript.damage <= 50 && projScript.enemyTag != newEnemy)
                {
                    projScript.enemyTag = newEnemy;
                    projScript.damage *= 2;
                    Vector3 projVel = proj.GetComponent<Rigidbody2D>().velocity;
                    projVel = new Vector3(-(projVel.x), projVel.y, projVel.z);

                    Transform projTra = proj.GetComponent<Transform>();
                    Vector3 scale = projTra.localScale;
                    projTra.localScale = new Vector3(-(scale.x), scale.y, scale.z);
                }
            }
        }
    }

    public override void SideTilt()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[1].bounds.center, stats.hitBoxes[1].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player2"))
            {
                controller.multiHit = false;
                controller.hit = true;

                enemyStats = c.gameObject.GetComponent<CharacterStats>();
                if (!enemyStats.isShielding && !enemyStats.isInvincible)
                {
                    c.GetComponent<Animator>().SetTrigger("hurt");
                    knockbackMult = controller.CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.sTiltLaunch.x * Mathf.Sign(gameObject.transform.localScale.x), stats.sTiltLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.sTiltStun;
                    enemyStats.damagePercent += stats.sTiltDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= stats.sTiltDamage;
                }
            }
        }
        controller.moveLag = stats.sTiltLag;
    }

    public override void Special()
    {
        controller.multiHit = false;
        controller.hit = true;

        if (controller.facingRight)
        {
            marioFireball = Instantiate(controller.projectile1, controller.rangedStartPoint.position, transform.rotation);
            Vector3 vec = marioFireball.transform.localScale;
            vec.x *= -1;
            marioFireball.transform.localScale *= -1;
            marioFireball.GetComponent<marioFireball>().launchVector = new Vector2(stats.nSpecialLaunch.x * -1, stats.nSpecialLaunch.y);
            
        }
        else
        {
            marioFireball = Instantiate(controller.projectile1, controller.rangedStartPoint.position, transform.rotation);
            marioFireball.GetComponent<marioFireball>().launchVector = stats.nSpecialLaunch;
        }
        marioFireball.GetComponent<marioFireball>().enemyTag = "Player2";
        marioFireball.GetComponent<marioFireball>().damage = stats.nSpecialDamage;
        marioFireball.GetComponent<marioFireball>().stun = stats.nSpecialStun;
        controller.moveLag = stats.nSpecialLag;
    }

    public override void UpAir()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[4].bounds.center, stats.hitBoxes[4].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player2"))
            {
                controller.multiHit = false;
                controller.hit = true;

                enemyStats = c.gameObject.GetComponent<CharacterStats>();
                if (!enemyStats.isShielding && !enemyStats.isInvincible)
                {
                    c.GetComponent<Animator>().SetTrigger("hurt");
                    knockbackMult = controller.CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.uAirLaunch.x * Mathf.Sign(gameObject.transform.localScale.x), stats.uAirLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.uAirStun;
                    enemyStats.damagePercent += stats.uAirDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= stats.uAirDamage;
                }
            }
        }
        controller.moveLag = stats.uAirLag;
    }

    public override void UpSmash()
    {
        Collider2D[] cols;

        //hitboxes on both extend and retract
        if ((controller.timeInAttack < 0.033) || (controller.timeInAttack >= 1))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[7].bounds.center, stats.hitBoxes[7].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.033 && controller.timeInAttack < 0.067) || (controller.timeInAttack >= 0.967 && controller.timeInAttack < 1))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[8].bounds.center, stats.hitBoxes[8].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.067 && controller.timeInAttack < 0.01) || (controller.timeInAttack >= 0.933 && controller.timeInAttack < .967))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[9].bounds.center, stats.hitBoxes[9].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.1 && controller.timeInAttack < 0.133) || (controller.timeInAttack >= 0.9 && controller.timeInAttack < .933))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[10].bounds.center, stats.hitBoxes[10].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.133 && controller.timeInAttack < 0.167) || (controller.timeInAttack >= 0.867 && controller.timeInAttack < .9))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[11].bounds.center, stats.hitBoxes[11].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.167 && controller.timeInAttack < 0.2) || (controller.timeInAttack >= 0.833 && controller.timeInAttack < .867))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[12].bounds.center, stats.hitBoxes[12].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.2 && controller.timeInAttack < 0.233) || (controller.timeInAttack >= 0.8 && controller.timeInAttack < .833))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[13].bounds.center, stats.hitBoxes[13].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.233 && controller.timeInAttack < 0.267) || (controller.timeInAttack >= 0.767 && controller.timeInAttack < .8))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[14].bounds.center, stats.hitBoxes[14].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.267 && controller.timeInAttack < 0.3) || (controller.timeInAttack >= 0.733 && controller.timeInAttack < .767))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[15].bounds.center, stats.hitBoxes[15].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.3 && controller.timeInAttack < 0.333) || (controller.timeInAttack >= 0.7 && controller.timeInAttack < .733))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[16].bounds.center, stats.hitBoxes[16].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[17].bounds.center, stats.hitBoxes[17].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }

        //hitboxes on just extend
        /*if (controller.timeInAttack < 0.033)
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[7].bounds.center, stats.hitBoxes[7].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.033 && controller.timeInAttack < 0.067))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[8].bounds.center, stats.hitBoxes[8].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.067 && controller.timeInAttack < 0.01))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[9].bounds.center, stats.hitBoxes[9].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.1 && controller.timeInAttack < 0.133))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[10].bounds.center, stats.hitBoxes[10].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.133 && controller.timeInAttack < 0.167))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[11].bounds.center, stats.hitBoxes[11].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.167 && controller.timeInAttack < 0.2))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[12].bounds.center, stats.hitBoxes[12].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.2 && controller.timeInAttack < 0.233))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[13].bounds.center, stats.hitBoxes[13].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.233 && controller.timeInAttack < 0.267))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[14].bounds.center, stats.hitBoxes[14].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.267 && controller.timeInAttack < 0.3))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[15].bounds.center, stats.hitBoxes[15].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if ((controller.timeInAttack >= 0.3 && controller.timeInAttack < 0.333))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[16].bounds.center, stats.hitBoxes[16].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if (((controller.timeInAttack >= 0.333 && controller.timeInAttack < 0.7)))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[17].bounds.center, stats.hitBoxes[17].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else
        {
            cols = null;
        }*/


        if (cols != null)
        {
            foreach (Collider2D c in cols)
            {
                if (c.gameObject.tag.Equals("Player2"))
                {
                    controller.multiHit = false;
                    controller.hit = true;

                    enemyStats = c.gameObject.GetComponent<CharacterStats>();
                    if (!enemyStats.isShielding && !enemyStats.isInvincible)
                    {
                        c.GetComponent<Animator>().SetTrigger("hurt");
                        knockbackMult = controller.CalculateKnockbackMult(c);
                        c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.uSmashLaunch.x * Mathf.Sign(gameObject.transform.localScale.x), stats.uSmashLaunch.y) * knockbackMult;
                        enemyStats.stunTime = stats.uSmashStun;
                        enemyStats.damagePercent += stats.uSmashDamage;
                        enemyStats.charState = CharacterStats.state.stun;
                    }
                    else if (enemyStats.isShielding)
                    {
                        enemyStats.shieldHealth -= stats.uSmashDamage;
                    }
                }
            }
        }
        controller.moveLag = stats.uSmashLag;
    }

    public override void UpSpecial()
    {
        controller.multiHit = false;
        controller.hit = true;
        rb.velocity = new Vector2(0, (stats.upSpecialForce));
        controller.moveLag = stats.uSpecialLag;
        controller.freeFall = true;

        gameObject.GetComponent<Animator>().SetBool("inUpSpecial", true);
        inUpSpecial = true;
    }

    public override void UpTilt()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[24].bounds.center, stats.hitBoxes[24].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player2"))
            {
                controller.multiHit = false;
                controller.hit = true;

                enemyStats = c.gameObject.GetComponent<CharacterStats>();
                if (!enemyStats.isShielding && !enemyStats.isInvincible)
                {
                    c.GetComponent<Animator>().SetTrigger("hurt");
                    knockbackMult = controller.CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.uTiltLaunch.x * Mathf.Sign(gameObject.transform.localScale.x), stats.uTiltLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.uTiltStun;
                    enemyStats.damagePercent += stats.uTiltDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= stats.uTiltDamage;
                }
            }
        }
        controller.moveLag = stats.uTiltLag;
    }
}
