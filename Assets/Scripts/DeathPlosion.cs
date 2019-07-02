using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlosion : MonoBehaviour {

	public Animator animP1;
    public GameObject explotionP1;
    public AudioSource sourceP1;
    public Animator animP2;
    public GameObject explotionP2;
    public AudioSource sourceP2;
    //public AudioClip sound;
    private Camera mainCamera;
    private bool lockX = false;
    private bool lockY = false;

    public int bz;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        switch (bz)
        {
            case 0:
                explotionP1 = GameObject.FindGameObjectWithTag("explotionP1L");
                explotionP2 = GameObject.FindGameObjectWithTag("explotionP2L");
                lockX = true;
                break;
            case 1:
                explotionP1 = GameObject.FindGameObjectWithTag("explotionP1R");
                explotionP2 = GameObject.FindGameObjectWithTag("explotionP2R");
                lockX = true;
                break;
            case 2:
                explotionP1 = GameObject.FindGameObjectWithTag("explotionP1U");
                explotionP2 = GameObject.FindGameObjectWithTag("explotionP2U");
                lockY = true;
                break;
            case 3:
                explotionP1 = GameObject.FindGameObjectWithTag("explotionP1D");
                explotionP2 = GameObject.FindGameObjectWithTag("explotionP2D");
                lockY = true;
                break;
        }

        if (explotionP1 != null)
        {
            animP1 = explotionP1.GetComponent<Animator>();
            sourceP1 = explotionP1.GetComponent<AudioSource>();
        }
        if (explotionP2 != null)
        {
            animP2 = explotionP2.GetComponent<Animator>();
            sourceP2 = explotionP2.GetComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (explotionP1 != null && explotionP2 != null)
        {
            if (c.gameObject.tag.Equals("Player1"))
            {
                Vector3 p1Loc = mainCamera.WorldToViewportPoint(c.transform.position);
                if (p1Loc.x > 1)
                {
                    p1Loc.x = 1;
                }
                else if (p1Loc.x < 0)
                {
                    p1Loc.x = 0;
                }

                if (p1Loc.y > 1)
                {
                    p1Loc.y = 1;
                }
                else if (p1Loc.y < 0)
                {
                    p1Loc.y = 0;
                }

                p1Loc = new Vector3(mainCamera.scaledPixelWidth * p1Loc.x, mainCamera.scaledPixelHeight * p1Loc.y, p1Loc.z);

                if (lockX)
                {
                    explotionP1.transform.position = new Vector3(explotionP1.transform.position.x, p1Loc.y);
                }
                else if (lockY)
                {
                    explotionP1.transform.position = new Vector3(p1Loc.x, explotionP1.transform.position.y);
                }

                animP1.SetTrigger("death");
                //sourceP1.clip = sound;
                sourceP1.Play();
            }
            else if (c.gameObject.tag.Equals("Player2"))
            {
                Vector3 p2Loc = mainCamera.WorldToViewportPoint(c.transform.position);
                if (p2Loc.x > 1)
                {
                    p2Loc.x = 1;
                }
                else if (p2Loc.x < 0)
                {
                    p2Loc.x = 0;
                }

                if (p2Loc.y > 1)
                {
                    p2Loc.y = 1;
                }
                else if (p2Loc.y < 0)
                {
                    p2Loc.y = 0;
                }

                p2Loc = new Vector3(mainCamera.scaledPixelWidth * p2Loc.x, mainCamera.scaledPixelHeight * p2Loc.y, p2Loc.z);

                if (lockX)
                {
                    explotionP2.transform.position = new Vector3(explotionP2.transform.position.x, p2Loc.y);
                }
                else if (lockY)
                {
                    explotionP2.transform.position = new Vector3(p2Loc.x, explotionP2.transform.position.y);
                }

                animP2.SetTrigger("death");
                //sourceP2.clip = sound;
                sourceP2.Play();
            }
        }
    }
}
