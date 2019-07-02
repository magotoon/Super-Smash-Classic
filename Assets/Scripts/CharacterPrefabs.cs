using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPrefabs : MonoBehaviour {

    public GameObject mainCamera;
    public GameObject p1Hoop;
    public GameObject p2Hoop;
    public GameObject cameraP1;
    public GameObject cameraP2;
    public GameObject ThumbnailP1;
    public GameObject ThumbnailP2;

    // Firebrand
    public GameObject firebrandP1;
    public GameObject firebrandP2;
    public GameObject firebrandAI;
    // End

    // Link
    public GameObject linkP1;
    public GameObject linkP2;
    public GameObject linkAI;
    // End

    // Mario
    public GameObject marioP1;
    public GameObject marioP2;
    public GameObject marioAI;
    // End

    // Samus
    public GameObject samusP1;
    public GameObject samusP2;
    public GameObject samusAI;
    // End

    // Use this for initialization
    void Awake () {

        // instantiate level
        GameObject level = Instantiate(MatchPref.levelSelected, Vector3.zero, Quaternion.identity);
        //Debug.Log("created the level");
        level.GetComponentInChildren<AudioSource>().volume *= MatchPref.musicVol;
        CameraFollow cf = mainCamera.GetComponent<CameraFollow>();

        // set camera boundaries
        switch (MatchPref.level)
        {
            case 1:
                cf.minSizeY = CameraLimits.l1MinSizeY;
                cf.maxSizeY = CameraLimits.l1MaxSizeY;
                cf.minPosX = CameraLimits.l1MinPosX;
                cf.maxPosX = CameraLimits.l1MaxPosX;
                cf.minPosY = CameraLimits.l1MinPosY;
                cf.maxPosY = CameraLimits.l1MaxPosY;
                break;
            case 2:
                cf.minSizeY = CameraLimits.l2MinSizeY;
                cf.maxSizeY = CameraLimits.l2MaxSizeY;
                cf.minPosX = CameraLimits.l2MinPosX;
                cf.maxPosX = CameraLimits.l2MaxPosX;
                cf.minPosY = CameraLimits.l2MinPosY;
                cf.maxPosY = CameraLimits.l2MaxPosY;
                break;
            case 3:
                cf.minSizeY = CameraLimits.l3MinSizeY;
                cf.maxSizeY = CameraLimits.l3MaxSizeY;
                cf.minPosX = CameraLimits.l3MinPosX;
                cf.maxPosX = CameraLimits.l3MaxPosX;
                cf.minPosY = CameraLimits.l3MinPosY;
                cf.maxPosY = CameraLimits.l3MaxPosY;
                break;
            case 4:
                cf.minSizeY = CameraLimits.l4MinSizeY;
                cf.maxSizeY = CameraLimits.l4MaxSizeY;
                cf.minPosX = CameraLimits.l4MinPosX;
                cf.maxPosX = CameraLimits.l4MaxPosX;
                cf.minPosY = CameraLimits.l4MinPosY;
                cf.maxPosY = CameraLimits.l4MaxPosY;
                break;
            case 5:
                cf.minSizeY = CameraLimits.l5MinSizeY;
                cf.maxSizeY = CameraLimits.l5MaxSizeY;
                cf.minPosX = CameraLimits.l5MinPosX;
                cf.maxPosX = CameraLimits.l5MaxPosX;
                cf.minPosY = CameraLimits.l5MinPosY;
                cf.maxPosY = CameraLimits.l5MaxPosY;
                break;
            default:
                break;
        }

        GameObject go1;
        GameObject go2;

        // instantiate player 1
        switch (MatchPref.player1)
        {
            case 1:
                go1 = Instantiate(firebrandP1, Vector3.zero, Quaternion.identity);
                p1Hoop.GetComponent<HoopCreator>().player = go1.transform;
                cameraP1.GetComponent<magnify>().Player = go1.transform;
                ThumbnailP1.GetComponent<UnityEngine.UI.Image>().sprite = MatchPref.p1Sprite;
                break;
            case 2:
                go1 = Instantiate(linkP1, Vector3.zero, Quaternion.identity);
                p1Hoop.GetComponent<HoopCreator>().player = go1.transform;
                cameraP1.GetComponent<magnify>().Player = go1.transform;
                ThumbnailP1.GetComponent<UnityEngine.UI.Image>().sprite = MatchPref.p1Sprite;
                break;
            case 3:
                go1 = Instantiate(marioP1, Vector3.zero, Quaternion.identity);
                p1Hoop.GetComponent<HoopCreator>().player = go1.transform;
                cameraP1.GetComponent<magnify>().Player = go1.transform;
                ThumbnailP1.GetComponent<UnityEngine.UI.Image>().sprite = MatchPref.p1Sprite;
                break;
            case 4:
                go1 = Instantiate(samusP1, Vector3.zero, Quaternion.identity);
                p1Hoop.GetComponent<HoopCreator>().player = go1.transform;
                cameraP1.GetComponent<magnify>().Player = go1.transform;
                ThumbnailP1.GetComponent<UnityEngine.UI.Image>().sprite = MatchPref.p1Sprite;
                break;
            default:
                break;
        }

        // instantiate player 2
        if (MatchPref.gameMode == 1)
        {
            // player 2 is AI
            switch (MatchPref.player2)
            {
                case 1:
                    go2 = Instantiate(firebrandAI, Vector3.zero, Quaternion.identity);
                    p2Hoop.GetComponent<HoopCreator>().player = go2.transform;
                    cameraP2.GetComponent<magnify>().Player = go2.transform;
                    ThumbnailP2.GetComponent<UnityEngine.UI.Image>().sprite = MatchPref.p2Sprite;
                    break;
                case 2:
                    go2 = Instantiate(linkAI, Vector3.zero, Quaternion.identity);
                    p2Hoop.GetComponent<HoopCreator>().player = go2.transform;
                    cameraP2.GetComponent<magnify>().Player = go2.transform;
                    ThumbnailP2.GetComponent<UnityEngine.UI.Image>().sprite = MatchPref.p2Sprite;
                    break;
                case 3:
                    go2 = Instantiate(marioAI, Vector3.zero, Quaternion.identity);
                    p2Hoop.GetComponent<HoopCreator>().player = go2.transform;
                    cameraP2.GetComponent<magnify>().Player = go2.transform;
                    ThumbnailP2.GetComponent<UnityEngine.UI.Image>().sprite = MatchPref.p2Sprite;
                    break;
                case 4:
                    go2 = Instantiate(samusAI, Vector3.zero, Quaternion.identity);
                    p2Hoop.GetComponent<HoopCreator>().player = go2.transform;
                    cameraP2.GetComponent<magnify>().Player = go2.transform;
                    ThumbnailP2.GetComponent<UnityEngine.UI.Image>().sprite = MatchPref.p2Sprite;
                    break;
                default:
                    break;
            }
        }
        else
        {
            // Player 2 is Human
            switch (MatchPref.player2)
            {
                case 1:
                    go2 = Instantiate(firebrandP2, Vector3.zero, Quaternion.identity);
                    p2Hoop.GetComponent<HoopCreator>().player = go2.transform;
                    cameraP2.GetComponent<magnify>().Player = go2.transform;
                    ThumbnailP2.GetComponent<UnityEngine.UI.Image>().sprite = MatchPref.p2Sprite;
                    break;
                case 2:
                    go2 = Instantiate(linkP2, Vector3.zero, Quaternion.identity);
                    p2Hoop.GetComponent<HoopCreator>().player = go2.transform;
                    cameraP2.GetComponent<magnify>().Player = go2.transform;
                    ThumbnailP2.GetComponent<UnityEngine.UI.Image>().sprite = MatchPref.p2Sprite;
                    break;
                case 3:
                    go2 = Instantiate(marioP2, Vector3.zero, Quaternion.identity);
                    p2Hoop.GetComponent<HoopCreator>().player = go2.transform;
                    cameraP2.GetComponent<magnify>().Player = go2.transform;
                    ThumbnailP2.GetComponent<UnityEngine.UI.Image>().sprite = MatchPref.p2Sprite;
                    break;
                case 4:
                    go2 = Instantiate(samusP2, Vector3.zero, Quaternion.identity);
                    p2Hoop.GetComponent<HoopCreator>().player = go2.transform;
                    cameraP2.GetComponent<magnify>().Player = go2.transform;
                    ThumbnailP2.GetComponent<UnityEngine.UI.Image>().sprite = MatchPref.p2Sprite;
                    break;
                default:
                    break;
            }
        }
    }
}
