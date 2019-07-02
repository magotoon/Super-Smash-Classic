using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mrTest : MonoBehaviour {

    public GameObject p2Spawn;
    public GameObject p2Shield;

    public bool invincible { get; set; }

    private bool grounded = false;
    private bool facingRight = true;
    private bool shielding = false;
    private float ivTimer = 0;
    private float respawnTimer = 0;
    private float startTimer;
    private float smashTimer = 0f;
    private float shortHopTimer = 0f;
    private float curSpeed;
    private float xAxis;
    private float yAxis;
    private int jumpsLeft;
    private Rigidbody2D body;
    private CharacterStats stats;


    void Start()
    {
        p2Spawn = GameObject.FindGameObjectWithTag("P2_Spawn");
        //p1Shield = GameObject.FindGameObjectWithTag("P1_Shield");
        //p1Shield.SetActive(false);
        gameObject.transform.position = p2Spawn.transform.position;
        body = gameObject.GetComponent<Rigidbody2D>();
        stats = gameObject.GetComponent<CharacterStats>();
        jumpsLeft = stats.numberOfExtraJumps;
        startTimer = stats.startTime;
        invincible = false;


        Physics2D.IgnoreLayerCollision(10,11);
    }
	
	// Update is called once per frame
	void Update () {
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
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            respawnTimer -= Time.deltaTime;

            if (/*(respawnTimer < 3 && PlayerInput()) ||*/ respawnTimer < 0.01)
            {
                gameObject.GetComponent<Rigidbody2D>().simulated = true;
                respawnTimer = 0;
                ivTimer = stats.ivFramesRespawnTime;
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
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag.Equals("Blastzone"))
        {
            transform.position = p2Spawn.transform.position;
            body.velocity = new Vector2(0, 0);
            respawnTimer = stats.respawnTime;
            invincible = true;
        }
    }
}
