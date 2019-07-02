using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class P1_CharController : MonoBehaviour {

    private CharacterStats stats;
    private CharAttacks attacks;
    public int testChar;

    public GameObject p1Spawn;
    public GameObject p1Shield;
    public GameObject projectile1;
    public GameObject projectile2;
    public Transform rangedStartPoint;
    public Transform rangedStartPoint2;
    private Animator myAnimator;
    private Transform frozenPos;

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
    private float dropThresh = 0.5f;
    private float zero = 0.01f;
    private float gravity;

    // writing to file
    
    int numJumps = 0;
    int numAirAttacks = 0;
    int numGroundAttacks = 0;
    float timeInAir = 0.0f;
    float timeOfMatch = 0.0f;
    int peakDmg = 0;
    int dmgGiven = 0;
    int dmgTaken = 0;
    float meleeAccuracy = 0.0f;
    float rangedAccuracy = 0.0f;

    private Vector2 shieldScale;
    //StreamWriter sw;

    //end of filewriting 
    void Start () {
        //sw = new StreamWriter("stats.txt", true);

        p1Spawn = GameObject.FindGameObjectWithTag("P1_Spawn");
        p1Shield = GameObject.FindGameObjectWithTag("P1_Shield");
        p1Shield.SetActive(false);
        gameObject.transform.position = p1Spawn.transform.position;
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

        shieldScale = p1Shield.transform.localScale;

        // set volume of attacks
        playerAudio.volume *= MatchPref.sfxVol;

        // Get the right Moveset for the character
        int ch = (MatchPref.player1 == 0) ? testChar : MatchPref.player1;
        switch (ch)
        {
            case 1:
                attacks = gameObject.GetComponent<P1_FB_Attacks>();
                //sw.WriteLine("playing Firebrand");
                break;
            case 2:
                attacks = gameObject.GetComponent<P1_Link_Attacks>();
                //sw.WriteLine("playing Link");
                break;
            case 3:
                attacks = gameObject.GetComponent<P1_Mario_Attacks>();
                //sw.WriteLine("playing Mario");
                break;
            case 4:
                attacks = gameObject.GetComponent<P1_Samus_Attacks>();
                //sw.WriteLine("playing Samus");
                break;
        }
        
        //Setting up the output to a file
    
            
            
        


        

	}

    // Update Manager
	void Update ()
    {
        // Resize shield acording to its current health
        p1Shield.transform.localScale = shieldScale * ((float)stats.shieldHealth / 100.0f);

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

        xAxis = hInput.GetAxis("P1_Horizontal");
        yAxis = hInput.GetAxis("P1_Vertical");

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
        }
	}

    void AirAttackUpdate()
    {
        timeInAir += Time.deltaTime;
        timeInAttack += Time.deltaTime;

        if (xAxis > 0)
        {
            //body.velocity = new Vector2(xAxis * stats.maxAirSpeed, body.velocity.y);

            body.AddForce(new Vector2(xAxis, 0) * stats.maxAirSpeed * stats.airSpeedAccel);
            Vector2 velocity = body.velocity;

            if (velocity.x > stats.maxAirSpeed)
            {
                velocity.x = stats.maxAirSpeed;
                body.velocity = velocity;
            }
        }
        else if (xAxis < 0)
        {
            //body.velocity = new Vector2(xAxis * stats.maxAirSpeed, body.velocity.y);

            body.AddForce(new Vector2(xAxis, 0) * stats.maxAirSpeed * stats.airSpeedAccel);
            Vector2 velocity = body.velocity;

            if (velocity.x < -stats.maxAirSpeed)
            {
                velocity.x = -stats.maxAirSpeed;
                body.velocity = velocity;
            }
        }

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
                    //Debug.Log("really got here");
                    attacks.Special();
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
            upSpecial = false;
            downSpecial = false;
            sideSpecial = false;
            neutralSpecial = false;

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
                numGroundAttacks++; // increment ground attacks
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
                    //Debug.Log("really got here");
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

    void GroundedUpdate()
    {
        // Condition to switch to airborne state
        if (!grounded)
        {
            stats.charState = CharacterStats.state.airborne;
            return;
        }
        
        // Manage going through platforms
        if (gameObject.layer == 14)
        {
            foreach (Transform t in gameObject.GetComponentsInChildren<Transform>(true))
            {
                t.gameObject.layer = 10;
            }
            gameObject.layer = 10;
        }
        
        // Shield and roll management
        if (hInput.GetButton("P1_Shield"))
        {
            Shield();
            myAnimator.SetBool("grounded", true);

            // Once the stick is returned to the standard position your soll is reset
            if (xAxis < deadZone && xAxis > -deadZone)
            {
                rolled = false;
            }
            // The !rolled makes sure you can only roll once if you dont reset the position of the analog stick or release shield
            else if (xAxis != 0 && !rolled)
            {
                myAnimator.SetTrigger("dodge");
                rolled = true;
                body.velocity = new Vector2(Mathf.Sign(xAxis) * stats.dodgeSpeed, 0);
                stats.charState = CharacterStats.state.lag;
                moveLag = stats.rollLag;
                ivTimer = stats.rollInv;
                ShieldOff();
                // Turn arround to the oposite direction to which you are moving
                if (!AirTurnArround(xAxis))
                {
                    TurnArround();
                }
                return;
            }
        }
        if (hInput.GetButtonUp("P1_Shield"))
        {
            myAnimator.SetBool("grounded", true);
            rolled = false;
            ShieldOff();
        }
        // End

        // Manage movement and attack triggers
        if (!shielding)
        {
            // Regen shield health over time when not being used
            stats.shieldHealth += Time.deltaTime;
            if (stats.shieldHealth > stats.maxShieldHealth)
            {
                stats.shieldHealth = stats.maxShieldHealth;
            }

            myAnimator.SetBool("grounded", true);

            if (Mathf.Abs(xAxis) > Mathf.Abs(yAxis))
            {
                if (xAxis > 0)
                {
                    rSmashTimer += Time.deltaTime;
                    lSmashTimer = 0;
                    uSmashTimer = 0;
                    dSmashTimer = 0;

                    // Change the orientation of the character of looking the wrong way
                    if (!facingRight)
                    {
                        TurnArround();
                    }

                    body.velocity = new Vector2(xAxis * stats.maxSpeed, body.velocity.y);
                    myAnimator.SetTrigger("walk");
                    playerAudio.clip = audio_walk;
                    playerAudio.Play();
                    myAnimator.SetFloat("speed", Mathf.Abs(xAxis) * stats.maxSpeed);

                    // do sideTilt, sideSmash, or sideSpecial
                    if (hInput.GetButtonDown("P1_Attack"))
                    {
                        stats.charState = CharacterStats.state.attacking;
                        // Checks if the attack button was pressed fast enough to perform a Smash attack
                        if (rSmashTimer <= stats.smashTime)
                        {
                            body.velocity = Vector2.zero;
                            preAttackTimer = stats.sSmashStartup;
                            activeAttackTimer = stats.sSmashActive;
                            myAnimator.SetTrigger("fSmash");
                            playerAudio.clip = audio_fSmash;
                            playerAudio.Play();
                            sideSmash = true;
                            return;
                        }
                        else
                        {
                            body.velocity = Vector2.zero;
                            preAttackTimer = stats.sTiltStartup;
                            activeAttackTimer = stats.sTiltActive;
                            myAnimator.SetTrigger("fTilt");
                            playerAudio.clip = audio_fTilt;
                            playerAudio.Play();
                            sideTilt = true;
                            return;
                        }
                    }
                    else if (hInput.GetButtonDown("P1_Special"))
                    {
                        stats.charState = CharacterStats.state.attacking;
                        preAttackTimer = stats.sSpecialStartup;
                        activeAttackTimer = stats.sSpecialActive;
                        myAnimator.SetTrigger("fSpecial");
                        playerAudio.clip = audio_fSpecial;
                        playerAudio.Play();
                        sideSpecial = true;
                        return;
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
                        TurnArround();
                    }

                    body.velocity = new Vector2(xAxis * stats.maxSpeed, body.velocity.y);
                    myAnimator.SetFloat("speed", Mathf.Abs(xAxis) * stats.maxSpeed);
                    playerAudio.clip = audio_walk;
                    playerAudio.Play();

                    // do sideTilt, sideSmash, or sideSpecial
                    if (hInput.GetButtonDown("P1_Attack"))
                    {
                        stats.charState = CharacterStats.state.attacking;

                        if (lSmashTimer <= stats.smashTime)
                        {
                            body.velocity = Vector2.zero;
                            preAttackTimer = stats.sSmashStartup;
                            activeAttackTimer = stats.sSmashActive;
                            myAnimator.SetTrigger("fSmash");
                            playerAudio.clip = audio_fSmash;
                            playerAudio.Play();
                            sideSmash = true;
                            return;
                        }
                        else
                        {
                            body.velocity = Vector2.zero;
                            preAttackTimer = stats.sTiltStartup;
                            activeAttackTimer = stats.sTiltActive;
                            myAnimator.SetTrigger("fTilt");
                            playerAudio.clip = audio_fTilt;
                            playerAudio.Play();
                            sideTilt = true;
                            return;
                        }
                    }
                    else if (hInput.GetButtonDown("P1_Special"))
                    {
                        stats.charState = CharacterStats.state.attacking;
                        preAttackTimer = stats.sSpecialStartup;
                        activeAttackTimer = stats.sSpecialActive;
                        myAnimator.SetTrigger("fSpecial");
                        playerAudio.clip = audio_fSpecial;
                        playerAudio.Play();
                        sideSpecial = true;
                        return;
                    }
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
                    if (hInput.GetButtonDown("P1_Attack"))
                    {
                        stats.charState = CharacterStats.state.attacking;

                        if (uSmashTimer <= stats.smashTime)
                        {
                            body.velocity = Vector2.zero;
                            preAttackTimer = stats.uSmashStartup;
                            activeAttackTimer = stats.uSmashActive;
                            myAnimator.SetTrigger("upSmash");
                            playerAudio.clip = audio_upSmash;
                            playerAudio.Play();
                            upSmash = true;
                            return;
                        }
                        else
                        {
                            body.velocity = Vector2.zero;
                            preAttackTimer = stats.uTiltStartup;
                            activeAttackTimer = stats.uTiltActive;
                            myAnimator.SetTrigger("upTilt");
                            playerAudio.clip = audio_upTilt;
                            playerAudio.Play();
                            upTilt = true;
                            return;
                        }
                    }
                    else if (hInput.GetButtonDown("P1_Special"))
                    {
                        stats.charState = CharacterStats.state.airAttacking;
                        preAttackTimer = stats.uSpecialStartup;
                        activeAttackTimer = stats.uSpecialActive;
                        myAnimator.SetTrigger("upSpecial");
                        playerAudio.clip = audio_upSpecial;
                        playerAudio.Play();
                        upSpecial = true;
                        return;
                    }
                }
                else if (yAxis > 0)
                {
                    rSmashTimer = 0;
                    lSmashTimer = 0;
                    uSmashTimer = 0;
                    dSmashTimer += Time.deltaTime;

                    // do downTilt, downSmash, or downSpecial
                    if (hInput.GetButtonDown("P1_Attack"))
                    {
                        stats.charState = CharacterStats.state.attacking;
                        if (dSmashTimer <= stats.smashTime)
                        {
                            body.velocity = Vector2.zero;
                            preAttackTimer = stats.dSmashStartup;
                            activeAttackTimer = stats.dSmashActive;
                            myAnimator.SetTrigger("dSmash");
                            playerAudio.clip = audio_dSmash;
                            playerAudio.Play();
                            downSmash = true;
                            return;
                        }
                        else
                        {
                            body.velocity = Vector2.zero;
                            preAttackTimer = stats.dTiltStartup;
                            activeAttackTimer = stats.dTiltActive;
                            myAnimator.SetTrigger("dTilt");
                            playerAudio.clip = audio_dTilt;
                            playerAudio.Play();
                            downTilt = true;
                            return;
                        }
                    }
                    else if (hInput.GetButtonDown("P1_Special"))
                    {
                        stats.charState = CharacterStats.state.attacking;
                        preAttackTimer = stats.dSpecialStartup;
                        activeAttackTimer = stats.dSpecialActive;
                        myAnimator.SetTrigger("nSpecial");
                        playerAudio.clip = audio_nSpecial;
                        playerAudio.Play();
                        downSpecial = true;
                        return;
                    }
                }
                else
                {
                    myAnimator.SetFloat("speed", 0);
                    rSmashTimer = 0;
                    lSmashTimer = 0;
                    uSmashTimer = 0;
                    dSmashTimer = 0;
                    // do jab or neutral special
                    if (hInput.GetButtonDown("P1_Attack"))
                    {
                        //Debug.Log("jab");
                        stats.charState = CharacterStats.state.attacking;
                        preAttackTimer = stats.jabStartup;
                        activeAttackTimer = stats.jabActive;
                        myAnimator.SetTrigger("jab");
                        playerAudio.clip = audio_jab;
                        playerAudio.Play();
                        jab = true;
                        return; // force state change 
                    }
                    else if (hInput.GetButtonDown("P1_Special"))
                    {
                        stats.charState = CharacterStats.state.attacking;
                        preAttackTimer = stats.nSpecialStartup;
                        activeAttackTimer = stats.nSpecialActive;
                        myAnimator.SetTrigger("nSpecial");
                        playerAudio.clip = audio_nSpecial;
                        playerAudio.Play();
                        neutralSpecial = true;
                        return;
                    }
                }
            }
        }

        // Manage Full and Short jump
        if (hInput.GetButton("P1_Jump"))
        {
            myAnimator.SetTrigger("jump");
            numJumps++;
            shortHopTimer += Time.deltaTime;
            if (shortHopTimer > stats.shortHopTime)
            {
                playerAudio.clip = audio_jump;
                playerAudio.Play();
                myAnimator.SetBool("grounded", false);
                ShieldOff();
                grounded = false;
                stats.charState = CharacterStats.state.airborne;
                body.velocity = new Vector2(body.velocity.x, stats.jumpForce);
                shortHopTimer = 0;
            }
        }
        if (hInput.GetButtonUp("P1_Jump"))
        {

            grounded = false;
            myAnimator.SetBool("grounded", false);
            ShieldOff();
            stats.charState = CharacterStats.state.airborne;

            if (shortHopTimer < stats.shortHopTime)
            {
                playerAudio.clip = audio_jump;
                playerAudio.Play();
                body.velocity = new Vector2(body.velocity.x, (stats.jumpForce * ((float) 2/3)));
            }
            shortHopTimer = 0;
        }
        // End
    }

    void AirborneUpdate()
    {
        timeInAir += Time.deltaTime;
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
        else if (gameObject.layer == 14)
        {
            foreach (Transform t in gameObject.GetComponentsInChildren<Transform>(true))
            {
                t.gameObject.layer = 10;
            }
            gameObject.layer = 10;
        }
        // End

        // Regen shield health over time when not being used
        stats.shieldHealth += Time.deltaTime;
        if (stats.shieldHealth > stats.maxShieldHealth)
        {
            stats.shieldHealth = stats.maxShieldHealth;
        }

        // Airdodge active time
        if (airdodgeTimer > 0)
        {
            airdodgeTimer -= Time.deltaTime;

            if (airdodgeTimer < zero)
            {
                airdodgeTimer = 0;
                stats.charState = CharacterStats.state.lag;
                airdodgeLag = stats.airDodgeLag;
            }
            return;
        }

        // Manage Airdodges 
        if (hInput.GetButtonDown("P1_Shield"))
        {
            myAnimator.SetTrigger("airDodge");
            if (xAxis < deadZone && xAxis > -deadZone) { xAxis = 0; }
            if (yAxis < deadZone && yAxis > -deadZone) { yAxis = 0; }

            if (xAxis != 0 && yAxis != 0)
            {
                float magnitude = new Vector2(xAxis, yAxis).magnitude;
                xAxis = xAxis / magnitude;
                yAxis = (yAxis / magnitude) * -1;
                if (xAxis < deadZone && xAxis > -deadZone) { xAxis = 0; }
                if (yAxis < deadZone && yAxis > -deadZone) { yAxis = 0; }
                body.velocity = new Vector2(xAxis, yAxis) * stats.airdodgeSpeed;
                airdodgeTimer += stats.airDodgeInv;
                ivTimer += stats.airDodgeInv;
            }
            else if (xAxis != 0 || yAxis != 0)
            {
                body.velocity = new Vector2(xAxis, yAxis * -1) * stats.airdodgeSpeed;
                airdodgeTimer += stats.airDodgeInv;
                ivTimer += stats.airDodgeInv;
            }
            else
            {
                airdodgeTimer += stats.airDodgeInv;
                ivTimer += stats.airDodgeInv;
            }
        }
        // Manage air movement and attacks triggers
        else if (Mathf.Abs(xAxis) > Mathf.Abs(yAxis))
        {
            if (xAxis > 0)
            {
                //body.velocity = new Vector2(xAxis * stats.maxAirSpeed, body.velocity.y);

                body.AddForce(new Vector2(xAxis, 0) * stats.maxAirSpeed * stats.airSpeedAccel);
                Vector2 velocity = body.velocity;

                if (velocity.x > stats.maxAirSpeed)
                {
                    velocity.x = stats.maxAirSpeed;
                    body.velocity = velocity;
                }

                // do sideAir or sideSpecial
                if (hInput.GetButtonDown("P1_Attack"))
                {
                    numAirAttacks++; // increment air attacks
                    stats.charState = CharacterStats.state.airAttacking;
                    if (facingRight)
                    {
                        preAttackTimer = stats.fAirStartup;
                        activeAttackTimer = stats.fAirActive;
                        myAnimator.SetTrigger("fAir");
                        playerAudio.clip = audio_fAir;
                        playerAudio.Play();
                        forwardAir = true;
                        return;
                    }
                    else
                    {
                        preAttackTimer = stats.bAirStartup;
                        activeAttackTimer = stats.bAirActive;
                        myAnimator.SetTrigger("bAir");
                        playerAudio.clip = audio_bAir;
                        playerAudio.Play();
                        backAir = true;
                        return;
                    }
                }
                else if (hInput.GetButtonDown("P1_Special"))
                {
                    stats.charState = CharacterStats.state.airAttacking;
                    if (!facingRight)
                    {
                        TurnArround();
                    }
                    preAttackTimer = stats.sSpecialStartup;
                    activeAttackTimer = stats.sSpecialActive;
                    myAnimator.SetTrigger("fSpecial");
                    playerAudio.clip = audio_fSpecial;
                    playerAudio.Play();
                    sideSpecial = true;
                    return;
                }
            }
            else if (xAxis < 0)
            {
                //body.velocity = new Vector2(xAxis * stats.maxAirSpeed, body.velocity.y);

                body.AddForce(new Vector2(xAxis, 0) * stats.maxAirSpeed * stats.airSpeedAccel);
                Vector2 velocity = body.velocity;

                if (velocity.x < -stats.maxAirSpeed)
                {
                    velocity.x = -stats.maxAirSpeed;
                    body.velocity = velocity;
                }

                // do sideAir or sideSpecial
                if (hInput.GetButtonDown("P1_Attack"))
                {
                    numAirAttacks++; // increment air attacks
                    stats.charState = CharacterStats.state.airAttacking;
                    if (!facingRight)
                    {
                        preAttackTimer = stats.fAirStartup;
                        activeAttackTimer = stats.fAirActive;
                        myAnimator.SetTrigger("fAir");
                        playerAudio.clip = audio_fAir;
                        playerAudio.Play();
                        forwardAir = true;
                        return;
                    }
                    else
                    {
                        preAttackTimer = stats.bAirStartup;
                        activeAttackTimer = stats.bAirActive;
                        myAnimator.SetTrigger("bAir");
                        playerAudio.clip = audio_bAir;
                        playerAudio.Play();
                        backAir = true;
                        return;
                    }
                }
                else if (hInput.GetButtonDown("P1_Special"))
                {
                    stats.charState = CharacterStats.state.airAttacking;
                    if (facingRight)
                    {
                        TurnArround();
                    }
                    preAttackTimer = stats.sSpecialStartup;
                    activeAttackTimer = stats.sSpecialActive;
                    myAnimator.SetTrigger("fSpecial");
                    playerAudio.clip = audio_fSpecial;
                    playerAudio.Play();
                    sideSpecial = true;
                    return;
                }
            }
        }
        else
        {
            if (yAxis < 0)
            {
                numAirAttacks++; // increment air attacks
                // do upAir, or upSpecial
                if (hInput.GetButtonDown("P1_Attack"))
                {
                    stats.charState = CharacterStats.state.airAttacking;
                    preAttackTimer = stats.uAirStartup;
                    activeAttackTimer = stats.uAirActive;
                    myAnimator.SetTrigger("upAir");
                    playerAudio.clip = audio_upAir;
                    playerAudio.Play();
                    upAir = true;
                    return;
                }
                else if (hInput.GetButtonDown("P1_Special"))
                {
                    stats.charState = CharacterStats.state.airAttacking;
                    preAttackTimer = stats.uSpecialStartup;
                    activeAttackTimer = stats.uSpecialActive;
                    myAnimator.SetTrigger("upSpecial");
                    playerAudio.clip = audio_upSpecial;
                    playerAudio.Play();
                    upSpecial = true;
                    return;
                }
            }
            else if (yAxis > 0)
            {
                // do downAir, or downSpecial
                if (hInput.GetButtonDown("P1_Attack"))
                {
                    numAirAttacks++; // increment air attacks
                    stats.charState = CharacterStats.state.airAttacking;
                    preAttackTimer = stats.dAirStartup;
                    activeAttackTimer = stats.dAirActive;
                    myAnimator.SetTrigger("dAir");
                    playerAudio.clip = audio_dAir;
                    playerAudio.Play();
                    downAir = true;
                    return;
                }
                else if (hInput.GetButtonDown("P1_Special"))
                {
                    stats.charState = CharacterStats.state.airAttacking;
                    preAttackTimer = stats.dSpecialStartup;
                    activeAttackTimer = stats.dSpecialActive;
                    myAnimator.SetTrigger("nSpecial");
                    playerAudio.clip = audio_nSpecial;
                    playerAudio.Play();
                    downSpecial = true;
                    return;
                }
            }
            else
            {
                // do neutralAir or neutral special
                if (hInput.GetButtonDown("P1_Attack"))
                {
                    numAirAttacks++; // increment air attacks
                    stats.charState = CharacterStats.state.airAttacking;
                    preAttackTimer = stats.nAirStartup;
                    activeAttackTimer = stats.nAirActive;
                    myAnimator.SetTrigger("nAir");
                    playerAudio.clip = audio_nAir;
                    playerAudio.Play();
                    neutralAir = true;
                    return;
                }
                else if (hInput.GetButtonDown("P1_Special"))
                {
                    stats.charState = CharacterStats.state.airAttacking;
                    preAttackTimer = stats.nSpecialStartup;
                    activeAttackTimer = stats.nSpecialActive;
                    myAnimator.SetTrigger("nSpecial");
                    playerAudio.clip = audio_nSpecial;
                    playerAudio.Play();
                    neutralSpecial = true;
                    return;
                }
            }
        }
        // End

        // Manage extra jumps
        if (hInput.GetButtonDown("P1_Jump") && jumpsLeft > 0)
        {
            myAnimator.SetTrigger("jump");
            numJumps++ ;
            playerAudio.clip = audio_jump;
            playerAudio.Play();
            myAnimator.SetBool("grounded", false);
            if (AirTurnArround(xAxis))
            {
                TurnArround();
            }
            body.velocity = new Vector2(body.velocity.x, stats.jumpForce);
            jumpsLeft--;

        }
    }

    void LockedUpdate()
    {
        myAnimator.SetBool("grounded", true);
        myAnimator.SetFloat("speed", 0);
        if (respawnTimer > 0)
        {
            myAnimator.SetTrigger("StunEnd");
            body.velocity = Vector2.zero;
            sideSpecial = false;
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            respawnTimer -= Time.deltaTime;

            if ((respawnTimer < 3 && PlayerInput()) || respawnTimer < zero)
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

            xAxis = hInput.GetAxis("P1_Horizontal");

            body.velocity = new Vector2(xAxis * stats.maxAirSpeed, body.velocity.y);
            myAnimator.SetTrigger("walk");
            playerAudio.clip = audio_walk;
            playerAudio.Play();
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

    // Handle player respawning
    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag.Equals("Blastzone"))
        {
            frozenPos = gameObject.transform;
            timeToRespawn = stats.timeBeforeRespawn;
            body.gravityScale = 0;
            body.velocity = new Vector2(0, 0);
            invincible = true;
            stats.isInvincible = true;
            //Invoke("respawn", timeToRespawn);
            stats.charState = CharacterStats.state.limbo;
        }
    }
    public void respawn()
    {
        
        if(stats.damagePercent > peakDmg)
        {
            peakDmg = stats.damagePercent;
        }


        transform.position = p1Spawn.transform.position;
        stats.damagePercent = 0;
        stats.numberOfLives -= 1;
        body.velocity = new Vector2(0, 0);
        respawnTimer = stats.respawnTime;
        body.gravityScale = gravity;
        stats.charState = CharacterStats.state.locked;
        //StreamWriter sw = new StreamWriter("stats.txt", true);
       
    }
    // End

    // Handle dropping from platforms
    private void OnCollisionStay2D(Collision2D c)
    {
        if (c.gameObject.layer == 16)
        {
            if (hInput.GetAxis("P1_Vertical") > dropThresh)
            {
                foreach(Transform t in gameObject.GetComponentsInChildren<Transform>(true))
                {
                    t.gameObject.layer = 14;
                }
                gameObject.layer = 14;
                grounded = false;
                myAnimator.SetBool("grounded", false);
                myAnimator.SetTrigger("jump");
                numJumps++;
                playerAudio.clip = audio_jump;
                playerAudio.Play();
                layerTimer = timeBetweenLayers;
                stats.charState = CharacterStats.state.airborne;
            }
        }
    }

    // Checks if the player model needs to turn arround
    bool AirTurnArround(float x)
    {
        return ((x < 0 && facingRight) || (x > 0 && !facingRight));
    }

    // Checks if the player is doing any input
    bool PlayerInput()
    {
        return (hInput.GetAxis("P1_Horizontal") > 0
                || hInput.GetAxis("P1_Horizontal") < 0
                || hInput.GetAxis("P1_Vertical") > 0
                || hInput.GetAxis("P1_Vertical") < 0
                || hInput.GetButton("P1_Attack")
                || hInput.GetButton("P1_Special")
                || hInput.GetButton("P1_Jump")
                || hInput.GetButton("P1_Shield"));
    }
    
    // check to see if the character is performing a special move
    bool IsSpecial()
    {
        return (upSpecial || sideSpecial || neutralSpecial);
    }

    // Activate Shield
    void Shield()
    {
        p1Shield.SetActive(true);
        shielding = true;
        stats.isShielding = true;
    }

    // Deactivate shield
    void ShieldOff()
    {
        p1Shield.SetActive(false);
        shielding = false;
        stats.isShielding = false;
    }

    public float CalculateKnockbackMult(Collider2D c)
    {
        return (c.gameObject.GetComponent<CharacterStats>().damagePercent < stats.knockbackPercentFloor) ? (stats.knockbackPercentFloor / 100) : ((float)c.gameObject.GetComponent<CharacterStats>().damagePercent / 100);
    }

    // Rotates player character to face the oposite direction
    void TurnArround()
    {
        Vector3 scale = gameObject.transform.localScale;
        scale.x *= -1;
        gameObject.transform.localScale = scale;
        facingRight = !facingRight;
    }

    
    //public void printToFile()
    //{
    //    sw.WriteLine("number of jumps taken: " + numJumps + Environment.NewLine + "Number of air attacks made: " + numAirAttacks + Environment.NewLine + "Number of ground attacks made: " + numGroundAttacks + Environment.NewLine + "Amount of time airborne: " + timeInAir + Environment.NewLine + "Match total time: " + timeOfMatch + Environment.NewLine + "Peak Damage Taken: " + peakDmg + Environment.NewLine, true);
    //    sw.Close();
    //}

}
