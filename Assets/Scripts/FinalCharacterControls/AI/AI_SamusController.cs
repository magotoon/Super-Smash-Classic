using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SamusController : MonoBehaviour {

    public GameObject Enemy;
    private CharacterStats stats;
    private CharAttacks attacks;
    public int testChar;

    public GameObject p2Spawn;
    public GameObject p2Shield;
    public GameObject projectile1;
    public GameObject projectile2;
    public Transform rangedStartPoint;
    public Transform rangedStartPoint2;
    private Animator myAnimator;
    private Transform frozenPos;

    private System.Random rand;

    public bool invincible { get; set; }

    // Movement
    public bool grounded { get; set; }
    public int jumpsLeft { get; set; }
    public bool facingRight { get; set; }
    private bool shielding = false;
    public bool freeFall { get; set; }
    private bool airdodging = false;
    private bool rolled = false;
    // End

    // AI orientation and triggers
    public int direction { get; set; }
    public float heightDiff { get; set; }
    public float xdiff { get; set; }
    public float followRange = 0.3f;
    public float shootingRange = 1f;
    public float chargeRange = 1.4f;
    public float innerShootingRange = 0.6f;
    public float emergencyShootRange = .2f;
    public float jumpThreshold = 0.5f;
    public float dropthreshold = -0.3f;
    public float aiHeight = 0.4f;
    public float timeBetweenAttacks = .5f;
    private float attackTimer = 0;
    public float retXDiff { get; set; }
    public float retHeightDiff { get; set; }

    private bool UpOfStage = false;
    private bool sidebOfStage = false;
    private bool recovering = false;
    public GameObject returnPoint;
    // End

    // Attacks
    private bool upTilt = false;
    private bool downTilt = false;
    private bool sideTilt = false;
    private bool upSmash = false;
    private bool downSmash = false;
    private bool sideSmash = false;
    private bool upAir = false;
    private bool downAir = false;
    private bool forwardAir = false;
    private bool backAir = false;
    private bool neutralAir = false;
    private bool jab = false;
    private bool upSpecial = false;
    private bool downSpecial = false;
    private bool sideSpecial = false;
    private bool neutralSpecial = false;

    public bool charging = false;
    public bool finishedCharging = false;
    public float fullCharge = 2.0f;
    public float chargePercent { get; set; }

    public bool multiHit { get; set; }
    public bool hit { get; set; }
    public float timeInAttack { get; set; }
    // End

    // Timers
    private float ivTimer = 0;
    private float respawnTimer = 0;
    private float timeToRespawn = 0;
    private float startTimer;
    private float airdodgeTimer = 0;
    private float rSmashTimer = 0;
    private float lSmashTimer = 0;
    private float uSmashTimer = 0;
    private float dSmashTimer = 0;
    private float shortHopTimer = 0;
    private float airdodgeLag = 0;
    public float moveLag { get; set; }
    private float stunTimer = 0;
    private float preAttackTimer = 0;
    public float activeAttackTimer { get; set; }
    public float timeBetweenLayers = 0.3f;
    private float layerTimer = 0;
    // End

    /* Audio controls*/
    public AudioClip audio_idle;
    public AudioClip audio_upAir;
    public AudioClip audio_dAir;
    public AudioClip audio_fAir;
    public AudioClip audio_bAir;
    public AudioClip audio_nAir;
    public AudioClip audio_upTilt;
    public AudioClip audio_dTilt;
    public AudioClip audio_fTilt;
    public AudioClip audio_bTilt;
    public AudioClip audio_upSmash;
    public AudioClip audio_dSmash;
    public AudioClip audio_fSmash;
    public AudioClip audio_bSmash;
    public AudioClip audio_upSpecial;
    public AudioClip audio_nSpecial;
    public AudioClip audio_fSpecial;
    public AudioClip audio_bSpecial;
    public AudioClip audio_jab;
    public AudioClip audio_charging;
    public AudioClip audio_walk;
    public AudioClip audio_jump;
    public AudioSource playerAudio;
    // end of audio controls

    private float xAxis;
    private float yAxis;
    private Rigidbody2D body;
    private float knockbackMult = 0.1f;
    private float deadZone = 0.3f;
    private float zero = 0.01f;
    private float gravity;
    private Vector2 shieldScale;

    void Start()
    {
        Enemy = GameObject.FindGameObjectWithTag("Player1");
        returnPoint = GameObject.FindGameObjectWithTag("returnPoint");
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
        stats.charState = CharacterStats.state.locked;
        hit = false;
        multiHit = false;
        freeFall = false;
        facingRight = true;
        gravity = body.gravityScale;
        timeInAttack = 0;
        rand = new System.Random();

        shieldScale = p2Shield.transform.localScale;

        // set volume of attacks
        playerAudio.volume *= MatchPref.sfxVol;

        // set difficoulty for the ai
        switch (MatchPref.cpuDif)
        {
            case 0:
                timeBetweenAttacks = 0.5f;
                break;
            case 1:
                timeBetweenAttacks = 0.3f;
                break;
            case 2:
                timeBetweenAttacks = 0.2f;
                break;
            case 3:
                timeBetweenAttacks = 0f;
                break;
            default:
                break;
        }

        // Get the right Moveset for the character
        int ch = (MatchPref.player2 == 0) ? testChar : MatchPref.player2;
        switch (ch)
        {
            case 1:
                attacks = gameObject.GetComponent<AI_FB_Attacks>();
                break;
            case 2:
                attacks = gameObject.GetComponent<AI_Link_Attacks>();
                break;
            case 3:
                attacks = gameObject.GetComponent<AI_Mario_Attacks>();
                break;
            case 4:
                attacks = gameObject.GetComponent<AI_Samus_Attacks>();
                break;
        }

    }

    // State Manager
    void Update()
    {
        // Resize shield acording to its current health
        p2Shield.transform.localScale = shieldScale * ((float)stats.shieldHealth / 100.0f);

        // Calculate the difference in the x-component of the players position
        xdiff = Enemy.transform.position.x - gameObject.transform.position.x;

        // Calculate tentative x movement direction
        direction = (int)Mathf.Sign(xdiff);

        // Calculate height difference between the characters
        heightDiff = Enemy.transform.position.y - gameObject.transform.position.y;

        // Calculate height difference between the returnpoint and AI
        retHeightDiff = returnPoint.transform.position.y - gameObject.transform.position.y;

        // Calculate the difference in the x-component of the AI position vs the return point
        retXDiff = returnPoint.transform.position.x - gameObject.transform.position.x;

        // Manage invinsibility frames
        if (ivTimer > 0)
        {
            ivTimer -= Time.deltaTime;

            if (ivTimer < zero)
            {
                invincible = false;
                stats.isInvincible = false;
                ivTimer = 0;
            }
        }
        //else
        //{
        //    invincible = false;
        //    stats.isInvincible = false;
        //    ivTimer = 0;
        //}

        // Check for damage cap
        if (stats.damagePercent > 999)
        {
            stats.damagePercent = 999;
        }

        // Check shield health
        if (stats.shieldHealth <= 0)
        {
            stats.charState = CharacterStats.state.stun;
            myAnimator.SetTrigger("hurt");
            stats.stunTime = stats.shieldBreakTime;
            stats.shieldHealth = stats.maxShieldHealth;
            ShieldOff();
        }

        // Go to the apropiate update method depending on state
        switch (stats.charState)
        {
            case CharacterStats.state.grounded:
                GroundedUpdate();
                break;
            case CharacterStats.state.attacking:
                AttackUpdate();
                break;
            case CharacterStats.state.airborne:
                AirborneUpdate();
                break;
            case CharacterStats.state.locked:
                LockedUpdate();
                break;
            case CharacterStats.state.lag:
                LagUpdate();
                break;
            case CharacterStats.state.stun:
                StunUpdate();
                break;
            case CharacterStats.state.limbo:
                LimboUpdate();
                break;
            case CharacterStats.state.airAttacking:
                AirAttackUpdate();
                break;
            case CharacterStats.state.charging:
                ChargeUpdate();
                break;
        }
    }

    void AirAttackUpdate()
    {
        timeInAttack += Time.deltaTime;

        if (preAttackTimer > 0)
        {
            preAttackTimer -= Time.deltaTime;

            if (preAttackTimer < zero)
            {
                preAttackTimer = 0;
            }
        }
        else if ((activeAttackTimer > 0 && !grounded) || (activeAttackTimer > 0 && IsSpecial()))
        {
            activeAttackTimer -= Time.deltaTime;

            if (multiHit || !hit)
            {
                if (upAir)
                {
                    attacks.UpAir();
                }
                else if (downAir)
                {
                    attacks.DownAir();
                }
                else if (forwardAir)
                {
                    attacks.ForwardAir();
                }
                else if (backAir)
                {
                    attacks.BackAir();
                }
                else if (neutralAir)
                {
                    attacks.NeutralAir();
                }
            }
        }
        else
        {
            upAir = false;
            downAir = false;
            forwardAir = false;
            backAir = false;
            neutralAir = false;

            multiHit = false;
            hit = false;
            timeInAttack = 0;
            activeAttackTimer = 0;

            stats.charState = CharacterStats.state.lag;
        }
    }

    void AttackUpdate()
    {
        timeInAttack += Time.deltaTime;

        if (preAttackTimer > 0)
        {
            preAttackTimer -= Time.deltaTime;

            if (preAttackTimer < zero)
            {
                preAttackTimer = 0;
            }
        }
        else if (activeAttackTimer > 0)
        {
            activeAttackTimer -= Time.deltaTime;

            if (multiHit || !hit)
            {
                if (upTilt)
                {
                    attacks.UpTilt();
                }
                else if (downTilt)
                {
                    attacks.DownTilt();
                }
                else if (sideTilt)
                {
                    attacks.SideTilt();
                }
                else if (upSmash)
                {
                    attacks.UpSmash();
                }
                else if (downSmash)
                {
                    attacks.DownSmash();
                }
                else if (sideSmash)
                {
                    attacks.SideSmash();
                }
                else if (jab)
                {
                    attacks.Jab();
                }
                else if (upSpecial)
                {
                    attacks.UpSpecial();
                }
                else if (downSpecial)
                {
                    attacks.DownSpecial();
                }
                else if (sideSpecial)
                {
                    attacks.SideSpecial();
                }
                else if (neutralSpecial)
                {
                    attacks.Special();
                }
            }
        }
        else
        {
            upTilt = false;
            downTilt = false;
            sideTilt = false;
            upSmash = false;
            downSmash = false;
            sideSmash = false;
            jab = false;
            upSpecial = false;
            downSpecial = false;
            sideSpecial = false;
            neutralSpecial = false;

            multiHit = false;
            hit = false;
            timeInAttack = 0;

            stats.charState = CharacterStats.state.lag;
        }
    }

    void LockedUpdate()
    {
        myAnimator.SetTrigger("StunEnd");
        stunTimer = 0;
        myAnimator.SetBool("grounded", true);
        myAnimator.SetFloat("speed", 0);

        if (respawnTimer > 0)
        {
            body.velocity = Vector2.zero;
            sideSpecial = false;
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            respawnTimer -= Time.deltaTime;

            if (respawnTimer < zero)
            {
                gameObject.GetComponent<Rigidbody2D>().simulated = true;
                respawnTimer = 0;
                ivTimer = stats.ivFramesRespawnTime;
                rSmashTimer = 0;
                lSmashTimer = 0;
                uSmashTimer = 0;
                dSmashTimer = 0;
            }
        }
        else if (startTimer > 0)
        {
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            startTimer -= Time.deltaTime;

            if (startTimer < zero)
            {
                gameObject.GetComponent<Rigidbody2D>().simulated = true;
                startTimer = 0;
                rSmashTimer = 0;
                lSmashTimer = 0;
                uSmashTimer = 0;
                dSmashTimer = 0;
            }
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().simulated = true;

            if (grounded)
            {
                stats.charState = CharacterStats.state.grounded;
            }
            else
            {
                stats.charState = CharacterStats.state.airborne;
            }
        }
    }

    void GroundedUpdate()
    {
        // Condition to switch to airborne state
        if (!grounded)
        {
            stats.charState = CharacterStats.state.airborne;
            return;
        }
        myAnimator.SetBool("grounded", true);
        UpOfStage = false;

        // Manage going through platforms
        if (gameObject.layer == 15)
        {
            foreach (Transform t in gameObject.GetComponentsInChildren<Transform>(true))
            {
                t.gameObject.layer = 11;
            }
            gameObject.layer = 11;
        }

        // Change facing direction depending on where the player is
        if (direction < 0)
        {
            if (facingRight)
            {
                TurnArround();
            }
        }
        else if (direction >= 0)
        {
            if (!facingRight)
            {
                TurnArround();
            }
        }
        // End

        // Manage Jump
        if (heightDiff > jumpThreshold)
        {
            if (rand.Next(1, 6) < 3)
            {
                int jumpheight = rand.Next(1, 3);
                myAnimator.SetTrigger("jump");
                myAnimator.SetBool("grounded", false);
                ShieldOff();
                grounded = false;
                playerAudio.clip = audio_jump;
                playerAudio.Play();
                stats.charState = CharacterStats.state.airborne;
                switch (jumpheight)
                {
                    case 1:
                        body.velocity = new Vector2(body.velocity.x, stats.jumpForce);
                        break;
                    case 2:
                        body.velocity = new Vector2(body.velocity.x, (stats.jumpForce * ((float)2 / 3)));
                        break;
                }
            }
        }

        // Manage Attack delay
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            // Manage Projectile attacks
            if (Mathf.Abs(xdiff) < shootingRange && Mathf.Abs(xdiff) > innerShootingRange)
            {
                int option = rand.Next(1, 11);
                if (option < 5)
                {
                    if (option < 4)
                    {
                        stats.charState = CharacterStats.state.attacking;
                        preAttackTimer = stats.nSpecialStartup;
                        activeAttackTimer = stats.nSpecialActive;
                        myAnimator.SetTrigger("nSpecial");
                        myAnimator.SetTrigger("finishedCharging");
                        playerAudio.clip = audio_nSpecial;
                        playerAudio.Play();
                        neutralSpecial = true;
                    }
                    else if (option == 4)
                    {
                        //side special
                        stats.charState = CharacterStats.state.attacking;
                        preAttackTimer = stats.sSpecialStartup;
                        activeAttackTimer = stats.sSpecialActive;
                        myAnimator.SetTrigger("fSpecial");
                        playerAudio.clip = audio_fSpecial;
                        playerAudio.Play();
                        sideSpecial = true;
                    }
                }
                attackTimer = timeBetweenAttacks;
                return;
            }

            // Manage Melee attacks
            if (Mathf.Abs(xdiff) < followRange)
            {
                if (rand.Next(1, 11) < 5)
                {
                    if (heightDiff > aiHeight)
                    {
                        if (rand.Next(1, 3) == 1)
                        {
                            // uptilt
                            stats.charState = CharacterStats.state.attacking;
                            body.velocity = Vector2.zero;
                            preAttackTimer = stats.uTiltStartup;
                            activeAttackTimer = stats.uTiltActive;
                            myAnimator.SetTrigger("upTilt");
                            playerAudio.clip = audio_upTilt;
                            playerAudio.Play();
                            upTilt = true;
                        }
                        else
                        {
                            // upsmash
                            stats.charState = CharacterStats.state.attacking;
                            body.velocity = Vector2.zero;
                            preAttackTimer = stats.uSmashStartup;
                            activeAttackTimer = stats.uSmashActive;
                            myAnimator.SetTrigger("upSmash");
                            playerAudio.clip = audio_upSmash;
                            playerAudio.Play();
                            upSmash = true;
                        }
                    }
                    else
                    {
                        int attack = rand.Next(1, 6);
                        switch (attack)
                        {
                            case 1:
                                // sideTilt
                                stats.charState = CharacterStats.state.attacking;
                                body.velocity = Vector2.zero;
                                preAttackTimer = stats.sTiltStartup;
                                activeAttackTimer = stats.sTiltActive;
                                myAnimator.SetTrigger("fTilt");
                                playerAudio.clip = audio_fTilt;
                                playerAudio.Play();
                                sideTilt = true;
                                break;
                            case 2:
                                // sidesmash
                                stats.charState = CharacterStats.state.attacking;
                                body.velocity = Vector2.zero;
                                preAttackTimer = stats.sSmashStartup;
                                activeAttackTimer = stats.sSmashActive;
                                myAnimator.SetTrigger("fSmash");
                                playerAudio.clip = audio_fSmash;
                                playerAudio.Play();
                                sideSmash = true;
                                break;
                            case 3:
                                // downtilt
                                stats.charState = CharacterStats.state.attacking;
                                body.velocity = Vector2.zero;
                                preAttackTimer = stats.dTiltStartup;
                                activeAttackTimer = stats.dTiltActive;
                                myAnimator.SetTrigger("dTilt");
                                playerAudio.clip = audio_dTilt;
                                playerAudio.Play();
                                downTilt = true;
                                break;
                            case 4:
                                // downsmash
                                stats.charState = CharacterStats.state.attacking;
                                body.velocity = Vector2.zero;
                                preAttackTimer = stats.dSmashStartup;
                                activeAttackTimer = stats.dSmashActive;
                                myAnimator.SetTrigger("dSmash");
                                playerAudio.clip = audio_dSmash;
                                playerAudio.Play();
                                downSmash = true;
                                break;
                            case 5:
                                // jab
                                stats.charState = CharacterStats.state.attacking;
                                //Debug.Log("jab");
                                preAttackTimer = stats.jabStartup;
                                activeAttackTimer = stats.jabActive;
                                myAnimator.SetTrigger("jab");
                                playerAudio.clip = audio_jab;
                                playerAudio.Play();
                                jab = true;
                                break;
                        }
                    }
                }
                attackTimer = timeBetweenAttacks;
                return;
            }
            //Debug.Log("How did I get here!?!?!?!");
        }

        // Follow the player
        if (Mathf.Abs(xdiff) > followRange)
        {
            body.velocity = new Vector2(direction * stats.maxSpeed * (1.0f / 2.0f), body.velocity.y);
            myAnimator.SetFloat("speed", Mathf.Abs(direction) * stats.maxSpeed);
        }
    }

    void AirborneUpdate()
    {
        // Condition to switch to grounded state
        if (grounded)
        {
            stats.charState = CharacterStats.state.grounded;
            return;
        }

        // Manage going through platforms
        if (layerTimer > 0)
        {
            layerTimer -= Time.deltaTime;
        }
        else if (gameObject.layer == 15)
        {
            foreach (Transform t in gameObject.GetComponentsInChildren<Transform>(true))
            {
                t.gameObject.layer = 11;
            }
            gameObject.layer = 11;
        }
        // End

        // Manage Jumps
        if (heightDiff > jumpThreshold && jumpsLeft > 0)
        {
            if (rand.Next(1, 6) < 3)
            {
                // Change facing direction depending on where the player is
                if (direction < 0)
                {
                    if (facingRight)
                    {
                        TurnArround();
                    }
                }
                else if (direction >= 0)
                {
                    if (!facingRight)
                    {
                        TurnArround();
                    }
                }
                // End

                myAnimator.SetTrigger("jump");
                myAnimator.SetBool("grounded", false);
                ShieldOff();
                grounded = false;
                playerAudio.clip = audio_jump;
                playerAudio.Play();
                stats.charState = CharacterStats.state.airborne;
                body.velocity = new Vector2(body.velocity.x, stats.jumpForce);
                jumpsLeft--;
            }
        }

        // Manage Attack delay
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            // Manage projectile Attacks
            if (Mathf.Abs(xdiff) < shootingRange && Mathf.Abs(xdiff) > innerShootingRange)
            {
                int option = rand.Next(1, 11);

                if ((facingRight && (direction > 0)) || (!facingRight && (direction < 0)))
                {
                    if (option < 5)
                    {
                        if (option < 4)
                        {
                            stats.charState = CharacterStats.state.attacking;
                            preAttackTimer = stats.nSpecialStartup;
                            activeAttackTimer = stats.nSpecialActive;
                            neutralSpecial = true;
                            myAnimator.SetTrigger("nSpecial");
                            myAnimator.SetTrigger("finishedCharging");
                            playerAudio.clip = audio_nSpecial;
                            playerAudio.Play();
                            return;
                        }
                        else if (option == 4)
                        {
                            //side special
                            stats.charState = CharacterStats.state.attacking;
                            preAttackTimer = stats.sSpecialStartup;
                            activeAttackTimer = stats.sSpecialActive;
                            myAnimator.SetTrigger("fSpecial");
                            playerAudio.clip = audio_fSpecial;
                            playerAudio.Play();
                            sideSpecial = true;
                        }
                    }
                }
                else
                {
                    if (option < 3)
                    {
                        // Change facing direction depending on where the player is
                        if (direction < 0)
                        {
                            if (facingRight)
                            {
                                TurnArround();
                            }
                        }
                        else if (direction >= 0)
                        {
                            if (!facingRight)
                            {
                                TurnArround();
                            }
                        }
                        // End

                        //side special
                        stats.charState = CharacterStats.state.attacking;
                        preAttackTimer = stats.sSpecialStartup;
                        activeAttackTimer = stats.sSpecialActive;
                        myAnimator.SetTrigger("fSpecial");
                        playerAudio.clip = audio_fSpecial;
                        playerAudio.Play();
                        sideSpecial = true;
                    }
                }
                attackTimer = timeBetweenAttacks;
                return;
            }

            // Manage melee Attacks
            if (Mathf.Abs(xdiff) < followRange)
            {
                if (rand.Next(1, 11) < 5)
                {
                    stats.charState = CharacterStats.state.airAttacking;

                    if (heightDiff > aiHeight)
                    {
                        // upair
                        preAttackTimer = stats.uAirStartup;
                        activeAttackTimer = stats.uAirActive;
                        myAnimator.SetTrigger("upAir");
                        playerAudio.clip = audio_upAir;
                        playerAudio.Play();
                        upAir = true;
                    }
                    else if (heightDiff < -aiHeight)
                    {
                        // downair
                        preAttackTimer = stats.dAirStartup;
                        activeAttackTimer = stats.dAirActive;
                        myAnimator.SetTrigger("dAir");
                        playerAudio.clip = audio_dAir;
                        playerAudio.Play();
                        downAir = true;
                    }
                    else
                    {
                        int attack = rand.Next(1, 3);
                        switch (attack)
                        {
                            case 1:
                                // neutralair
                                preAttackTimer = stats.nAirStartup;
                                activeAttackTimer = stats.nAirActive;
                                myAnimator.SetTrigger("nAir");
                                playerAudio.clip = audio_nAir;
                                playerAudio.Play();
                                neutralAir = true;
                                break;
                            case 2:
                                if ((facingRight && (direction > 0)) || (!facingRight && (direction < 0)))
                                {
                                    // forwardair
                                    preAttackTimer = stats.fAirStartup;
                                    activeAttackTimer = stats.fAirActive;
                                    myAnimator.SetTrigger("fAir");
                                    playerAudio.clip = audio_fAir;
                                    playerAudio.Play();
                                    forwardAir = true;
                                }
                                else
                                {
                                    // backair
                                    preAttackTimer = stats.bAirStartup;
                                    activeAttackTimer = stats.bAirActive;
                                    myAnimator.SetTrigger("bAir");
                                    playerAudio.clip = audio_bAir;
                                    playerAudio.Play();
                                    backAir = true;
                                }
                                break;
                        }
                    }
                }
                attackTimer = timeBetweenAttacks;
                return;
            }
        }

        // Follow Player
        if (xdiff < -followRange)
        {
            body.AddForce(new Vector2(direction, 0) * stats.maxAirSpeed * stats.airSpeedAccel);
            Vector2 velocity = body.velocity;

            if (velocity.x < -stats.maxAirSpeed)
            {
                velocity.x = -stats.maxAirSpeed;
                body.velocity = velocity;
            }
        }
        else if (xdiff > followRange)
        {
            body.AddForce(new Vector2(xAxis, 0) * stats.maxAirSpeed * stats.airSpeedAccel);
            Vector2 velocity = body.velocity;

            if (velocity.x > stats.maxAirSpeed)
            {
                velocity.x = stats.maxAirSpeed;
                body.velocity = velocity;
            }
        }
        // End
    }

    void LagUpdate()
    {
        if (airdodgeLag > 0)
        {
            airdodgeLag -= Time.deltaTime;

            if (grounded && airdodgeLag > stats.airDodgeLagGround)
            {
                airdodgeLag = stats.airDodgeLagGround;
                airdodgeLag -= Time.deltaTime;
            }
            else if (airdodgeLag < zero)
            {
                airdodgeLag = 0;
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
            if (moveLag < zero)
            {
                moveLag = -1;
            }

            if (recovering)
            {
                if (Mathf.Abs(retXDiff) < 0)
                {
                    body.velocity = new Vector2(direction * stats.maxAirSpeed, body.velocity.y);
                }
                else if (Mathf.Abs(retXDiff) >= 0)
                {
                    body.velocity = new Vector2(direction * stats.maxAirSpeed, body.velocity.y);
                }
            }
            else
            {
                if (direction < 0)
                {
                    body.velocity = new Vector2(direction * stats.maxAirSpeed, body.velocity.y);
                }
                else if (direction >= 0)
                {
                    body.velocity = new Vector2(direction * stats.maxAirSpeed, body.velocity.y);
                }
            }

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
            if (moveLag < zero)
            {
                moveLag = -1;
            }
        }
        else
        {
            UpOfStage = false;
            if (grounded)
            {
                stats.charState = CharacterStats.state.grounded;
            }
            else
            {
                stats.charState = CharacterStats.state.airborne;
            }
        }
    }

    void StunUpdate()
    {
        moveLag = 0;
        freeFall = false;

        if (stats.stunTime > 0)
        {
            stats.stunTime -= Time.deltaTime;
            body.drag = 3f;
        }
        else
        {
            myAnimator.SetTrigger("StunEnd");
            body.drag = 0f;
            if (grounded)
            {
                stats.charState = CharacterStats.state.grounded;
            }
            else
            {
                stats.charState = CharacterStats.state.airborne;
            }
        }
    }

    void LimboUpdate()
    {
        body.velocity = new Vector2(0, 0);
        if (timeToRespawn > 0)
        {
            timeToRespawn -= Time.deltaTime;
        }
        else
        {
            respawn();
        }
    }

    void ChargeUpdate()
    {
        charging = true;
        chargePercent += Time.deltaTime;
        body.velocity = new Vector2(0, body.velocity.y);

        if (chargePercent >= fullCharge)
        {
            if (grounded)
            {
                stats.charState = CharacterStats.state.grounded;
            }
            else
            {
                stats.charState = CharacterStats.state.airborne;
            }
            myAnimator.SetTrigger("finishedCharging");
            playerAudio.clip = audio_idle;
            playerAudio.Play();
            return;
        }

        if (Mathf.Abs(xdiff) < emergencyShootRange)
        {
            stats.charState = CharacterStats.state.attacking;
            preAttackTimer = stats.nSpecialStartup;
            activeAttackTimer = stats.nSpecialActive;
            neutralSpecial = true;
            
            myAnimator.SetTrigger("finishedCharging");
            playerAudio.clip = audio_nSpecial;
            playerAudio.Play();
            return;
        }
    }

    // Handle player respawning
    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag.Equals("Blastzone"))
        {
            frozenPos = gameObject.transform;
            timeToRespawn = stats.timeBeforeRespawn;
            body.velocity = new Vector2(0, 0);
            invincible = true;
            stats.isInvincible = true;
            body.gravityScale = 0;
            //Invoke("respawn", timeToRespawn);
            stats.charState = CharacterStats.state.limbo;
        }
    }
    public void respawn()
    {
        respawnTimer = stats.respawnTime;
        stats.charState = CharacterStats.state.locked;
        transform.position = p2Spawn.transform.position;
        stats.damagePercent = 0;
        stats.numberOfLives -= 1;
        body.velocity = new Vector2(0, 0);
        body.gravityScale = gravity;
    }
    // End

    // Handle navigation back to stage 
    private void OnTriggerStay2D(Collider2D c)
    {
        if (Borders(c) && stats.charState != CharacterStats.state.stun && stats.charState != CharacterStats.state.limbo)
        {
            recovering = true;
            stats.charState = CharacterStats.state.lag;

            if (retHeightDiff > jumpThreshold)
            {
                if (jumpsLeft > 0)
                {
                    if (Mathf.Abs(retXDiff) < 0)
                    {
                        if (facingRight)
                        {
                            TurnArround();
                        }
                    }
                    else if (Mathf.Abs(retXDiff) >= 0)
                    {
                        if (!facingRight)
                        {
                            TurnArround();
                        }
                    }

                    body.velocity = new Vector2(body.velocity.x, stats.jumpForce);
                    jumpsLeft--;
                }
                else //if (!UpOfStage)
                {
                    UpOfStage = true;
                    stats.charState = CharacterStats.state.attacking;
                    preAttackTimer = stats.uSpecialStartup;
                    activeAttackTimer = stats.uSpecialActive;
                    myAnimator.SetTrigger("upSpecial");
                    playerAudio.clip = audio_upSpecial;
                    playerAudio.Play();
                    upSpecial = true;
                }
            }
            //else
            //{
            //    if (!sidebOfStage)
            //    {
            //        if (Mathf.Abs(retXDiff) < 0)
            //        {
            //            if (facingRight)
            //            {
            //                TurnArround();
            //            }
            //        }
            //        else if (Mathf.Abs(retXDiff) >= 0)
            //        {
            //            if (!facingRight)
            //            {
            //                TurnArround();
            //            }
            //        }

            //        //side special
            //        stats.charState = CharacterStats.state.attacking;
            //        preAttackTimer = stats.sSpecialStartup;
            //        activeAttackTimer = stats.sSpecialActive;
            //        myAnimator.SetTrigger("fSpecial");
            //        playerAudio.clip = audio_fSpecial;
            //        playerAudio.Play();
            //        sideSpecial = true;
            //        sidebOfStage = true;
            //    }
            //}
        }
    }
    private void OnTriggerExit2D(Collider2D c)
    {
        if (Borders(c))
        {
            recovering = false;
        }
    }
    // End

    // Handle dropping from platforms
    private void OnCollisionStay2D(Collision2D c)
    {
        if (c.gameObject.layer == 16)
        {
            if (heightDiff < dropthreshold)
            {
                foreach (Transform t in gameObject.GetComponentsInChildren<Transform>(true))
                {
                    t.gameObject.layer = 15;
                }
                gameObject.layer = 15;
                grounded = false;
                myAnimator.SetBool("grounded", false);
                myAnimator.SetTrigger("jump");
                layerTimer = timeBetweenLayers;
                stats.charState = CharacterStats.state.airborne;
            }
        }
    }

    bool Borders(Collider2D c)
    {
        return (c.gameObject.tag.Equals("leftBorder") || c.gameObject.tag.Equals("rightBorder"));
    }

    // check to see if the character is performing a special move
    bool IsSpecial()
    {
        return (upSpecial || sideSpecial || neutralSpecial);
    }

    // Activate Shield
    void Shield()
    {
        p2Shield.SetActive(true);
        shielding = true;
        stats.isShielding = true;
    }

    // Deactivate shield
    void ShieldOff()
    {
        p2Shield.SetActive(false);
        shielding = false;
        stats.isShielding = false;
    }

    public float CalculateKnockbackMult(Collider2D c)
    {
        return (c.gameObject.GetComponent<CharacterStats>().damagePercent < stats.knockbackPercentFloor) ? (stats.knockbackPercentFloor / 100) : ((float)c.gameObject.GetComponent<CharacterStats>().damagePercent / 100);
    }

    // Rotates character to face the oposite direction
    void TurnArround()
    {
        Vector3 scale = gameObject.transform.localScale;
        scale.x *= -1;
        gameObject.transform.localScale = scale;
        facingRight = !facingRight;
    }
}
