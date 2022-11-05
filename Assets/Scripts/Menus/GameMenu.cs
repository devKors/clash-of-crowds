using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public Image joystick;
    public Image pointer;
    private bool isJoystickActive;
    private int level;

    void OnEnable()
    {
        level = PlayerPrefs.GetInt(SerializableFields.Level, 0);
        Debug.Log(level + " Level");

        if (level > 2)
        {
            joystick.gameObject.SetActive(false);
            pointer.gameObject.SetActive(false);
            isJoystickActive = false;
        }
        else
        {
            isJoystickActive = true;
        }
    }

    void Update()
    {
        if (!isJoystickActive) {
            return;
        }

        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0)) {
            joystick.gameObject.SetActive(false);
            pointer.gameObject.SetActive(false);
        }
    }
}
