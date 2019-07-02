using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1_Link_Attacks : CharAttacks {

    private P1_CharController controller;
    private CharacterStats stats;
    private CharacterStats enemyStats;
    private GameObject arrow;
    private GameObject bomb;
    private float knockbackMult;

    public float[] fAttackTimes;

    private void Start()
    {
        controller = gameObject.GetComponent<P1_CharController>();
        stats = gameObject.GetComponent<CharacterStats>();
    }

    public override void BackAir()
    {
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
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.bAirLaunch.x * Mathf.Sign(gameObject.transform.localScale.x) * -1, stats.bAirLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.bAirStun;
                    enemyStats.damagePercent += stats.bAirDamage;
                    enemyStats.charState = CharacterStats.state.stun;
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
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[10].bounds.center, stats.hitBoxes[10].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player2"))
            {
                controller.multiHit = false;
                controller.hit = true;
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, 3);

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
        }
        controller.moveLag = stats.dTiltLag;
    }

    public override void ForwardAir()
    {
        Collider2D[] cols;

        if (controller.timeInAttack < fAttackTimes[0])
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[0].bounds.center, stats.hitBoxes[0].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if (controller.timeInAttack < fAttackTimes[1])
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[1].bounds.center, stats.hitBoxes[1].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if (controller.timeInAttack < fAttackTimes[2])
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[2].bounds.center, stats.hitBoxes[2].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else 
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[3].bounds.center, stats.hitBoxes[3].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }


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

        if (controller.timeInAttack < fAttackTimes[0])
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[0].bounds.center, stats.hitBoxes[0].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if (controller.timeInAttack < fAttackTimes[1])
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[1].bounds.center, stats.hitBoxes[1].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if (controller.timeInAttack < fAttackTimes[2])
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[2].bounds.center, stats.hitBoxes[2].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[3].bounds.center, stats.hitBoxes[3].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }

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
        controller.multiHit = false;
        controller.hit = true;

        GameObject[] bombs = GameObject.FindGameObjectsWithTag("bomb1");

        if (bombs.Length >= 2)
        {
            return;
        }

        if (controller.facingRight)
        {
            bomb = Instantiate(controller.projectile2, controller.rangedStartPoint.position, transform.rotation);
            bomb.GetComponent<Bomb>().launchVector = stats.sSpecialLaunch;
        }
        else
        {
            bomb = Instantiate(controller.projectile2, controller.rangedStartPoint.position, transform.rotation);
            Vector3 vec = bomb.transform.localScale;
            vec.x *= -1;
            bomb.transform.localScale = vec;
            bomb.GetComponent<Bomb>().launchVector = new Vector2(stats.sSpecialLaunch.x * -1, stats.sSpecialLaunch.y);
        }
        bomb.GetComponent<Bomb>().enemyTag = "Player2";
        bomb.GetComponent<Bomb>().damage = stats.sSpecialDamage;
        bomb.GetComponent<Bomb>().stun = stats.sSpecialStun;
        controller.moveLag = stats.sSpecialLag;
    }

    public override void SideTilt()
    {
        Collider2D[] cols;

        if (controller.timeInAttack < fAttackTimes[0])
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[0].bounds.center, stats.hitBoxes[0].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if (controller.timeInAttack < fAttackTimes[1])
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[1].bounds.center, stats.hitBoxes[1].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else if (controller.timeInAttack < fAttackTimes[2])
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[2].bounds.center, stats.hitBoxes[2].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[3].bounds.center, stats.hitBoxes[3].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }

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
            arrow = Instantiate(controller.projectile1, controller.rangedStartPoint.position, transform.rotation);
            arrow.GetComponent<Arrow>().launchVector = stats.nSpecialLaunch;
        }
        else
        {
            arrow = Instantiate(controller.projectile1, controller.rangedStartPoint.position, transform.rotation);
            Vector3 vec = arrow.transform.localScale;
            vec.x *= -1;
            arrow.transform.localScale *= -1;
            arrow.GetComponent<Arrow>().launchVector = new Vector2(stats.nSpecialLaunch.x * -1, stats.nSpecialLaunch.y);
        }
        arrow.GetComponent<Arrow>().enemyTag = "Player2";
        arrow.GetComponent<Arrow>().damage = stats.nSpecialDamage;
        arrow.GetComponent<Arrow>().stun = stats.nSpecialStun;
        controller.moveLag = stats.nSpecialLag;
    }

    public override void UpAir()
    {
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
        controller.moveLag = stats.uSmashLag;
    }

    public override void UpSpecial()
    {
        controller.multiHit = false;
        controller.hit = true;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, (stats.upSpecialForce));

        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[13].bounds.center, stats.hitBoxes[13].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player2"))
            {
                enemyStats = c.gameObject.GetComponent<CharacterStats>();
                if (!enemyStats.isShielding && !enemyStats.isInvincible)
                {
                    c.GetComponent<Animator>().SetTrigger("hurt");
                    knockbackMult = controller.CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.uSpecialLaunch.x * Mathf.Sign(gameObject.transform.localScale.x), stats.uSpecialLaunch.y) * knockbackMult;
                    enemyStats.stunTime = stats.uSpecialStun;
                    enemyStats.damagePercent += stats.uSpecialDamage;
                    enemyStats.charState = CharacterStats.state.stun;
                }
                else if (enemyStats.isShielding)
                {
                    enemyStats.shieldHealth -= stats.uSpecialDamage;
                }
            }
        }

        controller.moveLag = stats.uSpecialLag;
        controller.freeFall = true;
    }

    public override void UpTilt()
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
