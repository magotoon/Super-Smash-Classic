using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

    public int numberOfExtraJumps = 1;
    public int numberOfLives = 3;
    public int damagePercent = 0;
    public float maxShieldHealth = 100;
    public float shieldHealth = 100;
    public float shieldBreakTime = 6;
    public float jumpForce = 4.0f;
    public float respawnTime = 4;
    public float timeBeforeRespawn = 1;
    public float startTime = 3;
    public float shortHopTime = 0.08f;
    public float smashTime = 0.1f;
    public float maxSpeed = 2;
    public float maxAirSpeed = 1;
    public float airSpeedAccel = 4;
    public float ivFramesRespawnTime = 1;
    public float ivFramesLedgeTime = 1.5f;
    public float upSpecialForce = 3;
    public float sideSpecialForce = 7;
    public float stunTime = 0;
    public Collider2D[] hitBoxes;
    public float knockbackPercentFloor = 20;
    public bool isShielding = false;
    public bool isInvincible = false;

    // state vars
    public enum state
    {
        grounded,
        attacking,
        airborne,
        locked,
        lag,
        stun,
        limbo,
        airAttacking,
        charging
    }
    public state charState { get; set; }
    

    // dodging vars
    public float airDodgeLag = 2;
    public float airDodgeLagGround = 0.25f;
    public float airDodgeInv = 0.25f;
    public float rollLag = 0.75f;
    public float rollInv = 0.25f;
    public float dodgeSpeed = 3.5f;
    public float airdodgeSpeed = 7f;

    // stuntimes for moves
    public float uTiltStun = 0.5f;
    public float dTiltStun = 0.5f;
    public float sTiltStun = 0.5f;
    public float uSmashStun = 0.5f;
    public float dSmashStun = 0.5f;
    public float sSmashStun = 0.5f;
    public float uAirStun = 0.5f;
    public float fAirStun = 0.5f;
    public float bAirStun = 0.5f;
    public float dAirStun = 0.5f;
    public float nAirStun = 0.5f;
    public float jabStun = 0.5f;
    public float uSpecialStun = 0.5f;
    public float dSpecialStun = 0.5f;
    public float sSpecialStun = 0.5f;
    public float nSpecialStun = 0.5f;

   
    // damage for moves
    public int uTiltDamage = 1;
    public int dTiltDamage = 1;
    public int sTiltDamage = 1;
    public int uSmashDamage = 1;
    public int dSmashDamage = 1;
    public int sSmashDamage = 1;
    public int uAirDamage = 1;
    public int fAirDamage = 1;
    public int bAirDamage = 1;
    public int dAirDamage = 1;
    public int nAirDamage = 1;
    public int jabDamage = 1;
    public int uSpecialDamage = 1;
    public int dSpecialDamage = 1;
    public int sSpecialDamage = 1;
    public int nSpecialDamage = 1;


    // launch vectors per move
    public Vector2 uTiltLaunch = new Vector2(0,0);
    public Vector2 dTiltLaunch = new Vector2(0, 0);
    public Vector2 sTiltLaunch = new Vector2(0, 0);
    public Vector2 uSmashLaunch = new Vector2(0, 0);
    public Vector2 dSmashLaunch = new Vector2(0, 0);
    public Vector2 sSmashLaunch = new Vector2(0, 0);
    public Vector2 uAirLaunch = new Vector2(0, 0);
    public Vector2 fAirLaunch = new Vector2(0, 0);
    public Vector2 bAirLaunch = new Vector2(0, 0);
    public Vector2 dAirLaunch = new Vector2(0, 0);
    public Vector2 nAirLaunch = new Vector2(0, 0);
    public Vector2 jabLaunch = new Vector2(0, 0);
    public Vector2 uSpecialLaunch = new Vector2(0, 0);
    public Vector2 dSpecialLaunch = new Vector2(0, 0);
    public Vector2 sSpecialLaunch = new Vector2(0, 0);
    public Vector2 nSpecialLaunch = new Vector2(0, 0);


    // move lag vars
    public float uTiltLag = 1;
    public float dTiltLag = 1;
    public float sTiltLag = 1f;
    public float uSmashLag = 2;
    public float dSmashLag = 2;
    public float sSmashLag = 2f;
    public float uAirLag = 1;
    public float dAirLag = 1;
    public float fAirLag = 1;
    public float bAirLag = 1;
    public float nAirLag = 1;
    public float jabLag = 0.5f;
    public float uSpecialLag = 1;
    public float dSpecialLag = 1;
    public float sSpecialLag = 1;
    public float nSpecialLag = 0.25f;

    // startup time for moves
    public float uTiltStartup = 0;
    public float dTiltStartup = 0;
    public float sTiltStartup = 0;
    public float uSmashStartup = 0;
    public float dSmashStartup = 0;
    public float sSmashStartup = 0;
    public float uAirStartup = 0;
    public float dAirStartup = 0;
    public float fAirStartup = 0;
    public float bAirStartup = 0;
    public float nAirStartup = 0;
    public float jabStartup = 0;
    public float uSpecialStartup = 0;
    public float dSpecialStartup = 0;
    public float sSpecialStartup = 0;
    public float nSpecialStartup = 0;

    // active time per move
    public float uTiltActive = 0;
    public float dTiltActive = 0;
    public float sTiltActive = 0;
    public float uSmashActive = 0;
    public float dSmashActive = 0;
    public float sSmashActive = 0;
    public float uAirActive = 0;
    public float dAirActive = 0;
    public float fAirActive = 0;
    public float bAirActive = 0;
    public float nAirActive = 0;
    public float jabActive = 0.5f;
    public float uSpecialActive = 0;
    public float dSpecialActive = 0;
    public float sSpecialActive = 0;
    public float nSpecialActive = 0;
}
