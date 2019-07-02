using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoopCreator : MonoBehaviour {

    public Transform player;
    public bool visible = true;
    public Camera cam, main;
    public RawImage hoop;
    public RectTransform loc;

    public GameObject border;
    public float borderThickness = 5.0f;
    private Image borderI;
    private RectTransform borderT;

    // Use this for initialization
    void Start()
    {
        main = Camera.main;
        hoop = GetComponent<RawImage>();
        loc = GetComponent<RectTransform>();

        //border = loc.GetChild(0).gameObject;
        borderI = border.GetComponent<Image>();
        borderT = border.GetComponent<RectTransform>();
        borderT.sizeDelta = new Vector2(loc.rect.width + borderThickness, loc.rect.height + borderThickness);
    }

    void showHoop()
    {
        //Debug.Log("Player Invisible");
        cam.enabled = true;
        hoop.enabled = true;
        borderI.enabled = true;

        float x = 0f;
        float y = 0f;
        int top = -1;
        int right = -1;

        //(0,0) represents bottom-left of screen
        //(1,1) represents upper-right of screen
        //therefore, if x<0, we're to the left of the screen, and if x>0, we're to the right of the screen
        //also, if y<0, we're below the screen, and if y>0, we're above the screen
        Vector3 playerLoc = main.WorldToViewportPoint(player.position);

        if (playerLoc.y > 1)
            top = 1;
        else if (playerLoc.y < 0)
            top = 0;

        if (playerLoc.x > 1)
            right = 1;
        else if (playerLoc.x < 0)
            right = 0;




        if (top == -1)      //in the horizontal-middle of screen
            y = Mathf.Abs(playerLoc.y) * Screen.height;
        if (top == 0)       //below the screen
            y = 0 + (hoop.rectTransform.rect.height / 2) + borderThickness;
        if (top == 1)       //above the screen
            y = Screen.height - (hoop.rectTransform.rect.height / 2) - borderThickness;

        if (right == -1)    //in the vertical-middle of screen
            x = Mathf.Abs(playerLoc.x) * Screen.width;
        if (right == 0)     //left of the screen
            x = 0 + (hoop.rectTransform.rect.width / 2);
        if (right == 1)     //right of the screen
            x = Screen.width - (hoop.rectTransform.rect.width / 2);

        loc.position = new Vector3(x, y, loc.position.z);
        borderT.position = new Vector3(x, y, borderT.position.z);
    }

    void disableHoop()
    {
        //Debug.Log("Player Visible");
        cam.enabled = false;
        hoop.enabled = false;
        borderI.enabled = false;
    }

    // Update is called once per frame
    void Update () {
        Vector3 screenPoint = main.WorldToViewportPoint(player.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (!onScreen)
            showHoop();
        else
            disableHoop();

    }
}