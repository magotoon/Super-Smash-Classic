﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2_Samus_Attacks : CharAttacks
{
    private samus_P2_CharController controller;
    private CharacterStats stats;
    private CharacterStats enemyStats;
    private GameObject unchargedShot;
    private GameObject chargedShot;
    private float knockbackMult;
    public float amountCharged = 0;
    private bool finishedCharging;
    public Time maxTimeCharging;

    private void Start()
    {
        controller = gameObject.GetComponent<samus_P2_CharController>();
        stats = gameObject.GetComponent<CharacterStats>();
    }


    public override void BackAir()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[16].bounds.center, stats.hitBoxes[16].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
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
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[17].bounds.center, stats.hitBoxes[17].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
            {
                controller.multiHit = false;
                controller.hit = true;

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


        if ((controller.timeInAttack < 0.033) || (controller.timeInAttack >= 1))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[12].bounds.center, stats.hitBoxes[12].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }
        else
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[13].bounds.center, stats.hitBoxes[13].bounds.extents, 0.0f, LayerMask.GetMask("Player2"));
        }

        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
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
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[2].bounds.center, stats.hitBoxes[2].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
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

        cols = Physics2D.OverlapBoxAll(stats.hitBoxes[15].bounds.center, stats.hitBoxes[15].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));


        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
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
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[14].bounds.center, stats.hitBoxes[14].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
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
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[19].bounds.center, stats.hitBoxes[19].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
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

        //hitboxes on both extend and retract
        if ((controller.timeInAttack < 0.033) || (controller.timeInAttack >= 1))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[3].bounds.center, stats.hitBoxes[3].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        }
        else if ((controller.timeInAttack >= 0.033 && controller.timeInAttack < 0.3))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[4].bounds.center, stats.hitBoxes[4].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        }
        else
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[5].bounds.center, stats.hitBoxes[5].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        }
 
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
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
        if (controller.facingRight)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.sideSpecialForce, 0);
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.sideSpecialForce * -1, 0);
        }

        Collider2D[] cols;

        if ((controller.timeInAttack < 0.033) || (controller.timeInAttack >= 1))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[20].bounds.center, stats.hitBoxes[20].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        }
        else if ((controller.timeInAttack >= 0.033 && controller.timeInAttack < 0.067) || (controller.timeInAttack >= 0.967 && controller.timeInAttack < 1))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[21].bounds.center, stats.hitBoxes[21].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        }
        else if ((controller.timeInAttack >= 0.067 && controller.timeInAttack < 0.01) || (controller.timeInAttack >= 0.933 && controller.timeInAttack < .967))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[22].bounds.center, stats.hitBoxes[22].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        }
        else
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[23].bounds.center, stats.hitBoxes[23].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        }





        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
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
        Collider2D[] cols;

        cols = Physics2D.OverlapBoxAll(stats.hitBoxes[0].bounds.center, stats.hitBoxes[0].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));

        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
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
        amountCharged = controller.chargePercent;
        // check if the charging is done if done, then check charge percentage, if over 50%  then shoot charged shot, if not, shoot uncharged. 

        if (amountCharged > .5)
        {
            if (controller.facingRight)
            {
                controller.chargePercent = 0;
                //Debug.Log("firing charged shot");
                chargedShot = Instantiate(controller.projectile1, controller.rangedStartPoint.position, transform.rotation);
                chargedShot.GetComponent<chargedShotController>().launchVector = stats.nSpecialLaunch;
            }
            else
            {
                chargedShot = Instantiate(controller.projectile1, controller.rangedStartPoint.position, transform.rotation);
                chargedShot.transform.localScale *= -1;
                chargedShot.GetComponent<chargedShotController>().launchVector = new Vector2(stats.nSpecialLaunch.x * -1, stats.nSpecialLaunch.y);
            }
            chargedShot.GetComponent<chargedShotController>().enemyTag = "Player1";
            chargedShot.GetComponent<chargedShotController>().damage = stats.nSpecialDamage * 3;
            chargedShot.GetComponent<chargedShotController>().fbStun = stats.nSpecialStun * 2;
            controller.moveLag = stats.nSpecialLag;
            controller.chargePercent = 0;
        }
        else
        {
            controller.chargePercent = 0;
            if (controller.facingRight)
            {
                //Debug.Log("firing uncharged shot");
                unchargedShot = Instantiate(controller.projectile2, controller.rangedStartPoint.position, transform.rotation);
                unchargedShot.GetComponent<unchargedShotController>().launchVector = stats.nSpecialLaunch;
            }
            else
            {
                unchargedShot = Instantiate(controller.projectile2, controller.rangedStartPoint.position, transform.rotation);
                unchargedShot.transform.localScale *= -1;
                unchargedShot.GetComponent<unchargedShotController>().launchVector = new Vector2(stats.nSpecialLaunch.x * -1, stats.nSpecialLaunch.y);
            }
            unchargedShot.GetComponent<unchargedShotController>().enemyTag = "Player1";
            unchargedShot.GetComponent<unchargedShotController>().damage = stats.nSpecialDamage;
            unchargedShot.GetComponent<unchargedShotController>().fbStun = stats.nSpecialStun;
            controller.moveLag = stats.nSpecialLag;
            controller.chargePercent = 0;
        }
    }

    public override void UpAir()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[18].bounds.center, stats.hitBoxes[18].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
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

        if ((controller.timeInAttack < 0.033) || (controller.timeInAttack >= 1))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[6].bounds.center, stats.hitBoxes[6].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        }
        else if ((controller.timeInAttack >= 0.033 && controller.timeInAttack < 0.267))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[7].bounds.center, stats.hitBoxes[7].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        }
        else if ((controller.timeInAttack >= 0.267 && controller.timeInAttack < 0.3))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[8].bounds.center, stats.hitBoxes[8].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        }
        else if ((controller.timeInAttack >= 0.3 && controller.timeInAttack < 0.43))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[9].bounds.center, stats.hitBoxes[9].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        }
        else if ((controller.timeInAttack >= 0.43 && controller.timeInAttack < 0.567))
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[10].bounds.center, stats.hitBoxes[10].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        }
        else
        {
            cols = Physics2D.OverlapBoxAll(stats.hitBoxes[11].bounds.center, stats.hitBoxes[11].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        }
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
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

        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[24].bounds.center, stats.hitBoxes[24].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
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
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[1].bounds.center, stats.hitBoxes[1].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
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

    public void ChargingProjectile()
    {







    }

    public void hasFinishedCharging(bool has)
    {
        finishedCharging = has;
    }
}
