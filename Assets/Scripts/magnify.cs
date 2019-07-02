using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//IMPORTANT
//PLACE THIS SCRIPT ONTO THE CAMERA

public class magnify : MonoBehaviour
{

    public Transform Player;
    public Camera Cam;
    public float size = .3f;
    public float xOffset = 0f;
    public float yOffset = .2f;
    public Renderer render;

    // Use this for initialization
    void Start()
    {
    }

    public void setCameraPos()
    {
        float x = Player.position.x + xOffset;
        float y = Player.position.y + yOffset;

        Cam.transform.position = new Vector3(x, y, Cam.transform.position.z);
    }

    void setCameraSize()
    {
        Cam.orthographicSize = size;
    }

    // Update is called once per frame
    void Update()
    {
        setCameraPos();
        setCameraSize();
    }
}