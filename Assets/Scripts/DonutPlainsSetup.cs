using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutPlainsSetup : MonoBehaviour {

    private GameObject setup1;
    private GameObject setup2;

    // Use this for initialization
    void Start () {
        setup1 = GameObject.FindWithTag("DPS1");
        setup2 = GameObject.FindWithTag("DPS2");

        setup1.SetActive(false);
        setup2.SetActive(false);

        int rand = Random.Range(1, 3);
        //Debug.Log("rand = " + rand);
        if (rand == 1)
        {
            setup1.SetActive(true);
            setup2.SetActive(false);
        }
        else if (rand == 2)
        {
            setup1.SetActive(false);
            setup2.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
