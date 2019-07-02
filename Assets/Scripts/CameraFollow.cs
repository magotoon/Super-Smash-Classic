using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform Player1, Player2;
    public float minSizeY = 1.5f;
    public float maxSizeY = 3f;
    public float maxPosX = 2.5f;
    public float minPosX = -2.5f;
    public float maxPosY = 2.5f;
    public float minPosY = -2.5f;


    // Use this for initialization
    void Start()
    {
        Player1 = GameObject.FindGameObjectWithTag("Player1").transform;
        Player2 = GameObject.FindGameObjectWithTag("Player2").transform;
    }

    public void setCameraPos()
    {
        //Vector3 midpoint = (Player1.position + Player2.position) * 0.5f;
        float midpointX = (Player1.position.x + Player2.position.x) * 0.5f;
        float midpointY = (Player1.position.y + Player2.position.y) * 0.5f;
        //Debug.Log("midX: "+midpointX+"\nmidY: "+midpointY);

        
        if (midpointX > maxPosX)
        {
            midpointX = maxPosX;
        }
        if (midpointX < minPosX)
        {
            midpointX = minPosX;
        }
        if (midpointY > maxPosY)
        {
            midpointY = maxPosY;
        }
        if (midpointY < minPosY)
        {
            midpointY = minPosY;
        }
        

        Camera.main.transform.position = new Vector3(midpointX, midpointY, Camera.main.transform.position.z);
    }

    void setCameraSize()
    {
        //horizontal size is based on actual screen ratio
        float minSizeX = minSizeY * Screen.width / Screen.height;

        //multiplying by 0.5, because the ortographicSize is actually half the height
        //float width = Mathf.Abs(Player1.position.x - Player2.position.x) * 0.5f;
        float width = Player1.position.x - Player2.position.x * 0.5f;
        float height = Mathf.Abs(Player1.position.y - Player2.position.y) * 0.5f;
        float camSizeX;
        //computing the size
        //float camSizeX = Mathf.Max(width, minSizeX);
        if (Mathf.Abs(width) > minSizeX)
        {
            camSizeX = width;
        }
        else
        {
            camSizeX = minSizeX;
        }
        float newCamSize;
        //float newCamSize = Mathf.Max(height * 1.5f, camSizeX * Screen.height / Screen.width, minSizeY) * 1.2f;
        if (((Mathf.Abs(camSizeX) * Screen.height / Screen.width) > (height * 1.5f)) && ((Mathf.Abs(camSizeX) * Screen.height / Screen.width) > minSizeY))
        {
            newCamSize = camSizeX * Screen.height / Screen.width;
        }
        else
        {
            newCamSize = Mathf.Max(height * 1.5f, minSizeY);
        }
        //Debug.Log(newCamSize);
        if (Mathf.Abs(newCamSize) > maxSizeY)
        {
            newCamSize = maxSizeY;
            //Debug.Log("max camera size");
        }
            
        //Debug.Log("height: "+height+"\tformula: "+ camSizeX * Screen.height / Screen.width + "\tminSizeY: "+minSizeY+"\tCamera: "+newCamSize);



        Camera.main.orthographicSize = Mathf.Abs(newCamSize);
    }

    // Update is called once per frame
    void Update()
    {
        setCameraPos();
        setCameraSize();
    }
}
