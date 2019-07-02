using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HardShellStudios.CompleteControl;

[AddComponentMenu("Hard Shell Studios/Complete Control/UI Rebind Button")]
[RequireComponent(typeof(Button))]
public class UI_RebindKey : MonoBehaviour {

    public string uniqueName;
    public KeyTarget keyTarget = KeyTarget.PositivePrimary;
    public Text text;
    public bool constantUpdate = false;

    private string originalString;
    private bool isBinding = false;
    private Button button;
    private bool textSettled = false;

    // delay for controller
    private float delaytime = 0.0001f;
    private float timer = 0.0f;

    private void Start()
    {
        originalString = text.text;
        button = GetComponent<Button>();
        button.onClick.AddListener(RebindKey);
        timer = 0.0f;
    }

    private void Update()
    {
        if (isBinding)
        {
            
            if (timer < delaytime)
            {
                timer += Time.deltaTime;
                return;
            }
            timer = 0.0f;
            

            KeyCode key = hInput.CurrentKeyDown();
//            Debug.Log(key);

            if (uniqueName.Equals("P1_att") || uniqueName.Equals("P1_sp") || uniqueName.Equals("P1_jmp") || uniqueName.Equals("P1_shield"))
            {
                if (key != KeyCode.None)
                {
                    if (key == KeyCode.JoystickButton0)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick1Button0, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton1)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick1Button1, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton2)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick1Button2, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton3)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick1Button3, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton4)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick1Button4, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton5)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick1Button5, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton6)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick1Button6, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton7)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick1Button7, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton8)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick1Button8, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton9)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick1Button9, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton10)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick1Button10, keyTarget);
                    }
                    else if (key == hInput.RebindRemovalKey)
                        hInput.SetKey(uniqueName, KeyCode.None, keyTarget);
                    else
                        hInput.SetKey(uniqueName, key, keyTarget);

                    isBinding = false;
                    button.interactable = true;
                }
            }
            else if (uniqueName.Equals("P2_att") || uniqueName.Equals("P2_sp") || uniqueName.Equals("P2_jmp") || uniqueName.Equals("P2_shield"))
            {   
                if (key != KeyCode.None)
                {
                    if (key == KeyCode.JoystickButton0)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick2Button0, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton1)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick2Button1, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton2)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick2Button2, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton3)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick2Button3, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton4)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick2Button4, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton5)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick2Button5, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton6)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick2Button6, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton7)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick2Button7, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton8)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick2Button8, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton9)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick2Button9, keyTarget);
                    }
                    else if (key == KeyCode.JoystickButton10)
                    {
                        hInput.SetKey(uniqueName, KeyCode.Joystick2Button10, keyTarget);
                    }
                    else if (key == hInput.RebindRemovalKey)
                        hInput.SetKey(uniqueName, KeyCode.None, keyTarget);
                    else
                        hInput.SetKey(uniqueName, key, keyTarget);

                    isBinding = false;
                    button.interactable = true;
                }
            }
            else
            {
                if (key != KeyCode.None)
                {
                    if (key == hInput.RebindRemovalKey)
                        hInput.SetKey(uniqueName, KeyCode.None, keyTarget);
                    else
                        hInput.SetKey(uniqueName, key, keyTarget);

                    isBinding = false;
                    button.interactable = true;
                }
            }
        }
        else
        {
            if (!textSettled || constantUpdate)
            {
                if (originalString.Contains("{key}") || originalString.Contains("{name}"))
                {
                    text.text = originalString.Replace("{key}", hInput.DetailsFromKey(uniqueName, keyTarget).ToString());
                }
                else
                    text.text = originalString;
            }
        }
    }

    public void RebindKey()
    {
        text.text = "PRESS ANY KEY";
        textSettled = false;
        isBinding = true;
        button.interactable = false;
    }
}
