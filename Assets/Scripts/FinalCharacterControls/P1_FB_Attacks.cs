using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class P1_FB_Attacks : CharAttacks {

    private P1_CharController controller;
    private CharacterStats stats;
    private CharacterStats enemyStats;
    private GameObject fireball;
    private float knockbackMult;

    private void Start()
    {
        controller = gameObject.GetComponent<P1_CharController>();
        stats = gameObject.GetComponent<CharacterStats>();
    }

    public override void BackAir()
    {
        //Debug.Log("backAir");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[8].bounds.center, stats.hitBoxes[8].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
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
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.bAirLaunch.x * gameObject.transform.localScale.x * -1, stats.bAirLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.bAirStun;
                    enemyStats.damagePercent += stats.bAirDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                    //Debug.Log(c.name);
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= stats.bAirDamage;
                }
            }
        }
        controller.moveLag = stats.bAirLag;
    }

    public override void DownAir()
    {
        //Debug.Log("downAir");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[9].bounds.center, stats.hitBoxes[9].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
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
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.dAirLaunch.x * gameObject.transform.localScale.x, stats.dAirLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.dAirStun;
                    enemyStats.damagePercent += stats.dAirDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                    //Debug.Log(c.name);
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
        //Debug.Log("downSmash");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[5].bounds.center, stats.hitBoxes[5].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
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
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.dSmashLaunch.x * gameObject.transform.localScale.x, stats.dSmashLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.dSmashStun;
                    enemyStats.damagePercent += stats.dSmashDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                    //Debug.Log(c.name);
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
        //Debug.Log("downTilt");
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
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.dTiltLaunch.x * gameObject.transform.localScale.x, stats.dTiltLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.dTiltStun;
                    enemyStats.damagePercent += stats.dTiltDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                    //Debug.Log(c.name);
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= stats.dTiltDamage;
                }
            }
        }
        controller.moveLag = stats.dTiltLag;
    }

    public override void ForwardAir()
    {
        //Debug.Log("forwardAir");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[7].bounds.center, stats.hitBoxes[7].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
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
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.fAirLaunch.x * gameObject.transform.localScale.x, stats.fAirLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.fAirStun;
                    enemyStats.damagePercent += stats.fAirDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                    //Debug.Log(c.name);
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
        //Debug.Log("jab");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[6].bounds.center, stats.hitBoxes[6].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
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
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.jabLaunch.x * gameObject.transform.localScale.x, stats.jabLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.jabStun;
                    enemyStats.damagePercent += stats.jabDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                    //Debug.Log(c.name);
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
        //Debug.Log("neutralAir");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[11].bounds.center, stats.hitBoxes[11].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
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
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.nAirLaunch.x * gameObject.transform.localScale.x, stats.nAirLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.nAirStun;
                    enemyStats.damagePercent += stats.nAirDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                    //Debug.Log(c.name);
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
        //Debug.Log("sideSmash");
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
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.sSmashLaunch.x * gameObject.transform.localScale.x, stats.sSmashLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.sSmashStun;
                    enemyStats.damagePercent += stats.sSmashDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                    //Debug.Log(c.name);
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
        if (controller.facingRight)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.sideSpecialForce, 0);
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.sideSpecialForce * -1, 0);
        }

        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[12].bounds.center, stats.hitBoxes[12].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player2"))
            {
                controller.multiHit = true;

                enemyStats = c.gameObject.GetComponent<CharacterStats>();
                if (!enemyStats.isShielding && !enemyStats.isInvincible)
                {
                    if (controller.hit == false)
                    {
                        c.GetComponent<Animator>().SetTrigger("hurt");
                    }
                    controller.hit = true;

                    knockbackMult = controller.CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.sSpecialLaunch.x * gameObject.transform.localScale.x, stats.sSpecialLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.sSpecialStun;
                    enemyStats.damagePercent += stats.sSpecialDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                    //Debug.Log(c.name);
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= stats.sSpecialDamage;
                }
            }
        }
        if (controller.activeAttackTimer < 0.01)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        controller.moveLag = stats.sSpecialLag;
    }

    public override void SideTilt()
    {
        //Debug.Log("sideTilt");
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
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.sTiltLaunch.x * gameObject.transform.localScale.x, stats.sTiltLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.sTiltStun;
                    enemyStats.damagePercent += stats.sTiltDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                    //Debug.Log(c.name);
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
            fireball = Instantiate(controller.projectile1, controller.rangedStartPoint.position, transform.rotation);
            fireball.GetComponent<fireball>().launchVector = stats.nSpecialLaunch;
        }
        else
        {
            fireball = Instantiate(controller.projectile1, controller.rangedStartPoint.position, transform.rotation);
            Vector3 vec = fireball.transform.localScale;
            vec.x *= -1;
            fireball.transform.localScale = vec;
            fireball.GetComponent<fireball>().launchVector = new Vector2(stats.nSpecialLaunch.x * -1, stats.nSpecialLaunch.y);
        }
        fireball.GetComponent<fireball>().enemyTag = "Player2";
        fireball.GetComponent<fireball>().damage = stats.nSpecialDamage;
        fireball.GetComponent<fireball>().fbStun = stats.nSpecialStun;
        controller.moveLag = stats.nSpecialLag;
    }

    public override void UpAir()
    {
        //Debug.Log("upAir");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[10].bounds.center, stats.hitBoxes[10].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
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
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.uAirLaunch.x * gameObject.transform.localScale.x, stats.uAirLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.uAirStun;
                    enemyStats.damagePercent += stats.uAirDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                    //Debug.Log(c.name);
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
        //Debug.Log("upSmash");
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
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.uSmashLaunch.x * gameObject.transform.localScale.x, stats.uSmashLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.uSmashStun;
                    enemyStats.damagePercent += stats.uSmashDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                    //Debug.Log(c.name);
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= stats.uSmashDamage;
                }
            }
        }
        controller.moveLag = stats.uSmashLag;
    }

    public override void UpSpecial()
    {

        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, (stats.upSpecialForce));
        controller.freeFall = true;

        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[13].bounds.center, stats.hitBoxes[13].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
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
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.uSpecialLaunch.x * gameObject.transform.localScale.x, stats.uSpecialLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.uSmashStun;
                    enemyStats.damagePercent += stats.uSmashDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                    //Debug.Log(c.name);
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= stats.uSpecialDamage;
                }
            }
        }
        controller.moveLag = stats.uSpecialLag; ;
    }

    public override void UpTilt()
    {
        //Debug.Log("upTilt");
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
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.uTiltLaunch.x * gameObject.transform.localScale.x, stats.uTiltLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.uTiltStun;
                    enemyStats.damagePercent += stats.uTiltDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                    //Debug.Log(c.name);
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
