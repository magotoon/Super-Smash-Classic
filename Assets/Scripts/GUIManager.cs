using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GUIManager : MonoBehaviour {

    // Variables to persist
    private int player1;
    private int player2;
    private GameObject levelSelected;
    private int gameMode;
    private Sprite p1Sprite;
    private Sprite p2Sprite;
    // End

    private GameObject[] charBtns;

    private bool player1Selected = false;
    private bool p1SpriteSelected = false;
    private bool player2Selected = false;
    private bool p2SpriteSelected = false;
    private bool p1MatchSpriteSelected = false;

    public int scene;

    // Character select variables
    public GameObject p1Image;
    public GameObject p2Image;
    public GameObject firstSelected;
    public GameObject startBtn;
    public Sprite cpuBanner;
    private GameObject diffBanner;
    // End

    // Variables for Stage Select
    public GameObject stagePV;
    // End

    // Fight screen Virables
    public GameObject fighter1;
    public GameObject fighter2;
    public Text p1_dmg;
    public Text p2_dmg;
    public Image Counter;
    public Sprite three;
    public Sprite two;
    public Sprite one;
    public Sprite go;
    public Sprite clear;
    public Image[] p1Lives;
    public Image[] p2Lives;
    public GameObject pauseMenu;
    private bool paused = false;
    private float startTimer;
    // End

    // Controller setup and Title screen variables
    private GameObject selected;
    // End

    // Settings page variables
    public GameObject musicSlider;
    public GameObject sfxSlider;
    // End

    public P1_CharController characterController1;
    public P2_CharController characterController2;

    public void Awake()
    {
        player1 = MatchPref.player1;
        player2 = MatchPref.player2;
        levelSelected = MatchPref.levelSelected;
        gameMode = MatchPref.gameMode;
        p1Sprite = MatchPref.p1Sprite;
        p2Sprite = MatchPref.p2Sprite;

    }

    public void Start()
    {
        // Character select
        if (scene == 1)
        {
            diffBanner = GameObject.FindGameObjectWithTag("cpuDiff");
            string str = "" + gameMode + "-player";
            GameObject modeText = GameObject.FindGameObjectWithTag("Game_Mode_tag");
            modeText.GetComponent<UnityEngine.UI.Text>().text = str;

            if (gameMode == 1)
            {
                GameObject p2Banner = GameObject.FindGameObjectWithTag("P2_Banner");
                p2Banner.GetComponent<UnityEngine.UI.Image>().sprite = cpuBanner;
                MatchPref.cpuDif = 0;
            }
            else
            {
                diffBanner.SetActive(false);
            }
        }
        // Stage Select Screen
        else if (scene == 2)
        {
            string str = "" + gameMode + "-player";
            GameObject modeText = GameObject.FindGameObjectWithTag("Game_Mode_tag");
            modeText.GetComponent<UnityEngine.UI.Text>().text = str;
        }
        // Fight Screen
        else if (scene == 3)
        {
            fighter1 = GameObject.FindGameObjectWithTag("Player1");
            fighter2 = GameObject.FindGameObjectWithTag("Player2");
            startTimer = 4;
        }
        // Controler setup
        else if (scene == 4 || scene == 0)
        {
            selected = this.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject;
        }
        // settings
        else if (scene == 5)
        {
            musicSlider.GetComponent<UnityEngine.UI.Slider>().value = MatchPref.musicVol;
            sfxSlider.GetComponent<UnityEngine.UI.Slider>().value = MatchPref.sfxVol;
        }
        // machanics test scene
        else if (scene == -1)
        {
            fighter1 = GameObject.FindGameObjectWithTag("Player1");
            fighter2 = GameObject.FindGameObjectWithTag("Player2");
        }
    }

    public void SelectCharacter(int obj)
    {
        if (!player1Selected)
        {
            //Debug.Log("OBJ: " + obj);
            player1 = obj;
            //Debug.Log("Player1: " + player1);
            player1Selected = true;                                                        //!!!!!!!!!!!!!!!!!Character selection function here!!!!!!!!!!!!!!!!!!!
        }
        else
        {
            //Debug.Log("OBJ: " + obj);
            player2 = obj;
            //Debug.Log("Player2: " + player2);
            player2Selected = true;
            
        }
    }

    public void SetSprite(Sprite sprite)
    {
        if (!p1SpriteSelected)
        {
            // set sprite to p1Image
            p1SpriteSelected = true;

            p1Image.SetActive(true);
            p1Image.transform.GetComponent<UnityEngine.UI.Image>().sprite = sprite;
        }
        else
        {
            //set sprite to p2Image
            p2Image.SetActive(true);
            p2Image.transform.GetComponent<UnityEngine.UI.Image>().sprite = sprite;

            charBtns = GameObject.FindGameObjectsWithTag("Char_Select_Btn");
            // disable selection of characters
            for (int i = 0; i < charBtns.Length; i++)
            {
                charBtns[i].GetComponent<UnityEngine.UI.Button>().interactable = false;
            }
            p2SpriteSelected = true;

            // Make the Start banner appear and focus on it
            startBtn.SetActive(true);
            this.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(startBtn);
        }
    }

    public void SetMatchSprite(Sprite sprite)
    {
        // Store the sprite that will show in the damage counter
        if (!p1MatchSpriteSelected)
        {
            p1Sprite = sprite;
            p1MatchSpriteSelected = true;
        }
        else
        {
            p2Sprite = sprite;
        }
    }

    public void SelectMode (int mode)
    {
        // Persist the value and go to character select screen
        MatchPref.gameMode = mode;

        SceneManager.LoadScene("CharacterSelect");
    }

    public void toCharacterSelect ()
    {
        MatchPref.player1 = -1;
        MatchPref.player2 = -1;
        MatchPref.levelSelected = null;
        MatchPref.p1Sprite = null;
        MatchPref.p2Sprite = null;

        SceneManager.LoadScene("CharacterSelect");
    }

    public void ToTitleScreen ()
    {
        MatchPref.player1 = -1;
        MatchPref.player2 = -1;
        MatchPref.levelSelected = null;
        MatchPref.gameMode = 0;
        MatchPref.p1Sprite = null;
        MatchPref.p2Sprite = null;

        Time.timeScale = 1f;

        SceneManager.LoadScene("TitleScreen");
    }

    public void ToStageSelect()
    {
        MatchPref.player1 = player1;
        MatchPref.player2 = player2;
        MatchPref.p1Sprite = p1Sprite;
        MatchPref.p2Sprite = p2Sprite;

        SceneManager.LoadScene("StageSelect");
    }

    public void toControlerSetup()
    {
        SceneManager.LoadScene("ControlSetup");
    }

    public void SetStage (GameObject Stage)
    {
        MatchPref.levelSelected = Stage;

        SceneManager.LoadScene("FightScene");
    }

    public void StageNumber (int num)
    {
        MatchPref.level = num;
    }

    public void ExitGame()
    {
        MatchPref.player1 = -1;
        MatchPref.player2 = -1;
        MatchPref.levelSelected = null;
        MatchPref.gameMode = 0;
        MatchPref.p1Sprite = null;
        MatchPref.p2Sprite = null;

        Application.Quit();
    }

    public void ToSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void SetDifficulty()
    {
        MatchPref.cpuDif = diffBanner.GetComponent<UnityEngine.UI.Dropdown>().value;
        //Debug.Log(MatchPref.cpuDif);
    }

    public void TogglePause()
    {
        if (paused)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            paused = false;
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            paused = true;
        }
    }

    private void Update()
    {
        switch (scene)
        {
            // machanics test scene
            case -1:
                if (hInput.GetButtonDown("Pause"))
                {
                    TogglePause();
                }
                if (fighter1 != null)
                {
                    p1_dmg.text = "" + fighter1.GetComponent<CharacterStats>().damagePercent + "%";
                }
                if (fighter2 != null)
                {
                    p2_dmg.text = "" + fighter2.GetComponent<CharacterStats>().damagePercent + "%";
                }
                break;
            // Title Screen
            case 0:
                UnityEngine.EventSystems.EventSystem es = this.GetComponent<UnityEngine.EventSystems.EventSystem>();

                if (es.currentSelectedGameObject == null)
                {
                    es.SetSelectedGameObject(selected);
                }
                else
                {
                    selected = es.currentSelectedGameObject;
                }
                break;

            // Character Select Screen
            case 1:
                // If b button is pressed from a controller and the user has a character selected unselect the character
                if (Input.GetButtonDown("Cancel"))
                {
                    if (p2SpriteSelected)
                    {
                        charBtns = GameObject.FindGameObjectsWithTag("Char_Select_Btn");
                        // Enable selection of characters
                        for (int i = 0; i < charBtns.Length; i++)
                        {
                            charBtns[i].GetComponent<UnityEngine.UI.Button>().interactable = true;
                        }

                        // Set a button as first selected
                        this.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(firstSelected);

                        // Deselect player2's character
                        p2SpriteSelected = false;
                        player2Selected = false;

                        p2Image.SetActive(false);
                        startBtn.SetActive(false);
                    }
                    else if (p1SpriteSelected)
                    {
                        // Deselect player1's character
                        p1SpriteSelected = false;
                        player1Selected = false;
                        p1MatchSpriteSelected = false;

                        p1Image.SetActive(false);
                        p2Image.SetActive(false);
                    }
                    else
                    {
                        // Go back to main menu
                        ToTitleScreen();
                    }
                }

                
                GameObject character = this.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject;
                
                if (character.tag.Equals("Back_Btn") || character.tag.Equals("Untagged") || character.tag.Equals("cpuDiff"))
                {
                    if (!p1SpriteSelected)
                    {
                        p1Image.SetActive(false);
                    }
                    else if (!p2SpriteSelected)
                    {
                        p2Image.SetActive(false);
                    }
                }
                else
                {
                    if (!p1SpriteSelected)
                    {
                        p1Image.SetActive(true);
                        p1Image.GetComponent<UnityEngine.UI.Image>().sprite = character.GetComponent<Thumbnail>().thumbnail;
                    }
                    else if (!p2SpriteSelected)
                    {
                        p2Image.SetActive(true);
                        p2Image.GetComponent<UnityEngine.UI.Image>().sprite = character.GetComponent<Thumbnail>().thumbnail;
                    }
                }
                break;
            
            // Stage Select Screen
            case 2:
                if (Input.GetButtonDown("Cancel"))
                {
                    toCharacterSelect();
                }

                GameObject obj = this.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject;
                if (obj.tag.Equals("Back_Btn"))
                {
                    stagePV.GetComponent<UnityEngine.UI.Image>().sprite = cpuBanner;
                }
                else
                {
                    stagePV.GetComponent<UnityEngine.UI.Image>().sprite = obj.GetComponent<UnityEngine.UI.Image>().sprite;
                }
                break;

            // Fight Screen
            case 3:
                UnityEngine.EventSystems.EventSystem eventManager = this.GetComponent<UnityEngine.EventSystems.EventSystem>();

                if (startTimer >= 0)
                {
                    startTimer -= Time.deltaTime;

                    if (startTimer < 0.1)
                    {
                        Counter.sprite = clear;
                    }
                    else if (startTimer < 1)
                    {
                        Counter.sprite = go;
                    }
                    else if (startTimer < 2)
                    {
                        Counter.sprite = one;
                    }
                    else if (startTimer < 3)
                    {
                        Counter.sprite = two;
                    }
                    else if (startTimer < 4)
                    {
                        Counter.sprite = three;
                    }
                }

                if (hInput.GetButtonDown("Pause"))
                {
                    TogglePause();

                    if (paused)
                    {
                        eventManager.SetSelectedGameObject(firstSelected);
                    }
                }

                if (paused)
                {
                    if (eventManager.currentSelectedGameObject == null)
                    {
                        eventManager.SetSelectedGameObject(firstSelected);
                    }
                    else
                    {
                        firstSelected = eventManager.currentSelectedGameObject;
                    }
                }
                
                if (fighter1 != null)
                {
                    p1_dmg.text = "" + fighter1.GetComponent<CharacterStats>().damagePercent + "%";

                    switch (fighter1.GetComponent<CharacterStats>().numberOfLives)
                    {
                        case 0:

                            // The following commented code was for gathering data in tendencies from testers

                            //try
                            //{
                            //    fighter1.GetComponent<P1_CharController>().printToFile();
                            //}
                            //catch
                            //{
                            //    try
                            //    {
                            //        fighter1.GetComponent<samus_P1_CharController>().printToFile();
                            //    }
                            //    catch
                            //    {
                            //        Debug.Log("No Controller found");
                            //    }
                            //}
                            
                            //if (MatchPref.gameMode == 1 && fighter2 != null)
                            //{
                            //    try
                            //    {
                            //        fighter2.GetComponent<P2_CharController>().printToFile();
                            //    }
                            //    catch
                            //    {
                            //        try
                            //        {
                            //            fighter2.GetComponent<samus_P2_CharController>().printToFile();
                            //        }
                            //        catch
                            //        {
                            //            Debug.Log("No Controller found");
                            //        }
                            //    }
                            //}
                            
                            //player 2 wins
                            if (MatchPref.gameMode == 1)
                            {
                                SceneManager.LoadScene("AIWins");
                            }
                            else
                            {
                                SceneManager.LoadScene("Player2Wins");
                            }
                            break;
                        case 1:
                            // delete second heart if not already deleted
                            if (p1Lives[1] != null)
                            {
                                Destroy(p1Lives[1]);
                                p1Lives[1] = null;
                            }
                            break;
                        case 2:
                            // delete third heart if not already deleted
                            if (p1Lives[2] != null)
                            {
                                Destroy(p1Lives[2]);
                                p1Lives[2] = null;
                            }
                            break;
                        default:
                            break;
                    }
                }
                if (fighter2 != null)
                {
                    p2_dmg.text = "" + fighter2.GetComponent<CharacterStats>().damagePercent + "%";

                    switch (fighter2.GetComponent<CharacterStats>().numberOfLives)
                    {
                        case 0:

                            // The following commented code was for gathering data in tendencies from testers

                            //try
                            //{
                            //    fighter1.GetComponent<P1_CharController>().printToFile();
                            //}
                            //catch
                            //{
                            //    try
                            //    {
                            //        fighter1.GetComponent<samus_P1_CharController>().printToFile();
                            //    }
                            //    catch
                            //    {
                            //        Debug.Log("No Controller found");
                            //    }
                            //}
                            
                            //if (MatchPref.gameMode == 1 && fighter2 != null)
                            //{
                            //    try
                            //    {
                            //        fighter2.GetComponent<P2_CharController>().printToFile();
                            //    }
                            //    catch
                            //    {
                            //        try
                            //        {
                            //            fighter2.GetComponent<samus_P2_CharController>().printToFile();
                            //        }
                            //        catch
                            //        {
                            //            Debug.Log("No Controller found");
                            //        }
                            //    }
                            //}

                            // player 1 wins
                            SceneManager.LoadScene("Player1Wins");
                            break;
                        case 1:
                            // delete second heart if not already deleted
                            if (p2Lives[1] != null)
                            {
                                Destroy(p2Lives[1]);
                                p2Lives[1] = null;
                            }
                            break;
                        case 2:
                            // delete third heart if not already deleted
                            if (p2Lives[2] != null)
                            {
                                Destroy(p2Lives[2]);
                                p2Lives[2] = null;
                            }
                            break;
                        default:
                            break;
                    }
                }
                break;

            // Controler setup
            case 4:
                
                UnityEngine.EventSystems.EventSystem ev = this.GetComponent<UnityEngine.EventSystems.EventSystem>();

                if (ev.currentSelectedGameObject == null)
                {
                    ev.SetSelectedGameObject(selected);
                }
                else
                {
                    selected = ev.currentSelectedGameObject;
                }
                
                break;
            
            // Settings
            case 5:
                MatchPref.musicVol = musicSlider.GetComponent<UnityEngine.UI.Slider>().value;
                MatchPref.sfxVol = sfxSlider.GetComponent<UnityEngine.UI.Slider>().value;

                if (Input.GetButtonDown("Cancel"))
                {
                    ToTitleScreen();
                }
                break;
        }
        
    }
}
