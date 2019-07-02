using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2FirebrandController : MonoBehaviour
{

    public GameObject p2Spawn;
    public GameObject p2Shield;
    public GameObject fireball;
    public GameObject projectile;
    public Transform rangedStartPoint;
    private Animator myAnimator;
    public bool invincible { get; set; }
    public bool grounded { get; set; }
    public int jumpsLeft { get; set; }

    private bool facingRight = true;
    private bool shielding = false;
    private bool freeFall = false;
    private bool sideSecial = false;
    private float ivTimer = 0;
    private float respawnTimer = 0;
    private float startTimer;
    private float rSmashTimer = 0f;
    private float lSmashTimer = 0f;
    private float uSmashTimer = 0f;
    private float dSmashTimer = 0f;
    private float shortHopTimer = 0f;
    private float curSpeed;
    private float xAxis;
    private float yAxis;
    private Rigidbody2D body;
    private CharacterStats stats;
    private float moveLag = 0;
    private float knockbackMult = 0.1f;
    private SpriteRenderer mySpriteRenderer;


    void Start()
    {
        p2Spawn = GameObject.FindGameObjectWithTag("P2_Spawn");
        p2Shield = GameObject.FindGameObjectWithTag("P2_Shield");
        p2Shield.SetActive(false);
        gameObject.transform.position = p2Spawn.transform.position;
        body = gameObject.GetComponent<Rigidbody2D>();
        stats = gameObject.GetComponent<CharacterStats>();
        jumpsLeft = stats.numberOfExtraJumps;
        grounded = false;
        startTimer = stats.startTime;
        invincible = false;
        myAnimator = gameObject.GetComponent<Animator>();
        Physics2D.IgnoreLayerCollision(10, 11);
        mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        mySpriteRenderer.color = new Color(255, 255, 0, 255);
    }

    void Update()
    {

        if (ivTimer > 0)
        {
            ivTimer -= Time.deltaTime;

            if (ivTimer < 0.01)
            {
                invincible = false;
                ivTimer = 0;
            }
        }

        if (respawnTimer > 0)
        {
            body.velocity = Vector2.zero;
            sideSecial = false;
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            respawnTimer -= Time.deltaTime;

            if ((respawnTimer < 3 && PlayerInput()) || respawnTimer < 0.01)
            {
                gameObject.GetComponent<Rigidbody2D>().simulated = true;
                respawnTimer = 0;
                ivTimer = stats.ivFramesRespawnTime;
                rSmashTimer = 0;
                lSmashTimer = 0;
                uSmashTimer = 0;
                dSmashTimer = 0;
            }

            return;
        }

        if (startTimer > 0)
        {
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            startTimer -= Time.deltaTime;

            if (startTimer < 0.01)
            {
                gameObject.GetComponent<Rigidbody2D>().simulated = true;
                startTimer = 0;
                rSmashTimer = 0;
                lSmashTimer = 0;
                uSmashTimer = 0;
                dSmashTimer = 0;
            }
            return;
        }

        if (stats.shieldHealth <= 0)
        {
            stats.stunTime = stats.shieldBreakTime;
            stats.shieldHealth = stats.maxShieldHealth;
            ShieldOff();
        }

        if (stats.stunTime > 0)
        {
            stats.stunTime -= Time.deltaTime;
            return;
        }

        if (stats.damagePercent > 999)
        {
            stats.damagePercent = 999;
        }

        if (sideSecial)
        {
            stats.shieldHealth += Time.deltaTime;
            if (stats.shieldHealth > stats.maxShieldHealth)
            {
                stats.shieldHealth = stats.maxShieldHealth;
            }

            if (moveLag > 0)
            {
                if (facingRight)
                {
                    body.velocity = new Vector2(stats.sideSpecialForce, 0);
                }
                else
                {
                    body.velocity = new Vector2(stats.sideSpecialForce * -1, 0);
                }

                Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[12].bounds.center, stats.hitBoxes[12].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
                foreach (Collider2D c in cols)
                {
                    if (c.gameObject.tag.Equals("Player1"))
                    {
                        if (!c.gameObject.GetComponent<CharacterStats>().isShielding)
                        {
                            knockbackMult = CalculateKnockbackMult(c);
                            c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.sSpecialLaunch.x * gameObject.transform.localScale.x, stats.sSpecialLaunch.y) * knockbackMult;
                            c.gameObject.GetComponent<CharacterStats>().stunTime = stats.sSpecialStun;
                            c.gameObject.GetComponent<CharacterStats>().damagePercent += stats.sSpecialDamage;
                            Debug.Log(c.name);
                        }
                        else
                        {
                            c.gameObject.GetComponent<CharacterStats>().shieldHealth -= stats.sSpecialDamage;
                        }
                    }
                }
                moveLag -= Time.deltaTime;
                if (moveLag < 0.01)
                {
                    moveLag = -1;
                }
            }
            else
            {
                sideSecial = false;
                body.velocity = new Vector2(0, 0);
                moveLag = stats.sSpecialLag;
            }
        }
        else if ((moveLag > 0 || freeFall) && !grounded)
        {
            stats.shieldHealth += Time.deltaTime;
            if (stats.shieldHealth > stats.maxShieldHealth)
            {
                stats.shieldHealth = stats.maxShieldHealth;
            }

            myAnimator.SetBool("grounded", false);
            moveLag -= Time.deltaTime;
            if (moveLag < 0.01)
            {
                moveLag = -1;
            }

            xAxis = hInput.GetAxis("P2_Horizontal");

            body.velocity = new Vector2(xAxis * stats.maxAirSpeed, body.velocity.y);
            myAnimator.SetTrigger("walk");
            myAnimator.SetFloat("speed", stats.maxSpeed);
        }
        else if ((moveLag > 0 || freeFall) && grounded)
        {
            stats.shieldHealth += Time.deltaTime;
            if (stats.shieldHealth > stats.maxShieldHealth)
            {
                stats.shieldHealth = stats.maxShieldHealth;
            }

            myAnimator.SetBool("grounded", true);
            moveLag -= Time.deltaTime;
            freeFall = false;
            if (moveLag < 0.01)
            {
                moveLag = -1;
            }
        }
        else if (moveLag <= 0)
        {
            myAnimator.SetBool("grounded", false); // TODO: Patrick Check this to see if it is correct for logic

            xAxis = hInput.GetAxis("P2_Horizontal");
            yAxis = hInput.GetAxis("P2_Vertical");

            if (grounded && !shielding)
            {
                stats.shieldHealth += Time.deltaTime;
                if (stats.shieldHealth > stats.maxShieldHealth)
                {
                    stats.shieldHealth = stats.maxShieldHealth;
                }

                myAnimator.SetBool("grounded", true);

                if (xAxis > 0)
                {
                    rSmashTimer += Time.deltaTime;
                    lSmashTimer = 0;
                    uSmashTimer = 0;
                    dSmashTimer = 0;

                    if (!facingRight)
                    {
                        Vector3 scale = gameObject.transform.localScale;
                        scale.x *= -1;
                        gameObject.transform.localScale = scale;
                        facingRight = !facingRight;
                    }

                    body.velocity = new Vector2(xAxis * stats.maxSpeed, body.velocity.y);
                    myAnimator.SetFloat("speed", xAxis * stats.maxSpeed);

                    // do sideTilt, sideSmash, or sideSpecial
                    if (hInput.GetButtonDown("P2_Attack"))
                    {
                        if (rSmashTimer <= stats.smashTime)
                        {
                            body.velocity = Vector2.zero;
                            SideSmash();
                        }
                        else
                        {
                            body.velocity = Vector2.zero;
                            SideTilt();
                        }
                    }
                    else if (hInput.GetButtonDown("P2_Special"))
                    {
                        SideSpecial();
                    }
                }
                else if (xAxis < 0)
                {
                    myAnimator.SetFloat("speed", (-1 * xAxis) * stats.maxSpeed);
                    rSmashTimer = 0;
                    lSmashTimer += Time.deltaTime;
                    uSmashTimer = 0;
                    dSmashTimer = 0;

                    if (facingRight)
                    {
                        Vector3 scale = gameObject.transform.localScale;
                        scale.x *= -1;
                        gameObject.transform.localScale = scale;
                        facingRight = !facingRight;
                    }

                    body.velocity = new Vector2(xAxis * stats.maxSpeed, body.velocity.y);

                    // do sideTilt, sideSmash, or sideSpecial
                    if (hInput.GetButtonDown("P2_Attack"))
                    {

                        if (lSmashTimer <= stats.smashTime)
                        {
                            body.velocity = Vector2.zero;
                            SideSmash();
                        }
                        else
                        {
                            body.velocity = Vector2.zero;
                            SideTilt();
                        }
                    }
                    else if (hInput.GetButtonDown("P2_Special"))
                    {
                        SideSpecial();
                    }
                }
                else
                {
                    if (yAxis < 0)
                    {
                        rSmashTimer = 0;
                        lSmashTimer = 0;
                        uSmashTimer += Time.deltaTime;
                        dSmashTimer = 0;

                        // do upTilt, upSmash, or upSpecial
                        if (hInput.GetButtonDown("P2_Attack"))
                        {
                            if (uSmashTimer <= stats.smashTime)
                            {
                                body.velocity = Vector2.zero;
                                UpSmash();
                            }
                            else
                            {
                                body.velocity = Vector2.zero;
                                UpTilt();
                            }
                        }
                        else if (hInput.GetButtonDown("P2_Special"))
                        {
                            UpSpecial();
                        }
                    }
                    else if (yAxis > 0)
                    {
                        rSmashTimer = 0;
                        lSmashTimer = 0;
                        uSmashTimer = 0;
                        dSmashTimer += Time.deltaTime;

                        // do downTilt, downSmash, or downSpecial
                        if (hInput.GetButtonDown("P2_Attack"))
                        {
                            if (dSmashTimer <= stats.smashTime)
                            {
                                body.velocity = Vector2.zero;
                                DownSmash();
                            }
                            else
                            {
                                body.velocity = Vector2.zero;
                                DownTilt();
                            }
                        }
                        else if (hInput.GetButtonDown("P2_Special"))
                        {
                            DownSpecial();
                        }
                    }
                    else
                    {
                        myAnimator.SetFloat("speed", xAxis * stats.maxSpeed);
                        rSmashTimer = 0;
                        lSmashTimer = 0;
                        uSmashTimer = 0;
                        dSmashTimer = 0;
                        // do jab
                        if (hInput.GetButtonDown("P2_Attack"))
                        {
                            Jab();
                        }
                        else if (hInput.GetButtonDown("P2_Special"))
                        {
                            Special();
                        }
                    }
                }
            }
            else if (!shielding)
            {
                stats.shieldHealth += Time.deltaTime;
                if (stats.shieldHealth > stats.maxShieldHealth)
                {
                    stats.shieldHealth = stats.maxShieldHealth;
                }

                if (xAxis > 0)
                {
                    body.velocity = new Vector2(xAxis * stats.maxAirSpeed, body.velocity.y);
                    //body.AddForce(new Vector2(xAxis, 0) * stats.maxAirSpeed);

                    // do sideAir or sideSpecial
                    if (hInput.GetButton("P2_Attack"))
                    {
                        if (facingRight)
                        {
                            ForwardAir();
                        }
                        else
                        {
                            BackAir();
                        }
                    }
                    else if (hInput.GetButtonDown("P2_Special"))
                    {
                        if (!facingRight)
                        {
                            Vector3 scale = gameObject.transform.localScale;
                            scale.x *= -1;
                            gameObject.transform.localScale = scale;
                            facingRight = !facingRight;
                        }
                        SideSpecial();
                    }
                }
                else if (xAxis < 0)
                {
                    body.velocity = new Vector2(xAxis * stats.maxAirSpeed, body.velocity.y);
                    //body.AddForce(new Vector2(xAxis, 0) * stats.maxAirSpeed);

                    // do sideAir or sideSpecial
                    if (hInput.GetButton("P2_Attack"))
                    {
                        if (!facingRight)
                        {
                            ForwardAir();
                        }
                        else
                        {
                            BackAir();
                        }
                    }
                    else if (hInput.GetButtonDown("P2_Special"))
                    {
                        if (facingRight)
                        {
                            Vector3 scale = gameObject.transform.localScale;
                            scale.x *= -1;
                            gameObject.transform.localScale = scale;
                            facingRight = !facingRight;
                        }
                        SideSpecial();
                    }
                }
                else
                {
                    if (yAxis < 0)
                    {
                        // do upAir, or upSpecial
                        if (hInput.GetButton("P2_Attack"))
                        {
                            UpAir();
                        }
                        else if (hInput.GetButtonDown("P2_Special"))
                        {
                            UpSpecial();
                        }
                    }
                    else if (yAxis > 0)
                    {
                        // do downAir, or downSpecial
                        if (hInput.GetButton("P2_Attack"))
                        {
                            DownAir();
                        }
                        else if (hInput.GetButtonDown("P2_Special"))
                        {
                            DownSpecial();
                        }
                    }
                    else
                    {
                        // do neutralAir or neutral special
                        if (hInput.GetButton("P2_Attack"))
                        {
                            NeutralAir();
                        }
                        else if (hInput.GetButtonDown("P2_Special"))
                        {
                            Special();
                        }
                    }
                }
            }

            if (hInput.GetButton("P2_Jump") && grounded)
            {
                myAnimator.SetTrigger("jump");

                shortHopTimer += Time.deltaTime;
                if (shortHopTimer > stats.shortHopTime)
                {
                    myAnimator.SetBool("grounded", false);
                    grounded = false;
                    body.velocity = new Vector2(body.velocity.x, stats.jumpForce);
                    shortHopTimer = 0;
                }
            }
            if (hInput.GetButtonUp("P2_Jump") && grounded)
            {

                grounded = false;
                myAnimator.SetBool("grounded", false);

                if (shortHopTimer < stats.shortHopTime)
                {
                    body.velocity = new Vector2(body.velocity.x, (stats.jumpForce / 2));
                }
                shortHopTimer = 0;
            }
            else if (hInput.GetButtonDown("P2_Jump") && jumpsLeft > 0 && !grounded)
            {
                myAnimator.SetTrigger("jump");
                myAnimator.SetBool("grounded", false);
                if (AirTurnArround(xAxis))
                {
                    Vector3 scale = gameObject.transform.localScale;
                    scale.x *= -1;
                    gameObject.transform.localScale = scale;
                    facingRight = !facingRight;
                }
                body.velocity = new Vector2(body.velocity.x, stats.jumpForce);
                jumpsLeft--;

            }

            if (hInput.GetButtonDown("P2_Shield") && grounded)
            {
                Shield();
                myAnimator.SetBool("grounded", true);
            }
            if (hInput.GetButtonUp("P2_Shield") && grounded)
            {
                myAnimator.SetBool("grounded", true);
                ShieldOff();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag.Equals("Blastzone"))
        {
            transform.position = p2Spawn.transform.position;
            stats.damagePercent = 0;
            stats.numberOfLives -= 1;
            body.velocity = new Vector2(0, 0);
            respawnTimer = stats.respawnTime;
            invincible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {

    }

    bool AirTurnArround(float x)
    {
        return ((x < 0 && facingRight) || (x > 0 && !facingRight));
    }

    bool PlayerInput()
    {
        return (hInput.GetAxis("P2_Horizontal") > 0
                || hInput.GetAxis("P2_Horizontal") < 0
                || hInput.GetAxis("P2_Vertical") > 0
                || hInput.GetAxis("P2_Vertical") < 0
                || hInput.GetButton("P2_Attack")
                || hInput.GetButton("P2_Special")
                || hInput.GetButton("P2_Jump")
                || hInput.GetButton("P2_Shield"));
    }


    //--------------------------------//
    //            Atacks              //
    //--------------------------------//

    // TODO: implement shield condition to the attacks

    void UpTilt()
    {
        myAnimator.SetTrigger("Dup");
        myAnimator.SetTrigger("basicAttack");

        Debug.Log("upTilt");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[1].bounds.center, stats.hitBoxes[1].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
            {
                if (!c.gameObject.GetComponent<CharacterStats>().isShielding)
                {
                    knockbackMult = CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.uTiltLaunch.x * gameObject.transform.localScale.x, stats.uTiltLaunch.y) * knockbackMult;
                    c.gameObject.GetComponent<CharacterStats>().stunTime = stats.uTiltStun;
                    c.gameObject.GetComponent<CharacterStats>().damagePercent += stats.uTiltDamage;
                    Debug.Log(c.name);
                }
                else
                {
                    c.gameObject.GetComponent<CharacterStats>().shieldHealth -= stats.uTiltDamage;
                }
            }
        }
        moveLag = stats.uTiltLag;
    }

    void DownTilt()
    {
        myAnimator.SetTrigger("basicAttack");
        myAnimator.SetTrigger("Ddown");

        Debug.Log("downTilt");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[2].bounds.center, stats.hitBoxes[2].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
            {
                if (!c.gameObject.GetComponent<CharacterStats>().isShielding)
                {
                    knockbackMult = CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.dTiltLaunch.x * gameObject.transform.localScale.x, stats.dTiltLaunch.y) * knockbackMult;
                    c.gameObject.GetComponent<CharacterStats>().stunTime = stats.dTiltStun;
                    c.gameObject.GetComponent<CharacterStats>().damagePercent += stats.dTiltDamage;
                    Debug.Log(c.name);
                }
                else
                {
                    c.gameObject.GetComponent<CharacterStats>().shieldHealth -= stats.dTiltDamage;
                }
            }
        }
        moveLag = stats.dTiltLag;
    }

    void SideTilt()
    {
        myAnimator.SetTrigger("basicAttack");
        myAnimator.SetTrigger("Dfor");
        Debug.Log("sideTilt");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[0].bounds.center, stats.hitBoxes[0].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
            {
                if (!c.gameObject.GetComponent<CharacterStats>().isShielding)
                {
                    knockbackMult = CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.sTiltLaunch.x * gameObject.transform.localScale.x, stats.sTiltLaunch.y) * knockbackMult;
                    c.gameObject.GetComponent<CharacterStats>().stunTime = stats.sTiltStun;
                    c.gameObject.GetComponent<CharacterStats>().damagePercent += stats.sTiltDamage;
                    Debug.Log(c.name);
                }
                else
                {
                    c.gameObject.GetComponent<CharacterStats>().shieldHealth -= stats.sTiltDamage;
                }
            }
        }
        moveLag = stats.sTiltLag;
    }

    void Special()
    {
        myAnimator.SetTrigger("nSpecial");

        if (facingRight)
        {
            fireball = Instantiate(projectile, rangedStartPoint.position, transform.rotation);
            fireball.GetComponent<fireball>().launchVector = stats.nSpecialLaunch;
        }
        else
        {
            fireball = Instantiate(projectile, rangedStartPoint.position, transform.rotation);
            fireball.transform.localScale *= -1;
            fireball.GetComponent<fireball>().launchVector = new Vector2(stats.nSpecialLaunch.x * -1, stats.nSpecialLaunch.y);
        }
        fireball.GetComponent<fireball>().enemyTag = "Player1";
        fireball.GetComponent<fireball>().damage = stats.nSpecialDamage;
        fireball.GetComponent<fireball>().fbStun = stats.nSpecialStun;
        moveLag = stats.nSpecialLag;
    }

    void UpSpecial()
    {
        myAnimator.SetTrigger("specialAttack");
        myAnimator.SetTrigger("Dup");
        body.velocity = new Vector2(0, (stats.upSpecialForce));
        moveLag = stats.uSpecialLag;
        freeFall = true;
    }

    void DownSpecial()
    {
        myAnimator.SetTrigger("specialAttack");
        myAnimator.SetTrigger("Ddown");
        Special();                                    // TODO: do an actual down special if we have the animations
        Debug.Log("downSpecial");
    }

    void SideSpecial()
    {
        myAnimator.SetTrigger("specialAttack");
        myAnimator.SetTrigger("Dfor");
        sideSecial = true;
        moveLag = stats.sSpecialLag;
    }

    void UpSmash()
    {
        myAnimator.SetTrigger("basicAttack");
        myAnimator.SetTrigger("Dup");
        myAnimator.SetTrigger("smash");
        Debug.Log("upSmash");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[4].bounds.center, stats.hitBoxes[4].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
            {
                if (!c.gameObject.GetComponent<CharacterStats>().isShielding)
                {
                    knockbackMult = CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.uSmashLaunch.x * gameObject.transform.localScale.x, stats.uSmashLaunch.y) * knockbackMult;
                    c.gameObject.GetComponent<CharacterStats>().stunTime = stats.uSmashStun;
                    c.gameObject.GetComponent<CharacterStats>().damagePercent += stats.uSmashDamage;
                    Debug.Log(c.name);
                }
                else
                {
                    c.gameObject.GetComponent<CharacterStats>().shieldHealth -= stats.uSmashDamage;
                }
            }
        }
        moveLag = stats.uSmashLag;
    }

    void DownSmash()
    {
        myAnimator.SetTrigger("basicAttack");
        myAnimator.SetTrigger("Ddown");
        myAnimator.SetTrigger("smash");
        Debug.Log("downSmash");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[5].bounds.center, stats.hitBoxes[5].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
            {
                if (!c.gameObject.GetComponent<CharacterStats>().isShielding)
                {
                    knockbackMult = CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.dSmashLaunch.x * gameObject.transform.localScale.x, stats.dSmashLaunch.y) * knockbackMult;
                    c.gameObject.GetComponent<CharacterStats>().stunTime = stats.dSmashStun;
                    c.gameObject.GetComponent<CharacterStats>().damagePercent += stats.dSmashDamage;
                    Debug.Log(c.name);
                }
                else
                {
                    c.gameObject.GetComponent<CharacterStats>().shieldHealth -= stats.dSmashDamage;
                }
            }
        }
        moveLag = stats.dSmashLag;
    }

    void SideSmash()
    {
        myAnimator.SetTrigger("basicAttack");
        myAnimator.SetTrigger("Dfor");
        myAnimator.SetTrigger("smash");
        Debug.Log("sideSmash");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[3].bounds.center, stats.hitBoxes[3].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
            {
                if (!c.gameObject.GetComponent<CharacterStats>().isShielding)
                {
                    knockbackMult = CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.sSmashLaunch.x * gameObject.transform.localScale.x, stats.sSmashLaunch.y) * knockbackMult;
                    c.gameObject.GetComponent<CharacterStats>().stunTime = stats.sSmashStun;
                    c.gameObject.GetComponent<CharacterStats>().damagePercent += stats.sSmashDamage;
                    Debug.Log(c.name);
                }
                else
                {
                    c.gameObject.GetComponent<CharacterStats>().shieldHealth -= stats.sSmashDamage;
                }
            }
        }
        moveLag = stats.sSmashLag;
    }

    void UpAir()
    {
        myAnimator.SetTrigger("basicAttack");
        Debug.Log("upAir");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[10].bounds.center, stats.hitBoxes[10].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
            {
                if (!c.gameObject.GetComponent<CharacterStats>().isShielding)
                {
                    knockbackMult = CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.uAirLaunch.x * gameObject.transform.localScale.x, stats.uAirLaunch.y) * knockbackMult;
                    c.gameObject.GetComponent<CharacterStats>().stunTime = stats.uAirStun;
                    c.gameObject.GetComponent<CharacterStats>().damagePercent += stats.uAirDamage;
                    Debug.Log(c.name);
                }
                else
                {
                    c.gameObject.GetComponent<CharacterStats>().shieldHealth -= stats.uAirDamage;
                }
            }
        }
        moveLag = stats.uAirLag;
    }

    void DownAir()
    {
        myAnimator.SetTrigger("basicAttack");
        Debug.Log("downAir");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[9].bounds.center, stats.hitBoxes[9].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
            {
                if (!c.gameObject.GetComponent<CharacterStats>().isShielding)
                {
                    knockbackMult = CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.dAirLaunch.x * gameObject.transform.localScale.x, stats.dAirLaunch.y) * knockbackMult;
                    c.gameObject.GetComponent<CharacterStats>().stunTime = stats.dAirStun;
                    c.gameObject.GetComponent<CharacterStats>().damagePercent += stats.dAirDamage;
                    Debug.Log(c.name);
                }
                else
                {
                    c.gameObject.GetComponent<CharacterStats>().shieldHealth -= stats.dAirDamage;
                }
            }
        }
        moveLag = stats.dAirLag;
    }

    void ForwardAir()
    {
        myAnimator.SetTrigger("basicAttack");
        Debug.Log("forwardAir");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[7].bounds.center, stats.hitBoxes[7].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
            {
                if (!c.gameObject.GetComponent<CharacterStats>().isShielding)
                {
                    knockbackMult = CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.fAirLaunch.x * gameObject.transform.localScale.x, stats.fAirLaunch.y) * knockbackMult;
                    c.gameObject.GetComponent<CharacterStats>().stunTime = stats.fAirStun;
                    c.gameObject.GetComponent<CharacterStats>().damagePercent += stats.fAirDamage;
                    Debug.Log(c.name);
                }
                else
                {
                    c.gameObject.GetComponent<CharacterStats>().shieldHealth -= stats.fAirDamage;
                }
            }
        }
        moveLag = stats.fAirLag;
    }

    void BackAir()
    {
        myAnimator.SetTrigger("basicAttack");
        Debug.Log("backAir");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[8].bounds.center, stats.hitBoxes[8].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
            {
                if (!c.gameObject.GetComponent<CharacterStats>().isShielding)
                {
                    knockbackMult = CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.bAirLaunch.x * gameObject.transform.localScale.x * -1, stats.bAirLaunch.y) * knockbackMult;
                    c.gameObject.GetComponent<CharacterStats>().stunTime = stats.bAirStun;
                    c.gameObject.GetComponent<CharacterStats>().damagePercent += stats.bAirDamage;
                    Debug.Log(c.name);
                }
                else
                {
                    c.gameObject.GetComponent<CharacterStats>().shieldHealth -= stats.bAirDamage;
                }
            }
        }
        moveLag = stats.bAirLag;
    }

    void NeutralAir()
    {
        myAnimator.SetTrigger("basicAttack");
        Debug.Log("neutralAir");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[11].bounds.center, stats.hitBoxes[11].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
            {
                if (!c.gameObject.GetComponent<CharacterStats>().isShielding)
                {
                    knockbackMult = CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.nAirLaunch.x * gameObject.transform.localScale.x, stats.nAirLaunch.y) * knockbackMult;
                    c.gameObject.GetComponent<CharacterStats>().stunTime = stats.nAirStun;
                    c.gameObject.GetComponent<CharacterStats>().damagePercent += stats.nAirDamage;
                    Debug.Log(c.name);
                }
                else
                {
                    c.gameObject.GetComponent<CharacterStats>().shieldHealth -= stats.nAirDamage;
                }
            }
        }
        moveLag = stats.nAirLag;
    }

    void Jab()
    {
        myAnimator.SetTrigger("basicAttack");
        Debug.Log("jab");
        Collider2D[] cols = Physics2D.OverlapBoxAll(stats.hitBoxes[6].bounds.center, stats.hitBoxes[6].bounds.extents, 0.0f, LayerMask.GetMask("Player1"));
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.tag.Equals("Player1"))
            {
                if (!c.gameObject.GetComponent<CharacterStats>().isShielding)
                {
                    knockbackMult = CalculateKnockbackMult(c);
                    c.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.jabLaunch.x * gameObject.transform.localScale.x, stats.jabLaunch.y) * knockbackMult;
                    c.gameObject.GetComponent<CharacterStats>().stunTime = stats.jabStun;
                    c.gameObject.GetComponent<CharacterStats>().damagePercent += stats.jabDamage;
                    Debug.Log(c.name);
                }
                else
                {
                    c.gameObject.GetComponent<CharacterStats>().shieldHealth -= stats.jabDamage;
                }
            }
        }
        moveLag = stats.jabLag;
    }

    void Shield()
    {
        p2Shield.SetActive(true);
        shielding = true;
        stats.isShielding = true;
    }

    void ShieldOff()
    {
        p2Shield.SetActive(false);
        shielding = false;
        stats.isShielding = false;
    }

    float CalculateKnockbackMult(Collider2D c)
    {
        return (c.gameObject.GetComponent<CharacterStats>().damagePercent < stats.knockbackPercentFloor) ? (stats.knockbackPercentFloor / 100) : ((float)c.gameObject.GetComponent<CharacterStats>().damagePercent / 100);
    }
}
